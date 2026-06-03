using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace gym_mangment_system
{
    public partial class UsersForm : Form, IThemeAware
    {
        private DataTable _dt;
        private bool _isEditing      = false;
        private int  _editingRowIndex = -1;
        private FlowLayoutPanel _cardHost;

        public UsersForm()
        {
            InitializeComponent();
            LoadMockData();
            BuildCardHost();
            WireEvents();
            ApplyTheme(ThemeManager.Current);
            UpdateCount();
            this.Resize += (_, __) => { if (pnlForm.Visible) RecenterForm(); };
        }

        private void RecenterForm()
        {
            int w = Math.Min(pnlForm.Width, ClientSize.Width);
            int h = Math.Min(pnlForm.Height, ClientSize.Height);
            pnlForm.Location = new Point(Math.Max(0, (ClientSize.Width - w) / 2),
                                         Math.Max(0, (ClientSize.Height - h) / 2));
        }

        // ── Figma card layout (replaces the grid) ──────────
        private void BuildCardHost()
        {
            gridUsers.Visible   = false;
            pnlActions.Visible  = false;

            _cardHost = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                AutoScroll    = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents  = true,
                Padding       = new Padding(18, 10, 18, 18),
                BackColor     = ThemeManager.Current.ContentHost
            };
            Controls.Add(_cardHost);
            _cardHost.BringToFront();
        }

        private void BuildUserCards()
        {
            if (_cardHost == null) return;
            UiColorScheme s = ThemeManager.Current;
            _cardHost.SuspendLayout();
            foreach (Control c in _cardHost.Controls) c.Dispose();
            _cardHost.Controls.Clear();

            var sorted = new List<UserDirectoryEntry>(GymDataStore.Data.Users);
            sorted.Sort((a, b) => a.Id.CompareTo(b.Id));
            foreach (var u in sorted)
                _cardHost.Controls.Add(BuildUserCard(u, s));

            _cardHost.ResumeLayout();
        }

        private Guna2Panel BuildUserCard(UserDirectoryEntry u, UiColorScheme s)
        {
            int w = 330, h = 196, pad = 18, innerW = w - pad * 2;
            bool isAdmin = u.Username.Equals("admin", StringComparison.OrdinalIgnoreCase);
            string roleText = u.Role == AppSession.UserRole.Admin ? "مدير" : "موظف استقبال";

            Guna2Panel card = GunaUi.Card(w, h, s.Card, s.BorderSubtle);

            string title = string.IsNullOrWhiteSpace(u.FullName) ? u.Username : u.FullName;
            Label name = new Label { Text = title, Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = s.TextPrimary, Location = new Point(pad, 16), Size = new Size(innerW, 28), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Color roleColor = u.Role == AppSession.UserRole.Admin ? FigmaPalette.Primary : FigmaPalette.BadgeMaleText;
            Label role = new Label { Text = "الصلاحية: " + roleText, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = roleColor, Location = new Point(pad, 44), Size = new Size(innerW, 20), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label user = new Label { Text = "اسم المستخدم: " + u.Username, Font = new Font("Segoe UI", 10F), ForeColor = s.TextMuted, Location = new Point(pad, 70), Size = new Size(innerW, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label pass = new Label { Text = "كلمة المرور: " + u.Password, Font = new Font("Segoe UI", 10F), ForeColor = s.TextMuted, Location = new Point(pad, 96), Size = new Size(innerW, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };

            int btnW = (innerW - 10) / 2, btnY = 148, btnH = 34;
            Guna2Button del = GunaUi.Button("🗑  حذف", FigmaPalette.Red, new Point(pad, btnY), new Size(btnW, btnH));
            Guna2Button edit = GunaUi.Button("✎  تعديل", FigmaPalette.BlueBtn, new Point(pad + btnW + 10, btnY), new Size(btnW, btnH));
            if (isAdmin)
            {
                del.Enabled = false;
                del.FillColor = s.SecondaryButton;
                del.ForeColor = s.TextMuted;
            }
            int id = u.Id;
            string uname = u.Username;
            del.Click += (_, __) => DeleteUser(id, uname);
            edit.Click += (_, __) => LoadUserIntoForm(id);

            card.Controls.Add(name); card.Controls.Add(role); card.Controls.Add(user);
            card.Controls.Add(pass); card.Controls.Add(del); card.Controls.Add(edit);
            return card;
        }

        private void DeleteUser(int id, string username)
        {
            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                GunaUi.Show("لا يمكن حذف حساب المدير الافتراضي", "محظور", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (GunaUi.Show("هل أنت متأكد من حذف المستخدم: " + username + "؟", "⚠️ تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            if (UserDirectory.Remove(id))
            {
                UserDirectory.FillDataTable(_dt);
                UpdateCount();
            }
        }

        private void LoadUserIntoForm(int id)
        {
            UserDirectoryEntry u = null;
            foreach (var x in GymDataStore.Data.Users)
                if (x.Id == id) { u = x; break; }
            if (u == null) return;
            _isEditing = true;
            var rowIdx = -1;
            for (int i = 0; i < gridUsers.Rows.Count; i++)
            {
                if (gridUsers.Rows[i].DataBoundItem is DataRowView drv && Convert.ToInt32(drv["ID"]) == id) { rowIdx = i; break; }
            }
            _editingRowIndex = rowIdx;
            txtFFullName.Text = u.FullName;
            txtFUsername.Text = u.Username;
            txtFPassword.Text = "";
            cmbFRole.SelectedIndex = u.Role == AppSession.UserRole.Admin ? 0 : 1;
            lblFormTitle.Text = "✏️  تعديل بيانات المستخدم";
            ShowForm();
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            pnlActions.BackColor = s.ContentHost;
            lblTitle.ForeColor = s.TextPrimary;
            ThemeManager.StyleDataGridView(gridUsers, s);
            lblCount.ForeColor = s.TextMuted;

            pnlForm.FillColor = s.Card;
            pnlForm.BorderColor = s.BorderSubtle;
            lblFormTitle.ForeColor = s.TextPrimary;
            lblFFullName.ForeColor = lblFUsername.ForeColor = lblFPassword.ForeColor = lblFRole.ForeColor = s.TextMuted;
            txtFFullName.BackColor = txtFUsername.BackColor = txtFPassword.BackColor = cmbFRole.BackColor = s.InputBackground;
            txtFFullName.ForeColor = txtFUsername.ForeColor = txtFPassword.ForeColor = cmbFRole.ForeColor = s.InputForeground;
            btnFormSave.FillColor = FigmaPalette.GreenBtn;
            btnFormCancel.FillColor = s.SecondaryButton;
            btnFormCancel.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.LightGray;

            if (_cardHost != null)
            {
                _cardHost.BackColor = s.ContentHost;
                BuildUserCards();
            }
        }

        private void LoadMockData()
        {
            _dt = new DataTable();
            _dt.Columns.Add("ID",        typeof(int));
            _dt.Columns.Add("الاسم الكامل", typeof(string));
            _dt.Columns.Add("اسم المستخدم", typeof(string));
            _dt.Columns.Add("الصلاحية",   typeof(string));

            UserDirectory.FillDataTable(_dt);

            gridUsers.DataSource = _dt;
            gridUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (gridUsers.Columns.Contains("ID")) gridUsers.Columns["ID"].Visible = false;
        }

        private void WireEvents()
        {
            btnAddUser.Click    += (s, e) => { _isEditing = false; _editingRowIndex = -1; lblFormTitle.Text = "➕  إضافة مستخدم جديد"; ClearForm(); ShowForm(); };
            btnEditUser.Click   += BtnEditUser_Click;
            btnDeleteUser.Click += BtnDeleteUser_Click;
            btnFormSave.Click   += BtnFormSave_Click;
            btnFormCancel.Click += (s, e) => HideForm();
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            if (gridUsers.SelectedRows.Count == 0) { GunaUi.Show("الرجاء تحديد مستخدم للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            _isEditing = true;
            var row = gridUsers.SelectedRows[0];
            _editingRowIndex   = row.Index;
            txtFFullName.Text  = row.Cells["الاسم الكامل"].Value?.ToString() ?? "";
            txtFUsername.Text  = row.Cells["اسم المستخدم"].Value?.ToString() ?? "";
            txtFPassword.Text  = "";
            string role = row.Cells["الصلاحية"].Value?.ToString() ?? "";
            if (cmbFRole.Items.Contains(role))
                cmbFRole.SelectedItem = role;
            else
                cmbFRole.SelectedIndex = UserDirectoryEntry.RoleFromDisplay(role) == AppSession.UserRole.Admin ? 0 : 1;
            lblFormTitle.Text = "✏️  تعديل بيانات المستخدم";
            ShowForm();
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (gridUsers.SelectedRows.Count == 0) { GunaUi.Show("الرجاء تحديد مستخدم للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string name = gridUsers.SelectedRows[0].Cells["اسم المستخدم"].Value?.ToString() ?? "";
            if (name.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                GunaUi.Show("لا يمكن حذف حساب المدير الافتراضي", "محظور", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (GunaUi.Show("هل أنت متأكد من حذف المستخدم: " + name + "؟", "⚠️ تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(gridUsers.SelectedRows[0].Cells["ID"].Value);
                if (UserDirectory.Remove(id))
                {
                    UserDirectory.FillDataTable(_dt);
                    UpdateCount();
                }
            }
        }

        private void BtnFormSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFUsername.Text)) { GunaUi.Show("الرجاء إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cmbFRole.SelectedIndex < 0) { GunaUi.Show("الرجاء اختيار الصلاحية", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var role = UserDirectoryEntry.RoleFromDisplay(cmbFRole.SelectedItem?.ToString() ?? "");
            string u = txtFUsername.Text.Trim();

            if (_isEditing && _editingRowIndex >= 0 && _editingRowIndex < gridUsers.Rows.Count)
            {
                var drv = gridUsers.Rows[_editingRowIndex].DataBoundItem as DataRowView;
                if (drv == null) return;
                int id = Convert.ToInt32(drv["ID"]);
                if (UserDirectory.UsernameExists(u, id))
                {
                    GunaUi.Show("اسم المستخدم مستخدم بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string pwdKeep = string.IsNullOrWhiteSpace(txtFPassword.Text) ? null : txtFPassword.Text;
                UserDirectory.Update(id, txtFFullName.Text.Trim(), u, pwdKeep, role);
                UserDirectory.FillDataTable(_dt);
                GunaUi.Show("تم تحديث بيانات المستخدم", "✅ تم التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtFPassword.Text))
                {
                    GunaUi.Show("الرجاء إدخال كلمة مرور للمستخدم الجديد", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (UserDirectory.UsernameExists(u, null))
                {
                    GunaUi.Show("اسم المستخدم مستخدم بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UserDirectory.Add(txtFFullName.Text.Trim(), u, txtFPassword.Text, role);
                UserDirectory.FillDataTable(_dt);
                GunaUi.Show("تمت إضافة المستخدم بنجاح", "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            HideForm(); UpdateCount();
        }

        private void ClearForm()
        {
            txtFFullName.Text = ""; txtFUsername.Text = ""; txtFPassword.Text = "";
            if (cmbFRole.Items.Count > 0) cmbFRole.SelectedIndex = 0;
        }

        private void ShowForm()
        {
            RecenterForm();
            pnlForm.Visible = true; pnlForm.BringToFront(); txtFFullName.Focus();
        }

        private void HideForm() => pnlForm.Visible = false;

        private void UpdateCount()
        {
            lblCount.Text = "المستخدمون: " + _dt.DefaultView.Count;
            BuildUserCards();
        }
    }
}
