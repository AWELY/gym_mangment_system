using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class SubscriptionsForm : Form, IThemeAware
    {
        private DataTable _dt;
        private int _editingId = -1;

        public SubscriptionsForm()
            : this(false)
        {
        }

        public SubscriptionsForm(bool startAddMode)
        {
            InitializeComponent();
            ApplyBackgroundBranding();
            ConfigureEditorForPlanCatalog();
            LoadPlanData();
            WireEvents();
            ResetForm();
            ApplyTheme(ThemeManager.Current);

            if (startAddMode)
            {
                // Editor is inline on this screen; just focus the input.
                txtType.Focus();
            }
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            lblTitle.ForeColor = s.TextPrimary;
            pnlEditor.BackColor = s.Panel;
            lblHint.ForeColor = s.TextMuted;
            lblMember.ForeColor = s.TextPrimary;
            lblType.ForeColor = s.TextPrimary;
            lblDuration.ForeColor = s.TextPrimary;
            lblPrice.ForeColor = s.TextPrimary;
            lblStartDate.ForeColor = s.TextPrimary;

            txtType.BackColor = s.InputBackground;
            txtType.ForeColor = s.InputForeground;
            cmbMember.BackColor = s.InputBackground;
            cmbMember.ForeColor = s.InputForeground;
            cmbDurationUnit.BackColor = s.InputBackground;
            cmbDurationUnit.ForeColor = s.InputForeground;
            numDuration.BackColor = s.InputBackground;
            numDuration.ForeColor = s.InputForeground;
            numPrice.BackColor = s.InputBackground;
            numPrice.ForeColor = s.InputForeground;
            dtpStartDate.CalendarMonthBackground = s.InputBackground;
            dtpStartDate.CalendarForeColor = s.InputForeground;

            btnClearForm.BackColor = s.SecondaryButton;
            btnClearForm.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.White;
            btnClearForm.FlatAppearance.MouseOverBackColor = s.SecondaryButtonHover;

            ThemeManager.StyleDataGridView(gridSubs, s);
        }

        private void ApplyBackgroundBranding()
        {
            float op = ThemeManager.BrandingOpacity();
            Image faded = ImageAssets.TryLoadToughBackground("subscriptions", op);
            if (faded == null) return;
            this.BackgroundImage = faded;
            this.BackgroundImageLayout = ImageLayout.Center;
            gridSubs.BackgroundImage = faded;
            gridSubs.BackgroundImageLayout = ImageLayout.Center;
            pnlEditor.BackgroundImage = faded;
            pnlEditor.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void ConfigureEditorForPlanCatalog()
        {
            lblTitle.Text = "📋 إعداد أنواع الاشتراك";
            lblType.Text = "اسم الخطة";
            lblDuration.Text = "المدة";
            lblPrice.Text = "السعر $";
            lblHint.Text = "أضف نوع الاشتراك (اسم + مدة + سعر). الأنواع هنا تظهر في صفحة الأعضاء.";

            cmbMember.Visible = false;
            lblMember.Visible = false;
            dtpStartDate.Visible = false;
            lblStartDate.Visible = false;
            btnSaveSubscription.Text = "💾 حفظ الخطة";
        }

        private void LoadPlanData()
        {
            _dt = new DataTable();
            _dt.Columns.Add("ID", typeof(int));
            _dt.Columns.Add("نوع الاشتراك", typeof(string));
            _dt.Columns.Add("المدة", typeof(string));
            _dt.Columns.Add("السعر", typeof(decimal));
            _dt.Columns.Add("اسم العرض", typeof(string));

            foreach (SubscriptionPlan plan in SubscriptionPlanCatalog.GetPlans())
            {
                _dt.Rows.Add(plan.Id, plan.Name, BuildDurationLabel(plan.DurationValue, plan.DurationUnit), plan.Price, plan.DisplayName);
            }

            gridSubs.DataSource = _dt;
            gridSubs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (gridSubs.Columns.Contains("ID"))
                gridSubs.Columns["ID"].Visible = false;
            gridSubs.Columns["السعر"].DefaultCellStyle.Format = "N2";
        }

        private void WireEvents()
        {
            btnSaveSubscription.Click += BtnSaveSubscription_Click;
            btnClearForm.Click += (s, e) => ResetForm();
            btnDeleteSubscription.Click += BtnDeleteSubscription_Click;
            gridSubs.SelectionChanged += GridSubs_SelectionChanged;
        }

        private void BtnSaveSubscription_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtType.Text))
            {
                MessageBox.Show("أدخل اسم نوع الاشتراك.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string type = txtType.Text.Trim();
            int durationValue = (int)numDuration.Value;
            string durationUnit = cmbDurationUnit.SelectedItem?.ToString() ?? "شهر";
            decimal price = numPrice.Value;
            string durationLabel = BuildDurationLabel(durationValue, durationUnit);
            string displayName = type + " - " + price.ToString("0.##") + "$ / " + durationLabel;

            if (_editingId == -1)
            {
                int newId = _dt.Rows.Count > 0 ? _dt.AsEnumerable().Max(r => r.Field<int>("ID")) + 1 : 1;
                _dt.Rows.Add(newId, type, durationLabel, price, displayName);
                SubscriptionPlanCatalog.Upsert(new SubscriptionPlan { Id = newId, Name = type, Price = price, DurationValue = durationValue, DurationUnit = durationUnit });
                MessageBox.Show("تمت إضافة نوع الاشتراك.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataRow row = _dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("ID") == _editingId);
                if (row != null)
                {
                    row["نوع الاشتراك"] = type;
                    row["المدة"] = durationLabel;
                    row["السعر"] = price;
                    row["اسم العرض"] = displayName;
                    SubscriptionPlanCatalog.Upsert(new SubscriptionPlan { Id = _editingId, Name = type, Price = price, DurationValue = durationValue, DurationUnit = durationUnit });
                    MessageBox.Show("تم تحديث نوع الاشتراك.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            ResetForm();
        }

        private void GridSubs_SelectionChanged(object sender, EventArgs e)
        {
            if (gridSubs.SelectedRows.Count == 0) return;

            DataGridViewRow selected = gridSubs.SelectedRows[0];
            if (selected.Cells["ID"].Value == null) return;

            _editingId = Convert.ToInt32(selected.Cells["ID"].Value);
            txtType.Text = selected.Cells["نوع الاشتراك"].Value?.ToString();

            ParseDuration(selected.Cells["المدة"].Value?.ToString(), out int durationValue, out string durationUnit);
            numDuration.Value = Math.Max(numDuration.Minimum, Math.Min(numDuration.Maximum, durationValue));
            cmbDurationUnit.SelectedItem = durationUnit;

            if (decimal.TryParse(selected.Cells["السعر"].Value?.ToString(), out decimal parsedPrice))
                numPrice.Value = Math.Max(numPrice.Minimum, Math.Min(numPrice.Maximum, parsedPrice));
        }

        private void BtnDeleteSubscription_Click(object sender, EventArgs e)
        {
            if (gridSubs.SelectedRows.Count == 0)
            {
                MessageBox.Show("اختر نوع اشتراك من الجدول للحذف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = gridSubs.SelectedRows[0];
            string type = row.Cells["نوع الاشتراك"].Value?.ToString() ?? "";
            if (MessageBox.Show("حذف نوع الاشتراك: " + type + "؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            if (row.Cells["ID"].Value != null)
                SubscriptionPlanCatalog.Delete(Convert.ToInt32(row.Cells["ID"].Value));

            gridSubs.Rows.Remove(row);
            ResetForm();
        }

        private void ResetForm()
        {
            _editingId = -1;
            txtType.Text = "شهري";
            numDuration.Value = 1;
            cmbDurationUnit.SelectedItem = "شهر";
            numPrice.Value = 150;
            gridSubs.ClearSelection();
        }

        private static string BuildDurationLabel(int durationValue, string durationUnit)
        {
            return durationValue + " " + durationUnit;
        }

        private static void ParseDuration(string durationLabel, out int value, out string unit)
        {
            value = 1;
            unit = "شهر";
            if (string.IsNullOrWhiteSpace(durationLabel)) return;

            string[] parts = durationLabel.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                int.TryParse(parts[0], out value);
                unit = parts[1] == "سنة" ? "سنة" : "شهر";
            }
        }
    }
}
