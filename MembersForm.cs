using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class MembersForm : Form, IThemeAware
    {
        private DataTable _dt;
        private bool _isEditing = false;
        private int  _editingMemberId = -1;
        private readonly bool _startAddMode;

        public MembersForm() : this(false) { }

        public MembersForm(bool startAddMode)
        {
            _startAddMode = startAddMode;
            InitializeComponent();
            ApplyBackgroundBranding();
            LoadSubscriptionPlans();
            InitMembersTable();
            RebindMembersFromStore();
            WireEvents();
            ApplyTheme(ThemeManager.Current);
            UpdateMemberCount();

            if (_startAddMode)
                BtnAddMember_Click(this, EventArgs.Empty);
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            pnlSearch.BackColor = s.ContentHost;
            pnlActions.BackColor = s.ContentHost;
            lblTitle.ForeColor = s.TextPrimary;
            lblSearch.ForeColor = s.TextMuted;
            txtSearch.BackColor = s.InputBackground;
            txtSearch.ForeColor = s.InputForeground;
            ThemeManager.StyleDataGridView(gridMembers, s);
            lblMemberCount.ForeColor = s.TextMuted;

            pnlForm.FillColor = s.Card;
            pnlForm.BorderColor = s.BorderSubtle;
            lblFormTitle.ForeColor = s.TextPrimary;
            Color labelMuted = s.TextMuted;
            lblFName.ForeColor = labelMuted;
            lblFPhone.ForeColor = labelMuted;
            lblFGender.ForeColor = labelMuted;
            lblFPlan.ForeColor = labelMuted;
            lblFPlanPrice.ForeColor = labelMuted;
            lblFPlanMonths.ForeColor = labelMuted;

            txtFName.BackColor = s.InputBackground;
            txtFName.ForeColor = s.InputForeground;
            txtFPhone.BackColor = s.InputBackground;
            txtFPhone.ForeColor = s.InputForeground;
            cmbFGender.BackColor = s.InputBackground;
            cmbFGender.ForeColor = s.InputForeground;
            cmbFPlan.BackColor = s.InputBackground;
            cmbFPlan.ForeColor = s.InputForeground;

            txtFPlanPrice.BackColor = s.PanelElevated;
            txtFPlanMonths.BackColor = s.PanelElevated;

            btnFormCancel.FillColor = s.SecondaryButton;
            btnFormCancel.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.LightGray;
            btnFormSave.FillColor = FigmaPalette.GreenBtn;
            btnAddMember.FillColor = FigmaPalette.Primary;
        }

        private void ApplyBackgroundBranding()
        {
            float op = ThemeManager.BrandingOpacity();
            Image faded = ImageAssets.TryLoadToughBackground("members", op);
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

        private void LoadSubscriptionPlans()
        {
            cmbFPlan.Items.Clear();
            foreach (SubscriptionPlan plan in SubscriptionPlanCatalog.GetPlans())
                cmbFPlan.Items.Add(plan.Name);

            if (cmbFPlan.Items.Count == 0)
                cmbFPlan.Items.Add("بدون اشتراك");
        }

        private void InitMembersTable()
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

            gridMembers.DataSource = _dt;
            gridMembers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (gridMembers.Columns.Contains("ID"))
                gridMembers.Columns["ID"].Visible = false;
        }

        private void RebindMembersFromStore()
        {
            _dt.Rows.Clear();
            foreach (var m in GymDataStore.Data.Members.OrderBy(x => x.Id))
            {
                _dt.Rows.Add(m.Id, m.FullName, m.Phone, m.Gender, m.PlanName, m.PriceText, m.DurationText, m.JoinDate);
            }
            _dt.AcceptChanges();
            UpdateMemberCount();
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
            _isEditing       = false;
            _editingMemberId = -1;
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
            _editingMemberId = Convert.ToInt32(row.Cells["ID"].Value);

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
                int id = Convert.ToInt32(gridMembers.SelectedRows[0].Cells["ID"].Value);
                GymDataStore.Data.Members.RemoveAll(x => x.Id == id);
                GymDataStore.Save();
                RebindMembersFromStore();
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

            if (_isEditing && _editingMemberId > 0)
            {
                var mem = GymDataStore.Data.Members.FirstOrDefault(x => x.Id == _editingMemberId);
                if (mem != null)
                {
                    mem.FullName   = txtFName.Text.Trim();
                    mem.Phone      = txtFPhone.Text.Trim();
                    mem.Gender     = gender;
                    mem.PlanName   = planName;
                    mem.PriceText  = price;
                    mem.DurationText = duration;
                }
                GymDataStore.Save();
                MessageBox.Show("تم تحديث بيانات العضو بنجاح", "✅ تم التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                GymDataStore.Data.Members.Add(new MemberRecord
                {
                    Id            = GymDataStore.NextMemberId(),
                    FullName      = txtFName.Text.Trim(),
                    Phone         = txtFPhone.Text.Trim(),
                    Gender        = gender,
                    PlanName      = planName,
                    PriceText     = price,
                    DurationText  = duration,
                    JoinDate      = DateTime.Now.ToString("yyyy-MM-dd")
                });
                GymDataStore.Save();
                MessageBox.Show("تمت إضافة العضو بنجاح", "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            HideForm();
            RebindMembersFromStore();
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
