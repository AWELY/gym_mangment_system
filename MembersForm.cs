using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class MembersForm : Form
    {
        private DataTable _dt;
        private bool _isEditing = false;
        private int  _editingRowIndex = -1;

        public MembersForm()
        {
            InitializeComponent();
            ApplyBackgroundBranding();
            ApplyGridOpacityStyle();
            LoadSubscriptionPlans();
            LoadMockData();
            WireEvents();
            UpdateMemberCount();
        }

        private void ApplyBackgroundBranding()
        {
            Image faded = ImageAssets.TryLoadToughBackground("members", 0.22f);
            if (faded == null) return;
            this.BackgroundImage = faded;
            this.BackgroundImageLayout = ImageLayout.Center;
            gridMembers.BackgroundImage = faded;
            gridMembers.BackgroundImageLayout = ImageLayout.Center;
            pnlSearch.BackgroundImage = faded;
            pnlSearch.BackgroundImageLayout = ImageLayout.Stretch;
            pnlActions.BackgroundImage = faded;
            pnlActions.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void ApplyGridOpacityStyle()
        {
            gridMembers.BackgroundColor = Color.FromArgb(12, 12, 14);
            gridMembers.DefaultCellStyle.BackColor = Color.FromArgb(18, 18, 24);
            gridMembers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 40, 52);
            gridMembers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 24, 30);
            gridMembers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 38);
            gridMembers.EnableHeadersVisualStyles = false;
        }

        private void LoadSubscriptionPlans()
        {
            cmbFPlan.Items.Clear();
            foreach (SubscriptionPlan plan in SubscriptionPlanCatalog.GetPlans())
                cmbFPlan.Items.Add(plan.Name);

            if (cmbFPlan.Items.Count == 0)
                cmbFPlan.Items.Add("بدون اشتراك");
        }

        // ═══════════════════════════════════════════
        //  MOCK DATA
        // ═══════════════════════════════════════════
        private void LoadMockData()
        {
            _dt = new DataTable();
            _dt.Columns.Add("ID",              typeof(int));
            _dt.Columns.Add("الاسم",           typeof(string));
            _dt.Columns.Add("الهاتف",          typeof(string));
            _dt.Columns.Add("الجنس",           typeof(string));
            _dt.Columns.Add("الاشتراك",        typeof(string));
            _dt.Columns.Add("السعر",           typeof(string));
            _dt.Columns.Add("المدة",           typeof(string));
            _dt.Columns.Add("تاريخ الانضمام", typeof(string));

            _dt.Rows.Add(1,  "أحمد محمد",    "0501234567", "ذكر",  "Basic Plan",  "30 $",  "1 شهر",  "2026-01-15");
            _dt.Rows.Add(2,  "سارة علي",     "0559876543", "أنثى", "Pro Plan",    "50 $",  "3 شهر",  "2025-06-20");
            _dt.Rows.Add(3,  "خالد إبراهيم", "0561112233", "ذكر",  "Annual Plan", "300 $", "1 سنة",  "2025-11-01");
            _dt.Rows.Add(4,  "نورة حسن",     "0547778899", "أنثى", "Basic Plan",  "30 $",  "1 شهر",  "2026-02-10");
            _dt.Rows.Add(5,  "عمر فاروق",    "0533334455", "ذكر",  "Pro Plan",    "50 $",  "3 شهر",  "2025-09-05");
            _dt.Rows.Add(6,  "ليلى أحمد",    "0522225566", "أنثى", "Basic Plan",  "30 $",  "1 شهر",  "2026-03-01");
            _dt.Rows.Add(7,  "يوسف كمال",    "0511116677", "ذكر",  "Annual Plan", "300 $", "1 سنة",  "2025-08-15");
            _dt.Rows.Add(8,  "فاطمة سعيد",   "0588889900", "أنثى", "Basic Plan",  "30 $",  "1 شهر",  "2026-04-01");
            _dt.Rows.Add(9,  "محمود عادل",   "0577771122", "ذكر",  "Pro Plan",    "50 $",  "3 شهر",  "2025-12-20");
            _dt.Rows.Add(10, "هند محمود",    "0566662233", "أنثى", "Annual Plan", "300 $", "1 سنة",  "2026-01-05");

            gridMembers.DataSource = _dt;
            gridMembers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (gridMembers.Columns.Contains("ID"))
                gridMembers.Columns["ID"].Visible = false;
        }

        // ═══════════════════════════════════════════
        //  EVENT WIRING
        // ═══════════════════════════════════════════
        private void WireEvents()
        {
            btnAddMember.Click   += BtnAddMember_Click;
            btnEdit.Click        += BtnEdit_Click;
            btnWhatsApp.Click    += BtnWhatsApp_Click;
            btnDelete.Click      += BtnDelete_Click;
            btnFormSave.Click    += BtnFormSave_Click;
            btnFormCancel.Click  += BtnFormCancel_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cmbFPlan.SelectedIndexChanged += CmbFPlan_SelectedIndexChanged;
        }

        // ── Auto-fetch price & duration when plan changes ──
        private void CmbFPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = cmbFPlan.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedName))
            {
                txtFPlanPrice.Text  = "";
                txtFPlanMonths.Text = "";
                return;
            }

            SubscriptionPlan plan = SubscriptionPlanCatalog.GetPlans()
                .FirstOrDefault(p => p.Name == selectedName);

            if (plan != null)
            {
                txtFPlanPrice.Text  = plan.Price.ToString("0.##") + " $";
                txtFPlanMonths.Text = plan.DurationValue + " " + plan.DurationUnit;
            }
            else
            {
                txtFPlanPrice.Text  = "";
                txtFPlanMonths.Text = "";
            }
        }

        // ═══════════════════════════════════════════
        //  SEARCH
        // ═══════════════════════════════════════════
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string filter = txtSearch.Text.Trim().Replace("'", "''");
            _dt.DefaultView.RowFilter = string.IsNullOrEmpty(filter)
                ? ""
                : $"الاسم LIKE '%{filter}%' OR الهاتف LIKE '%{filter}%'";
            UpdateMemberCount();
        }

        // ═══════════════════════════════════════════
        //  CREATE
        // ═══════════════════════════════════════════
        private void BtnAddMember_Click(object sender, EventArgs e)
        {
            _isEditing      = false;
            _editingRowIndex = -1;
            lblFormTitle.Text = "➕  إضافة عضو جديد";
            ClearForm();
            ShowForm();
        }

        // ═══════════════════════════════════════════
        //  UPDATE
        // ═══════════════════════════════════════════
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (gridMembers.SelectedRows.Count == 0)
            {
                MessageBox.Show("الرجاء تحديد عضو للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isEditing = true;
            DataGridViewRow row = gridMembers.SelectedRows[0];
            _editingRowIndex = row.Index;

            txtFName.Text  = row.Cells["الاسم"].Value?.ToString()  ?? "";
            txtFPhone.Text = row.Cells["الهاتف"].Value?.ToString() ?? "";

            string gender = row.Cells["الجنس"].Value?.ToString() ?? "ذكر";
            cmbFGender.SelectedItem = cmbFGender.Items.Contains(gender) ? gender : (object)0;

            string plan = row.Cells["الاشتراك"].Value?.ToString() ?? "";
            if (!cmbFPlan.Items.Contains(plan)) cmbFPlan.Items.Add(plan);
            cmbFPlan.SelectedItem = plan;

            lblFormTitle.Text = "✏️  تعديل بيانات العضو";
            ShowForm();
        }

        // ═══════════════════════════════════════════
        //  WHATSAPP (opens browser wa.me)
        // ═══════════════════════════════════════════
        private void BtnWhatsApp_Click(object sender, EventArgs e)
        {
            if (gridMembers.SelectedRows.Count == 0)
            {
                MessageBox.Show("الرجاء تحديد عضو لفتح واتساب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name  = gridMembers.SelectedRows[0].Cells["الاسم"].Value?.ToString() ?? "";
            string phone = gridMembers.SelectedRows[0].Cells["الهاتف"].Value?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("لا يوجد رقم هاتف لهذا العضو", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string msg = $"مرحباً {name}،\n\nنراسلك من Glory Gym.";
            WhatsAppWeb.OpenChat(phone, msg);
        }

        // ═══════════════════════════════════════════
        //  DELETE
        // ═══════════════════════════════════════════
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (gridMembers.SelectedRows.Count == 0)
            {
                MessageBox.Show("الرجاء تحديد عضو للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = gridMembers.SelectedRows[0].Cells["الاسم"].Value?.ToString() ?? "";
            if (MessageBox.Show("هل أنت متأكد من حذف العضو: " + name + "؟", "⚠️ تأكيد الحذف",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int rowIndex = gridMembers.SelectedRows[0].Index;
                DataRowView drv = gridMembers.Rows[rowIndex].DataBoundItem as DataRowView;
                if (drv != null) { drv.Row.Delete(); _dt.AcceptChanges(); }
                UpdateMemberCount();
                MessageBox.Show("تم حذف العضو بنجاح", "✅ تم الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ═══════════════════════════════════════════
        //  SAVE
        // ═══════════════════════════════════════════
        private void BtnFormSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFName.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم العضو", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string planName   = cmbFPlan.SelectedItem?.ToString() ?? "";
            string price      = txtFPlanPrice.Text;
            string duration   = txtFPlanMonths.Text;
            string gender     = cmbFGender.SelectedItem?.ToString() ?? "ذكر";

            if (_isEditing && _editingRowIndex >= 0 && _editingRowIndex < gridMembers.Rows.Count)
            {
                DataRowView drv = gridMembers.Rows[_editingRowIndex].DataBoundItem as DataRowView;
                if (drv != null)
                {
                    drv["الاسم"]     = txtFName.Text.Trim();
                    drv["الهاتف"]   = txtFPhone.Text.Trim();
                    drv["الجنس"]    = gender;
                    drv["الاشتراك"] = planName;
                    drv["السعر"]    = price;
                    drv["المدة"]    = duration;
                }
                MessageBox.Show("تم تحديث بيانات العضو بنجاح", "✅ تم التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                int newId = _dt.Rows.Count > 0 ? _dt.AsEnumerable().Max(r => r.Field<int>("ID")) + 1 : 1;
                _dt.Rows.Add(newId, txtFName.Text.Trim(), txtFPhone.Text.Trim(),
                    gender, planName, price, duration, DateTime.Now.ToString("yyyy-MM-dd"));
                MessageBox.Show("تمت إضافة العضو بنجاح", "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            HideForm();
            UpdateMemberCount();
        }

        private void BtnFormCancel_Click(object sender, EventArgs e) => HideForm();

        // ═══════════════════════════════════════════
        //  HELPERS
        // ═══════════════════════════════════════════
        private void ClearForm()
        {
            txtFName.Text  = "";
            txtFPhone.Text = "";
            if (cmbFGender.Items.Count > 0) cmbFGender.SelectedIndex = 0;
            LoadSubscriptionPlans();
            if (cmbFPlan.Items.Count > 0) cmbFPlan.SelectedIndex = 0;
            // price & duration filled by event handler above
        }

        private void ShowForm()
        {
            pnlForm.Location = new Point((ClientSize.Width - pnlForm.Width) / 2,
                                         (ClientSize.Height - pnlForm.Height) / 2);
            pnlForm.Visible = true;
            pnlForm.BringToFront();
            txtFName.Focus();
        }

        private void HideForm() => pnlForm.Visible = false;

        private void UpdateMemberCount()
            => lblMemberCount.Text = "إجمالي الأعضاء: " + _dt.DefaultView.Count;
    }
}
