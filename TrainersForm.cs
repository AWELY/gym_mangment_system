using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class TrainersForm : Form
    {
        private DataTable _dt;
        private bool _isEditing      = false;
        private int  _editingRowIndex = -1;

        public TrainersForm()
        {
            InitializeComponent();
            LoadMockData();
            WireEvents();
            UpdateCount();
        }

        private void LoadMockData()
        {
            _dt = new DataTable();
            _dt.Columns.Add("ID",            typeof(int));
            _dt.Columns.Add("الاسم",         typeof(string));
            _dt.Columns.Add("الهاتف",        typeof(string));
            _dt.Columns.Add("التخصص",        typeof(string));
            _dt.Columns.Add("الراتب ($)",    typeof(decimal));
            _dt.Columns.Add("تاريخ التعيين", typeof(string));

            _dt.Rows.Add(1, "محمد السالم",    "0501111222", "رفع أثقال",    2500m, "2023-01-10");
            _dt.Rows.Add(2, "أنس العتيبي",    "0522223333", "كروس فيت",     2200m, "2023-03-15");
            _dt.Rows.Add(3, "ريم الزهراني",   "0533334444", "يوغا ولياقة",  2000m, "2023-06-01");
            _dt.Rows.Add(4, "فيصل الحربي",    "0544445555", "ملاكمة",       2800m, "2022-11-20");
            _dt.Rows.Add(5, "نورا الشمري",    "0555556666", "تمارين نسائية",1900m, "2024-01-05");

            gridTrainers.DataSource = _dt;
            gridTrainers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (gridTrainers.Columns.Contains("ID")) gridTrainers.Columns["ID"].Visible = false;
        }

        private void WireEvents()
        {
            btnAddTrainer.Click  += (s, e) => { _isEditing = false; _editingRowIndex = -1; lblFormTitle.Text = "➕  إضافة مدرب جديد"; ClearForm(); ShowForm(); };
            btnEdit.Click        += BtnEdit_Click;
            btnDelete.Click      += BtnDelete_Click;
            btnFormSave.Click    += BtnFormSave_Click;
            btnFormCancel.Click  += (s, e) => HideForm();
            txtSearch.TextChanged += (s, e) =>
            {
                string f = txtSearch.Text.Trim().Replace("'", "''");
                _dt.DefaultView.RowFilter = string.IsNullOrEmpty(f) ? "" : $"الاسم LIKE '%{f}%' OR الهاتف LIKE '%{f}%' OR التخصص LIKE '%{f}%'";
                UpdateCount();
            };
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (gridTrainers.SelectedRows.Count == 0) { MessageBox.Show("الرجاء تحديد مدرب للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            _isEditing = true;
            var row = gridTrainers.SelectedRows[0];
            _editingRowIndex = row.Index;
            txtFName.Text      = row.Cells["الاسم"].Value?.ToString() ?? "";
            txtFPhone.Text     = row.Cells["الهاتف"].Value?.ToString() ?? "";
            txtFSpecialty.Text = row.Cells["التخصص"].Value?.ToString() ?? "";
            object salObj = row.Cells["الراتب ($)"].Value;
            decimal sal = salObj == null || salObj == DBNull.Value ? 0m : Convert.ToDecimal(salObj);
            numFSalary.Value = Math.Max(numFSalary.Minimum, Math.Min(numFSalary.Maximum, sal));
            DateTime d; if (!DateTime.TryParse(row.Cells["تاريخ التعيين"].Value?.ToString(), out d)) d = DateTime.Now;
            dtpFJoinDate.Value = d;
            lblFormTitle.Text = "✏️  تعديل بيانات المدرب";
            ShowForm();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (gridTrainers.SelectedRows.Count == 0) { MessageBox.Show("الرجاء تحديد مدرب للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string name = gridTrainers.SelectedRows[0].Cells["الاسم"].Value?.ToString() ?? "";
            if (MessageBox.Show("هل أنت متأكد من حذف المدرب: " + name + "؟", "⚠️ تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int idx = gridTrainers.SelectedRows[0].Index;
                var drv = gridTrainers.Rows[idx].DataBoundItem as DataRowView;
                if (drv != null) { drv.Row.Delete(); _dt.AcceptChanges(); }
                UpdateCount();
                MessageBox.Show("تم حذف المدرب بنجاح", "✅ تم الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnFormSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFName.Text)) { MessageBox.Show("الرجاء إدخال اسم المدرب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (_isEditing && _editingRowIndex >= 0 && _editingRowIndex < gridTrainers.Rows.Count)
            {
                var drv = gridTrainers.Rows[_editingRowIndex].DataBoundItem as DataRowView;
                if (drv != null)
                {
                    drv["الاسم"]         = txtFName.Text.Trim();
                    drv["الهاتف"]        = txtFPhone.Text.Trim();
                    drv["التخصص"]        = txtFSpecialty.Text.Trim();
                    drv["الراتب ($)"]    = numFSalary.Value;
                    drv["تاريخ التعيين"] = dtpFJoinDate.Value.ToString("yyyy-MM-dd");
                }
                MessageBox.Show("تم تحديث بيانات المدرب", "✅ تم التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                int newId = _dt.Rows.Count > 0 ? _dt.AsEnumerable().Max(r => r.Field<int>("ID")) + 1 : 1;
                _dt.Rows.Add(newId, txtFName.Text.Trim(), txtFPhone.Text.Trim(), txtFSpecialty.Text.Trim(),
                    numFSalary.Value, dtpFJoinDate.Value.ToString("yyyy-MM-dd"));
                MessageBox.Show("تمت إضافة المدرب بنجاح", "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            HideForm(); UpdateCount();
        }

        private void ClearForm()
        {
            txtFName.Text = ""; txtFPhone.Text = ""; txtFSpecialty.Text = "";
            numFSalary.Value = 1500; dtpFJoinDate.Value = DateTime.Now;
        }

        private void ShowForm()
        {
            pnlForm.Location = new Point((ClientSize.Width - pnlForm.Width) / 2, (ClientSize.Height - pnlForm.Height) / 2);
            pnlForm.Visible = true; pnlForm.BringToFront(); txtFName.Focus();
        }

        private void HideForm() => pnlForm.Visible = false;

        private void UpdateCount() => lblCount.Text = "إجمالي المدربين: " + _dt.DefaultView.Count;
    }
}
