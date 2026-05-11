using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class UsersForm : Form
    {
        private DataTable _dt;
        private bool _isEditing      = false;
        private int  _editingRowIndex = -1;

        public UsersForm()
        {
            InitializeComponent();
            LoadMockData();
            WireEvents();
            UpdateCount();
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

        private void UpdateCount() => lblCount.Text = "المستخدمون: " + _dt.DefaultView.Count;
    }
}
