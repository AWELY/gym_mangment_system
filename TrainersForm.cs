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
        private FlowLayoutPanel _cardHost;
        private Label _lblSubtitle;

        public TrainersForm()
            : this(false) { }

        public TrainersForm(bool startAddMode)
        {
            _startAddMode = startAddMode;
            InitializeComponent();
            InitTrainersTable();
            BuildCardHost();
            RebindTrainersFromStore();
            WireEvents();
            ApplyTheme(ThemeManager.Current);
            UpdateCount();

            if (_startAddMode)
                BtnAddTrainer_Click(this, EventArgs.Empty);
        }

        // ── Figma card layout (replaces the grid) ──────────
        private void BuildCardHost()
        {
            gridTrainers.Visible = false;
            pnlActions.Visible   = false;
            lblSearch.Visible    = false;
            txtSearch.Visible    = false;

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

            _lblSubtitle = new Label
            {
                AutoSize  = false,
                Dock      = DockStyle.Right,
                Width     = 360,
                Font      = new Font("Segoe UI", 11F),
                TextAlign = ContentAlignment.MiddleRight,
                Padding   = new Padding(0, 10, 6, 0),
                BackColor = Color.Transparent
            };
            pnlSearch.Controls.Add(_lblSubtitle);
        }

        private void BuildTrainerCards()
        {
            if (_cardHost == null) return;
            UiColorScheme s = ThemeManager.Current;
            _cardHost.SuspendLayout();
            foreach (Control c in _cardHost.Controls) c.Dispose();
            _cardHost.Controls.Clear();

            foreach (var t in GymDataStore.Data.Trainers.OrderBy(x => x.Id))
                _cardHost.Controls.Add(BuildTrainerCard(t, s));

            _cardHost.ResumeLayout();
        }

        private Panel BuildTrainerCard(TrainerRecord t, UiColorScheme s)
        {
            int w = 330, h = 200, pad = 18, innerW = w - pad * 2;
            Panel card = new Panel { Size = new Size(w, h), Margin = new Padding(10), BackColor = s.Card };
            DashboardForm.StyleAsRoundedCard(card, s.BorderSubtle, 14);

            Label name = new Label { Text = t.Name, Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = s.TextPrimary, Location = new Point(pad, 16), Size = new Size(innerW, 28), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label phone = new Label { Text = (t.Phone ?? "") + "  📞", Font = new Font("Segoe UI", 10F), ForeColor = s.TextMuted, Location = new Point(pad, 46), Size = new Size(innerW, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label spec = new Label { Text = "التخصص: " + (t.Specialty ?? ""), Font = new Font("Segoe UI", 10F), ForeColor = s.TextMuted, Location = new Point(pad, 72), Size = new Size(innerW, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label salary = new Label { Text = "الراتب: " + t.Salary.ToString("0") + " ريال/شهر", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = FigmaPalette.Primary, Location = new Point(pad, 98), Size = new Size(innerW, 26), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label join = new Label { Text = "تاريخ الانضمام: " + (t.JoinDate ?? ""), Font = new Font("Segoe UI", 9.5F), ForeColor = s.TextMuted, Location = new Point(pad, 126), Size = new Size(innerW, 20), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };

            int btnW = (innerW - 10) / 2, btnY = 154, btnH = 34;
            Button del = MakeCardButton("🗑  حذف", FigmaPalette.Red, new Point(pad, btnY), new Size(btnW, btnH));
            Button edit = MakeCardButton("✎  تعديل", FigmaPalette.BlueBtn, new Point(pad + btnW + 10, btnY), new Size(btnW, btnH));
            int id = t.Id;
            del.Click += (_, __) => DeleteTrainer(id, t.Name);
            edit.Click += (_, __) => LoadTrainerIntoForm(id);

            card.Controls.Add(name); card.Controls.Add(phone); card.Controls.Add(spec);
            card.Controls.Add(salary); card.Controls.Add(join); card.Controls.Add(del); card.Controls.Add(edit);
            return card;
        }

        internal static Button MakeCardButton(string text, Color back, Point loc, Size size)
        {
            var b = new Button
            {
                Text = text, BackColor = back, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = loc, Size = size, Cursor = Cursors.Hand, UseVisualStyleBackColor = false
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void DeleteTrainer(int id, string name)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف المدرب: " + name + "؟", "⚠️ تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            GymDataStore.Data.Trainers.RemoveAll(x => x.Id == id);
            GymDataStore.Save();
            RebindTrainersFromStore();
        }

        private void LoadTrainerIntoForm(int id)
        {
            var tr = GymDataStore.Data.Trainers.FirstOrDefault(x => x.Id == id);
            if (tr == null) return;
            _isEditing = true;
            _editingTrainerId = id;
            txtFName.Text = tr.Name; txtFPhone.Text = tr.Phone; txtFSpecialty.Text = tr.Specialty;
            numFSalary.Value = Math.Max(numFSalary.Minimum, Math.Min(numFSalary.Maximum, tr.Salary));
            DateTime d; if (!DateTime.TryParse(tr.JoinDate, out d)) d = DateTime.Now;
            dtpFJoinDate.Value = d;
            lblFormTitle.Text = "✏️  تعديل بيانات المدرب";
            ShowForm();
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

            if (_lblSubtitle != null) _lblSubtitle.ForeColor = s.TextMuted;
            if (_cardHost != null)
            {
                _cardHost.BackColor = s.ContentHost;
                BuildTrainerCards();
            }
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
            BuildTrainerCards();
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

        private void UpdateCount()
        {
            decimal totalSalaries = GymDataStore.Data.Trainers.Sum(x => x.Salary);
            string txt = "إجمالي الرواتب الشهرية: " + totalSalaries.ToString("0") + " ريال";
            lblCount.Text = txt;
            if (_lblSubtitle != null) _lblSubtitle.Text = txt;
        }
    }
}
