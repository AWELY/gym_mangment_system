using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class TrainersForm : Form, IThemeAware
    {
        private DataTable _dt;
        private bool _isEditing       = false;
        private int  _editingTrainerId = -1;
        private readonly bool _startAddMode;

        public TrainersForm()
            : this(false) { }

        public TrainersForm(bool startAddMode)
        {
            _startAddMode = startAddMode;
            InitializeComponent();
            InitTrainersTable();
            RebindTrainersFromStore();
            WireEvents();
            ApplyTheme(ThemeManager.Current);
            UpdateCount();

            if (_startAddMode)
                BtnAddTrainer_Click(this, EventArgs.Empty);
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            lblTitle.ForeColor = s.TextPrimary;
            lblSearch.ForeColor = s.TextMuted;
            txtSearch.BackColor = s.InputBackground;
            txtSearch.ForeColor = s.InputForeground;
            ThemeManager.StyleDataGridView(gridTrainers, s);

            lblCount.ForeColor = s.TextMuted;

            pnlForm.BackColor = s.Card;
            lblFormTitle.ForeColor = s.TextPrimary;
            foreach (Label lb in new[] { lblFName, lblFPhone, lblFSpecialty, lblFSalary, lblFJoinDate })
                lb.ForeColor = s.TextMuted;
            txtFName.BackColor = txtFPhone.BackColor = txtFSpecialty.BackColor = numFSalary.BackColor = s.InputBackground;
            txtFName.ForeColor = txtFPhone.ForeColor = txtFSpecialty.ForeColor = numFSalary.ForeColor = s.InputForeground;
            dtpFJoinDate.CalendarMonthBackground = s.InputBackground;
            dtpFJoinDate.CalendarForeColor = s.InputForeground;
            btnFormCancel.BackColor = s.SecondaryButton;
            btnFormCancel.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.LightGray;
            btnFormCancel.FlatAppearance.MouseOverBackColor = s.SecondaryButtonHover;
        }

        private void InitTrainersTable()
        {
            _dt = new DataTable();
            _dt.Columns.Add("ID",            typeof(int));
            _dt.Columns.Add("الاسم",         typeof(string));
            _dt.Columns.Add("الهاتف",        typeof(string));
            _dt.Columns.Add("التخصص",        typeof(string));
            _dt.Columns.Add("الراتب ($)",    typeof(decimal));
            _dt.Columns.Add("تاريخ التعيين", typeof(string));

            gridTrainers.DataSource = _dt;
            gridTrainers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (gridTrainers.Columns.Contains("ID")) gridTrainers.Columns["ID"].Visible = false;
        }

        private void RebindTrainersFromStore()
        {
            _dt.Rows.Clear();
            foreach (var t in GymDataStore.Data.Trainers.OrderBy(x => x.Id))
                _dt.Rows.Add(t.Id, t.Name, t.Phone, t.Specialty, t.Salary, t.JoinDate);
            _dt.AcceptChanges();
            UpdateCount();
        }

        private void WireEvents()
        {
            btnAddTrainer.Click  += BtnAddTrainer_Click;
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

        private void BtnAddTrainer_Click(object sender, EventArgs e)
        {
            _isEditing = false;
            _editingTrainerId = -1;
            lblFormTitle.Text = "➕  إضافة مدرب جديد";
            ClearForm();
            ShowForm();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (gridTrainers.SelectedRows.Count == 0) { MessageBox.Show("الرجاء تحديد مدرب للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            _isEditing = true;
            var row = gridTrainers.SelectedRows[0];
            _editingTrainerId = Convert.ToInt32(row.Cells["ID"].Value);
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
                int id = Convert.ToInt32(gridTrainers.SelectedRows[0].Cells["ID"].Value);
                GymDataStore.Data.Trainers.RemoveAll(x => x.Id == id);
                GymDataStore.Save();
                RebindTrainersFromStore();
                MessageBox.Show("تم حذف المدرب بنجاح", "✅ تم الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnFormSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFName.Text)) { MessageBox.Show("الرجاء إدخال اسم المدرب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (_isEditing && _editingTrainerId > 0)
            {
                var tr = GymDataStore.Data.Trainers.FirstOrDefault(x => x.Id == _editingTrainerId);
                if (tr != null)
                {
                    tr.Name      = txtFName.Text.Trim();
                    tr.Phone     = txtFPhone.Text.Trim();
                    tr.Specialty = txtFSpecialty.Text.Trim();
                    tr.Salary    = numFSalary.Value;
                    tr.JoinDate  = dtpFJoinDate.Value.ToString("yyyy-MM-dd");
                }
                GymDataStore.Save();
                MessageBox.Show("تم تحديث بيانات المدرب", "✅ تم التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                GymDataStore.Data.Trainers.Add(new TrainerRecord
                {
                    Id         = GymDataStore.NextTrainerId(),
                    Name       = txtFName.Text.Trim(),
                    Phone      = txtFPhone.Text.Trim(),
                    Specialty  = txtFSpecialty.Text.Trim(),
                    Salary     = numFSalary.Value,
                    JoinDate   = dtpFJoinDate.Value.ToString("yyyy-MM-dd")
                });
                GymDataStore.Save();
                MessageBox.Show("تمت إضافة المدرب بنجاح", "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            HideForm();
            RebindTrainersFromStore();
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
