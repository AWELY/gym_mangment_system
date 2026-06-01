using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
            _cardHost.SendToBack();
        }

        private void BuildUserCards()
        {
            if (_cardHost == null) return;
            UiColorScheme s = ThemeManager.Current;
            _cardHost.SuspendLayout();
            foreach (Control c in _cardHost.Controls) c.Dispose();
            _cardHost.Controls.Clear();

            foreach (var u in GymDataStore.Data.Users.OrderBy(x => x.Id))
                _cardHost.Controls.Add(BuildUserCard(u, s));

            _cardHost.ResumeLayout();
        }

        private Panel BuildUserCard(UserDirectoryEntry u, UiColorScheme s)
        {
            int w = 330, h = 196, pad = 18, innerW = w - pad * 2;
            bool isAdmin = u.Username.Equals("admin", StringComparison.OrdinalIgnoreCase);
            string roleText = u.Role == AppSession.UserRole.Admin ? "مدير" : "موظف استقبال";

            Panel card = new Panel { Size = new Size(w, h), Margin = new Padding(10), BackColor = s.Card };
            DashboardForm.StyleAsRoundedCard(card, s.BorderSubtle, 14);

            string title = string.IsNullOrWhiteSpace(u.FullName) ? u.Username : u.FullName;
            Label name = new Label { Text = title, Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = s.TextPrimary, Location = new Point(pad, 16), Size = new Size(innerW, 28), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label role = new Label { Text = "الصلاحية: " + roleText, Font = new Font("Segoe UI", 9.5F), ForeColor = FigmaPalette.Primary, Location = new Point(pad, 44), Size = new Size(innerW, 20), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label user = new Label { Text = "اسم المستخدم: " + u.Username, Font = new Font("Segoe UI", 10F), ForeColor = s.TextMuted, Location = new Point(pad, 70), Size = new Size(innerW, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label pass = new Label { Text = "كلمة المرور: " + u.Password, Font = new Font("Segoe UI", 10F), ForeColor = s.TextMuted, Location = new Point(pad, 96), Size = new Size(innerW, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };

            int btnW = (innerW - 10) / 2, btnY = 148, btnH = 34;
            Button del = TrainersForm.MakeCardButton("🗑  حذف", FigmaPalette.Red, new Point(pad, btnY), new Size(btnW, btnH));
            Button edit = TrainersForm.MakeCardButton("✎  تعديل", FigmaPalette.BlueBtn, new Point(pad + btnW + 10, btnY), new Size(btnW, btnH));
            if (isAdmin)
            {
                del.Enabled = false;
                del.BackColor = s.SecondaryButton;
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
                MessageBox.Show("لا يمكن حذف حساب المدير الافتراضي", "محظور", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show("هل أنت متأكد من حذف المستخدم: " + username + "؟", "⚠️ تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            if (UserDirectory.Remove(id))
            {
                UserDirectory.FillDataTable(_dt);
                UpdateCount();
            }
        }

        private void LoadUserIntoForm(int id)
        {
            var u = GymDataStore.Data.Users.FirstOrDefault(x => x.Id == id);
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

            pnlForm.BackColor = s.Card;
            lblFormTitle.ForeColor = s.TextPrimary;
            lblFFullName.ForeColor = lblFUsername.ForeColor = lblFPassword.ForeColor = lblFRole.ForeColor = s.TextMuted;
            txtFFullName.BackColor = txtFUsername.BackColor = txtFPassword.BackColor = cmbFRole.BackColor = s.InputBackground;
            txtFFullName.ForeColor = txtFUsername.ForeColor = txtFPassword.ForeColor = cmbFRole.ForeColor = s.InputForeground;
            btnFormCancel.BackColor = s.SecondaryButton;
            btnFormCancel.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.LightGray;
            btnFormCancel.FlatAppearance.MouseOverBackColor = s.SecondaryButtonHover;

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
            if (gridUsers.SelectedRows.Count == 0) { MessageBox.Show("الرجاء تحديد مستخدم للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
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
            if (gridUsers.SelectedRows.Count == 0) { MessageBox.Show("الرجاء تحديد مستخدم للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string name = gridUsers.SelectedRows[0].Cells["اسم المستخدم"].Value?.ToString() ?? "";
            if (name.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("لا يمكن حذف حساب المدير الافتراضي", "محظور", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show("هل أنت متأكد من حذف المستخدم: " + name + "؟", "⚠️ تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
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
            if (string.IsNullOrWhiteSpace(txtFUsername.Text)) { MessageBox.Show("الرجاء إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cmbFRole.SelectedIndex < 0) { MessageBox.Show("الرجاء اختيار الصلاحية", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var role = UserDirectoryEntry.RoleFromDisplay(cmbFRole.SelectedItem?.ToString() ?? "");
            string u = txtFUsername.Text.Trim();

            if (_isEditing && _editingRowIndex >= 0 && _editingRowIndex < gridUsers.Rows.Count)
            {
                var drv = gridUsers.Rows[_editingRowIndex].DataBoundItem as DataRowView;
                if (drv == null) return;
                int id = Convert.ToInt32(drv["ID"]);
                if (UserDirectory.UsernameExists(u, id))
                {
                    MessageBox.Show("اسم المستخدم مستخدم بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string pwdKeep = string.IsNullOrWhiteSpace(txtFPassword.Text) ? null : txtFPassword.Text;
                UserDirectory.Update(id, txtFFullName.Text.Trim(), u, pwdKeep, role);
                UserDirectory.FillDataTable(_dt);
                MessageBox.Show("تم تحديث بيانات المستخدم", "✅ تم التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtFPassword.Text))
                {
                    MessageBox.Show("الرجاء إدخال كلمة مرور للمستخدم الجديد", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (UserDirectory.UsernameExists(u, null))
                {
                    MessageBox.Show("اسم المستخدم مستخدم بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UserDirectory.Add(txtFFullName.Text.Trim(), u, txtFPassword.Text, role);
                UserDirectory.FillDataTable(_dt);
                MessageBox.Show("تمت إضافة المستخدم بنجاح", "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            pnlForm.Location = new Point((ClientSize.Width - pnlForm.Width) / 2, (ClientSize.Height - pnlForm.Height) / 2);
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
