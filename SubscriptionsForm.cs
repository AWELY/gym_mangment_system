using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace gym_mangment_system
{
    public partial class SubscriptionsForm : Form, IThemeAware
    {
        private DataTable _dt;
        private int _editingId = -1;
        private FlowLayoutPanel _cardHost;
        private Guna2Button _btnAddPlan;
        private Label _lblEditorTitle;
        private Panel _rootPanel;

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
            BuildCardHost();
            WireEvents();
            ResetForm();
            ApplyTheme(ThemeManager.Current);

            if (startAddMode)
                ShowEditorOverlay(isNew: true);
        }

        // ── Figma card layout + overlay editor ─────────────
        private void BuildCardHost()
        {
            gridSubs.Visible = false;

            UiColorScheme s0 = ThemeManager.Current;

            // Root holds a toolbar row (add button) on top and the scrolling cards below,
            // so the button sits in its own band instead of floating over the cards.
            _rootPanel = new Panel { Dock = DockStyle.Fill, BackColor = s0.ContentHost };
            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, BackColor = Color.Transparent };
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var toolbar = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            _btnAddPlan = GunaUi.ToolbarButton("➕ إضافة خطة", FigmaPalette.Primary, new Point(18, 14));
            _btnAddPlan.Click += (_, __) => ShowEditorOverlay(isNew: true);
            toolbar.Controls.Add(_btnAddPlan);

            _cardHost = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                AutoScroll    = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents  = true,
                Padding       = new Padding(18, 0, 18, 18),
                BackColor     = s0.ContentHost
            };

            tlp.Controls.Add(toolbar, 0, 0);
            tlp.Controls.Add(_cardHost, 0, 1);
            _rootPanel.Controls.Add(tlp);
            Controls.Add(_rootPanel);
            _rootPanel.BringToFront();

            // Turn the inline editor into a centered overlay (Figma opens a form).
            pnlEditor.Dock = DockStyle.None;
            pnlEditor.Size = new Size(440, 320);
            pnlEditor.Visible = false;
            btnDeleteSubscription.Visible = false; // delete is per-card now

            _lblEditorTitle = new Label
            {
                Text      = "إضافة خطة اشتراك",
                Font      = new Font("Segoe UI", 15F, FontStyle.Bold),
                Location  = new Point(20, 14),
                Size      = new Size(400, 32),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };
            pnlEditor.Controls.Add(_lblEditorTitle);
            LayoutEditorOverlay();
        }

        private void LayoutEditorOverlay()
        {
            // Stack the catalog fields vertically for the modal.
            lblType.AutoSize = false;
            lblType.Location = new Point(220, 56); lblType.Size = new Size(200, 22); lblType.TextAlign = ContentAlignment.MiddleRight;
            txtType.Location = new Point(20, 80);  txtType.Size = new Size(400, 27);

            lblDuration.AutoSize = false;
            lblDuration.Location = new Point(220, 116); lblDuration.Size = new Size(200, 22); lblDuration.TextAlign = ContentAlignment.MiddleRight;
            numDuration.Location = new Point(230, 140); numDuration.Size = new Size(190, 27);
            cmbDurationUnit.Location = new Point(20, 140); cmbDurationUnit.Size = new Size(190, 27);

            lblPrice.AutoSize = false;
            lblPrice.Location = new Point(220, 176); lblPrice.Size = new Size(200, 22); lblPrice.TextAlign = ContentAlignment.MiddleRight;
            numPrice.Location = new Point(20, 200); numPrice.Size = new Size(400, 27);

            btnSaveSubscription.Location = new Point(20, 250);  btnSaveSubscription.Size = new Size(195, 40); btnSaveSubscription.Text = "💾 حفظ";
            btnClearForm.Location = new Point(225, 250); btnClearForm.Size = new Size(195, 40); btnClearForm.Text = "إلغاء";

            lblHint.Visible = false;
        }

        private void ShowEditorOverlay(bool isNew)
        {
            if (isNew) ResetForm();
            _lblEditorTitle.Text = isNew ? "إضافة خطة اشتراك" : "تعديل خطة الاشتراك";
            pnlEditor.Location = new Point((ClientSize.Width - pnlEditor.Width) / 2, (ClientSize.Height - pnlEditor.Height) / 2);
            pnlEditor.Visible = true;
            pnlEditor.BringToFront();
            txtType.Focus();
        }

        private void HideEditorOverlay() => pnlEditor.Visible = false;

        private void BuildPlanCards()
        {
            if (_cardHost == null) return;
            UiColorScheme s = ThemeManager.Current;
            _cardHost.SuspendLayout();
            foreach (Control c in _cardHost.Controls) c.Dispose();
            _cardHost.Controls.Clear();

            foreach (SubscriptionPlan plan in SubscriptionPlanCatalog.GetPlans())
                _cardHost.Controls.Add(BuildPlanCard(plan, s));

            _cardHost.ResumeLayout();
        }

        private Guna2Panel BuildPlanCard(SubscriptionPlan plan, UiColorScheme s)
        {
            int w = 320, h = 180, pad = 18, innerW = w - pad * 2;
            Guna2Panel card = GunaUi.Card(w, h, s.Card, s.BorderSubtle);

            Label name = new Label { Text = plan.Name, Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = s.TextPrimary, Location = new Point(pad, 16), Size = new Size(innerW, 30), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label price = new Label { Text = plan.Price.ToString("0") + " ريال", Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = FigmaPalette.Primary, Location = new Point(pad, 50), Size = new Size(innerW, 32), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label dur = new Label { Text = "المدة: " + BuildDurationLabel(plan.DurationValue, plan.DurationUnit), Font = new Font("Segoe UI", 10F), ForeColor = s.TextMuted, Location = new Point(pad, 86), Size = new Size(innerW, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };

            int btnW = (innerW - 10) / 2, btnY = 126, btnH = 36;
            Guna2Button del = GunaUi.Button("🗑  حذف", FigmaPalette.Red, new Point(pad, btnY), new Size(btnW, btnH));
            Guna2Button edit = GunaUi.Button("✎  تعديل", FigmaPalette.BlueBtn, new Point(pad + btnW + 10, btnY), new Size(btnW, btnH));
            int id = plan.Id;
            string nm = plan.Name;
            del.Click += (_, __) => DeletePlan(id, nm);
            edit.Click += (_, __) => EditPlan(id);

            card.Controls.Add(name); card.Controls.Add(price); card.Controls.Add(dur);
            card.Controls.Add(del); card.Controls.Add(edit);
            return card;
        }

        private void EditPlan(int id)
        {
            var plan = SubscriptionPlanCatalog.GetPlans().FirstOrDefault(p => p.Id == id);
            if (plan == null) return;
            _editingId = id;
            txtType.Text = plan.Name;
            numDuration.Value = Math.Max(numDuration.Minimum, Math.Min(numDuration.Maximum, plan.DurationValue));
            cmbDurationUnit.SelectedItem = plan.DurationUnit == "سنة" ? "سنة" : "شهر";
            numPrice.Value = Math.Max(numPrice.Minimum, Math.Min(numPrice.Maximum, plan.Price));
            ShowEditorOverlay(isNew: false);
        }

        private void DeletePlan(int id, string name)
        {
            if (MessageBox.Show("حذف نوع الاشتراك: " + name + "؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            SubscriptionPlanCatalog.Delete(id);
            DataRow row = _dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("ID") == id);
            if (row != null) row.Delete();
            _dt.AcceptChanges();
            BuildPlanCards();
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            lblTitle.ForeColor = s.TextPrimary;
            pnlEditor.FillColor = s.Panel;
            pnlEditor.BorderColor = s.BorderSubtle;
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

            btnClearForm.FillColor = s.SecondaryButton;
            btnClearForm.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.White;
            btnSaveSubscription.FillColor = FigmaPalette.GreenBtn;
            btnDeleteSubscription.FillColor = FigmaPalette.Red;

            ThemeManager.StyleDataGridView(gridSubs, s);

            if (_lblEditorTitle != null) _lblEditorTitle.ForeColor = s.TextPrimary;
            if (_rootPanel != null) _rootPanel.BackColor = s.ContentHost;
            if (_cardHost != null)
            {
                _cardHost.BackColor = s.ContentHost;
                BuildPlanCards();
            }
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
            btnClearForm.Click += (s, e) => HideEditorOverlay();
            btnDeleteSubscription.Click += BtnDeleteSubscription_Click;
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
            BuildPlanCards();
            HideEditorOverlay();
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
