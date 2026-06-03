using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class MembersForm : Form, IThemeAware
    {
        private DataTable _dt;
        private bool _isEditing = false;
        private int  _editingMemberId = -1;
        private readonly bool _startAddMode;

        // ── Gender filter (added programmatically) ──
        private Label _lblFilterGender;
        private ComboBox _cmbFilterGender;

        // ── Plan features view inside the add/edit overlay ──
        private Label _lblFormFeaturesTitle;
        private Label _lblFormFeatures;

        public MembersForm() : this(false) { }

        public MembersForm(bool startAddMode)
        {
            _startAddMode = startAddMode;
            InitializeComponent();
            ApplyBackgroundBranding();
            BuildExtraControls();
            LoadSubscriptionPlans();
            InitMembersTable();
            RebindMembersFromStore();
            WireEvents();
            ApplyTheme(ThemeManager.Current);
            UpdateMemberCount();

            if (_startAddMode)
                BtnAddMember_Click(this, EventArgs.Empty);
        }

        // ═══════════════════════════════════════════
        //  EXTRA UI: gender filter + plan-features view
        // ═══════════════════════════════════════════
        private void BuildExtraControls()
        {
            // Gender filter in the search bar (anchored to the right, left of the search box).
            _lblFilterGender = new Label
            {
                Text      = "الجنس:",
                Font      = new Font("Segoe UI", 12F),
                AutoSize  = true,
                Location  = new Point(300, 16),
                Anchor    = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent
            };
            _cmbFilterGender = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle     = FlatStyle.Flat,
                Font          = new Font("Segoe UI", 11F),
                RightToLeft   = RightToLeft.Yes,
                Location      = new Point(360, 12),
                Size          = new Size(150, 29),
                Anchor        = AnchorStyles.Top | AnchorStyles.Right
            };
            _cmbFilterGender.Items.AddRange(new object[] { "الكل", "ذكر", "أنثى" });
            _cmbFilterGender.SelectedIndex = 0;
            _cmbFilterGender.SelectedIndexChanged += (_, __) => ApplyFilters();

            pnlSearch.Controls.Add(_lblFilterGender);
            pnlSearch.Controls.Add(_cmbFilterGender);

            // Plan-features view inside the add/edit overlay.
            pnlForm.Size = new Size(500, 600);
            pnlForm.AutoScroll = true;

            _lblFormFeaturesTitle = new Label
            {
                Text      = "مميزات الاشتراك:",
                Font      = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize  = true,
                Location  = new Point(388, 360),
                BackColor = Color.Transparent
            };
            _lblFormFeatures = new Label
            {
                Text       = "—",
                Font       = new Font("Segoe UI", 10F),
                Location   = new Point(35, 388),
                Size       = new Size(435, 150),
                TextAlign  = ContentAlignment.TopRight,
                BackColor  = Color.Transparent,
                ForeColor  = FigmaPalette.GreenBtn
            };

            pnlForm.Controls.Add(_lblFormFeaturesTitle);
            pnlForm.Controls.Add(_lblFormFeatures);

            // Push the action buttons below the features list.
            btnFormSave.Location   = new Point(245, 548);
            btnFormCancel.Location = new Point(35, 548);
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

            if (_lblFilterGender != null) _lblFilterGender.ForeColor = s.TextMuted;
            if (_cmbFilterGender != null)
            {
                _cmbFilterGender.BackColor = s.InputBackground;
                _cmbFilterGender.ForeColor = s.InputForeground;
            }
            if (_lblFormFeaturesTitle != null) _lblFormFeaturesTitle.ForeColor = s.TextPrimary;

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
            GunaUi.ApplyBrandGradient(btnAddMember);
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

            // Figma: green prices + blue/pink gender pills. Cell-level styling wins
            // over the theme's row defaults, so colours survive ApplyTheme().
            gridMembers.CellFormatting -= GridMembers_CellFormatting;
            gridMembers.CellFormatting += GridMembers_CellFormatting;
        }

        private void GridMembers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string col = gridMembers.Columns[e.ColumnIndex].Name;
            if (col == "السعر")
            {
                e.CellStyle.ForeColor = FigmaPalette.GreenBtn;
                e.CellStyle.Font = new Font(gridMembers.Font, FontStyle.Bold);
            }
            else if (col == "الجنس")
            {
                string v = e.Value?.ToString() ?? "";
                if (v.Contains("ذكر"))
                    e.CellStyle.ForeColor = FigmaPalette.BadgeMaleText;
                else if (v.Contains("أنثى") || v.Contains("انثى"))
                    e.CellStyle.ForeColor = FigmaPalette.BadgeFemaleText;
            }
        }

        private void RebindMembersFromStore()
        {
            _dt.Rows.Clear();
            var sorted = new List<MemberRecord>(GymDataStore.Data.Members);
            sorted.Sort((a, b) => a.Id.CompareTo(b.Id));
            foreach (var m in sorted)
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
            btnPrint.Click       += BtnPrint_Click;
            btnFormSave.Click    += BtnFormSave_Click;
            btnFormCancel.Click  += BtnFormCancel_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cmbFPlan.SelectedIndexChanged += CmbFPlan_SelectedIndexChanged;
            this.Resize += (_, __) => { if (pnlForm.Visible) CenterForm(); };
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

            SubscriptionPlan plan = null;
            foreach (var p in SubscriptionPlanCatalog.GetPlans())
                if (p.Name == selectedName) { plan = p; break; }

            if (plan != null)
            {
                txtFPlanPrice.Text  = plan.Price.ToString("0.##") + " د.ل";
                txtFPlanMonths.Text = plan.DurationValue + " " + plan.DurationUnit;
            }
            else
            {
                txtFPlanPrice.Text  = "";
                txtFPlanMonths.Text = "";
            }

            UpdateFeaturesView(plan);
        }

        private void UpdateFeaturesView(SubscriptionPlan plan)
        {
            if (_lblFormFeatures == null) return;
            if (plan == null || plan.Features == null || plan.Features.Count == 0)
            {
                _lblFormFeatures.Text = "لا توجد مميزات لهذا الاشتراك";
                _lblFormFeatures.ForeColor = ThemeManager.Current.TextMuted;
                return;
            }

            _lblFormFeatures.Text = string.Join("\n", plan.Features.ConvertAll(f => "✓  " + f));
            _lblFormFeatures.ForeColor = FigmaPalette.GreenBtn;
        }

        // ═══════════════════════════════════════════
        //  SEARCH
        // ═══════════════════════════════════════════
        private void TxtSearch_TextChanged(object sender, EventArgs e) => ApplyFilters();

        // Combined search + gender filter applied to the grid's DefaultView.
        private void ApplyFilters()
        {
            if (_dt == null) return;

            var clauses = new System.Collections.Generic.List<string>();

            string filter = txtSearch.Text.Trim().Replace("'", "''");
            if (!string.IsNullOrEmpty(filter))
                clauses.Add($"(الاسم LIKE '%{filter}%' OR الهاتف LIKE '%{filter}%')");

            string gender = _cmbFilterGender?.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(gender) && gender != "الكل")
                clauses.Add($"الجنس = '{gender.Replace("'", "''")}'");

            _dt.DefaultView.RowFilter = string.Join(" AND ", clauses);
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
                GunaUi.Show("الرجاء تحديد عضو للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                GunaUi.Show("الرجاء تحديد عضو لفتح واتساب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name  = gridMembers.SelectedRows[0].Cells["الاسم"].Value?.ToString() ?? "";
            string phone = gridMembers.SelectedRows[0].Cells["الهاتف"].Value?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(phone))
            {
                GunaUi.Show("لا يوجد رقم هاتف لهذا العضو", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string msg = $"مرحباً {name}،\n\nنراسلك من Glory Gym.";
            WhatsAppWeb.OpenChat(phone, msg);
        }

        // ═══════════════════════════════════════════
        //  PRINT — open the GymMembers.rpt report file directly
        // ═══════════════════════════════════════════
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            ReportLauncher.OpenEmbeddedReport("GymMembers.rpt", "الأعضاء");
        }

        // ═══════════════════════════════════════════
        //  DELETE
        // ═══════════════════════════════════════════
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (gridMembers.SelectedRows.Count == 0)
            {
                GunaUi.Show("الرجاء تحديد عضو للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = gridMembers.SelectedRows[0].Cells["الاسم"].Value?.ToString() ?? "";
            if (GunaUi.Show("هل أنت متأكد من حذف العضو: " + name + "؟", "⚠️ تأكيد الحذف",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(gridMembers.SelectedRows[0].Cells["ID"].Value);
                GymDataStore.Data.Members.RemoveAll(x => x.Id == id);
                GymDataStore.Save();
                RebindMembersFromStore();
                GunaUi.Show("تم حذف العضو بنجاح", "✅ تم الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ═══════════════════════════════════════════
        //  SAVE
        // ═══════════════════════════════════════════
        private void BtnFormSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFName.Text))
            {
                GunaUi.Show("الرجاء إدخال اسم العضو", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string planName   = cmbFPlan.SelectedItem?.ToString() ?? "";
            string price      = txtFPlanPrice.Text;
            string duration   = txtFPlanMonths.Text;
            string gender     = cmbFGender.SelectedItem?.ToString() ?? "ذكر";

            // Resolve the real FK (PlanId) from the chosen plan name.
            int? planId = null;
            foreach (var p in SubscriptionPlanCatalog.GetPlans())
                if (p.Name == planName) { planId = p.Id; break; }

            if (_isEditing && _editingMemberId > 0)
            {
                MemberRecord mem = null;
                foreach (var x in GymDataStore.Data.Members)
                    if (x.Id == _editingMemberId) { mem = x; break; }
                if (mem != null)
                {
                    mem.FullName   = txtFName.Text.Trim();
                    mem.Phone      = txtFPhone.Text.Trim();
                    mem.Gender     = gender;
                    mem.PlanId     = planId;
                    mem.PlanName   = planName;
                    mem.PriceText  = price;
                    mem.DurationText = duration;
                }
                GymDataStore.Save();
                GunaUi.Show("تم تحديث بيانات العضو بنجاح", "✅ تم التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                GymDataStore.Data.Members.Add(new MemberRecord
                {
                    Id            = GymDataStore.NextMemberId(),
                    FullName      = txtFName.Text.Trim(),
                    Phone         = txtFPhone.Text.Trim(),
                    Gender        = gender,
                    PlanId        = planId,
                    PlanName      = planName,
                    PriceText     = price,
                    DurationText  = duration,
                    JoinDate      = DateTime.Now.ToString("yyyy-MM-dd")
                });
                GymDataStore.Save();
                GunaUi.Show("تمت إضافة العضو بنجاح", "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            CenterForm();
            pnlForm.Visible = true;
            pnlForm.BringToFront();
            txtFName.Focus();
        }

        private void CenterForm()
        {
            int w = Math.Min(pnlForm.Width, ClientSize.Width);
            int h = Math.Min(pnlForm.Height, ClientSize.Height);
            pnlForm.Location = new Point(Math.Max(0, (ClientSize.Width - w) / 2),
                                         Math.Max(0, (ClientSize.Height - h) / 2));
        }

        private void HideForm() => pnlForm.Visible = false;

        private void UpdateMemberCount()
            => lblMemberCount.Text = "إجمالي الأعضاء: " + _dt.DefaultView.Count;
    }
}
