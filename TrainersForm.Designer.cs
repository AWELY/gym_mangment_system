namespace gym_mangment_system
{
    partial class TrainersForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private Guna.UI2.WinForms.Guna2GradientButton btnAddTrainer;
        private System.Windows.Forms.DataGridView gridTrainers;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblCount;

        // Overlay form panel
        private Guna.UI2.WinForms.Guna2Panel pnlForm;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFName;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.Label lblFPhone;
        private System.Windows.Forms.TextBox txtFPhone;
        private System.Windows.Forms.Label lblFSpecialty;
        private System.Windows.Forms.TextBox txtFSpecialty;
        private System.Windows.Forms.Label lblFSalary;
        private System.Windows.Forms.NumericUpDown numFSalary;
        private System.Windows.Forms.Label lblFJoinDate;
        private System.Windows.Forms.DateTimePicker dtpFJoinDate;
        private Guna.UI2.WinForms.Guna2Button btnFormSave;
        private Guna.UI2.WinForms.Guna2Button btnFormCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle      = new System.Windows.Forms.Label();
            this.pnlSearch     = new System.Windows.Forms.Panel();
            this.btnAddTrainer = new Guna.UI2.WinForms.Guna2GradientButton();
            this.txtSearch     = new System.Windows.Forms.TextBox();
            this.lblSearch     = new System.Windows.Forms.Label();
            this.gridTrainers  = new System.Windows.Forms.DataGridView();
            this.pnlActions    = new System.Windows.Forms.Panel();
            this.lblCount      = new System.Windows.Forms.Label();
            this.btnDelete     = new System.Windows.Forms.Button();
            this.btnEdit       = new System.Windows.Forms.Button();
            this.pnlForm       = new Guna.UI2.WinForms.Guna2Panel();
            this.btnFormCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnFormSave   = new Guna.UI2.WinForms.Guna2Button();
            this.dtpFJoinDate  = new System.Windows.Forms.DateTimePicker();
            this.lblFJoinDate  = new System.Windows.Forms.Label();
            this.numFSalary    = new System.Windows.Forms.NumericUpDown();
            this.lblFSalary    = new System.Windows.Forms.Label();
            this.txtFSpecialty = new System.Windows.Forms.TextBox();
            this.lblFSpecialty = new System.Windows.Forms.Label();
            this.txtFPhone     = new System.Windows.Forms.TextBox();
            this.lblFPhone     = new System.Windows.Forms.Label();
            this.txtFName      = new System.Windows.Forms.TextBox();
            this.lblFName      = new System.Windows.Forms.Label();
            this.lblFormTitle  = new System.Windows.Forms.Label();

            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTrainers)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.pnlForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFSalary)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.Padding   = new System.Windows.Forms.Padding(15, 8, 15, 0);
            this.lblTitle.Size      = new System.Drawing.Size(950, 50);
            this.lblTitle.TabIndex  = 0;
            this.lblTitle.Text      = "🏋️  إدارة المدربين";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // pnlSearch
            this.pnlSearch.Controls.Add(this.btnAddTrainer);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Dock     = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Name     = "pnlSearch";
            this.pnlSearch.Padding  = new System.Windows.Forms.Padding(20, 5, 20, 5);
            this.pnlSearch.Size     = new System.Drawing.Size(950, 55);
            this.pnlSearch.TabIndex = 1;

            this.lblSearch.AutoSize  = true;
            this.lblSearch.Dock      = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font      = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(150, 150, 160);
            this.lblSearch.Name      = "lblSearch";
            this.lblSearch.Padding   = new System.Windows.Forms.Padding(0, 10, 5, 0);
            this.lblSearch.TabIndex  = 0;
            this.lblSearch.Text      = "🔍 بحث:";

            this.txtSearch.Anchor      = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.txtSearch.BackColor   = System.Drawing.Color.FromArgb(38, 38, 45);
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font        = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearch.ForeColor   = System.Drawing.Color.White;
            this.txtSearch.Location    = new System.Drawing.Point(530, 12);
            this.txtSearch.Name        = "txtSearch";
            this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSearch.Size        = new System.Drawing.Size(320, 29);
            this.txtSearch.TabIndex    = 1;

            this.btnAddTrainer.FillColor  = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnAddTrainer.FillColor2 = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnAddTrainer.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnAddTrainer.BorderRadius = 12;
            this.btnAddTrainer.Font       = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddTrainer.ForeColor  = System.Drawing.Color.White;
            this.btnAddTrainer.Location   = new System.Drawing.Point(25, 10);
            this.btnAddTrainer.Name       = "btnAddTrainer";
            this.btnAddTrainer.Size       = new System.Drawing.Size(190, 35);
            this.btnAddTrainer.TabIndex   = 2;
            this.btnAddTrainer.Text       = "➕ إضافة مدرب جديد";

            // gridTrainers
            this.gridTrainers.AllowUserToAddRows    = false;
            this.gridTrainers.AllowUserToDeleteRows = false;
            this.gridTrainers.BackgroundColor       = System.Drawing.Color.FromArgb(24, 24, 30);
            this.gridTrainers.BorderStyle           = System.Windows.Forms.BorderStyle.None;
            this.gridTrainers.CellBorderStyle       = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridTrainers.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(38, 38, 45), ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = System.Drawing.Color.FromArgb(38, 38, 45), SelectionForeColor = System.Drawing.Color.White
            };
            this.gridTrainers.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(26, 26, 32), ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                SelectionBackColor = System.Drawing.Color.FromArgb(50, 50, 60), SelectionForeColor = System.Drawing.Color.White,
                Padding = new System.Windows.Forms.Padding(5, 3, 5, 3)
            };
            this.gridTrainers.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(30, 30, 36) };
            this.gridTrainers.ColumnHeadersHeight         = 48;
            this.gridTrainers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridTrainers.Dock                        = System.Windows.Forms.DockStyle.Fill;
            this.gridTrainers.EnableHeadersVisualStyles   = false;
            this.gridTrainers.GridColor                   = System.Drawing.Color.FromArgb(40, 40, 48);
            this.gridTrainers.Name                        = "gridTrainers";
            this.gridTrainers.ReadOnly                    = true;
            this.gridTrainers.RightToLeft                 = System.Windows.Forms.RightToLeft.Yes;
            this.gridTrainers.RowHeadersVisible           = false;
            this.gridTrainers.RowTemplate.Height          = 44;
            this.gridTrainers.SelectionMode               = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTrainers.TabIndex                    = 2;

            // pnlActions
            this.pnlActions.Controls.Add(this.lblCount);
            this.pnlActions.Controls.Add(this.btnDelete);
            this.pnlActions.Controls.Add(this.btnEdit);
            this.pnlActions.Dock     = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Name     = "pnlActions";
            this.pnlActions.Padding  = new System.Windows.Forms.Padding(20, 8, 20, 8);
            this.pnlActions.Size     = new System.Drawing.Size(950, 55);
            this.pnlActions.TabIndex = 3;

            this.btnEdit.BackColor  = System.Drawing.Color.FromArgb(43, 127, 255);
            this.btnEdit.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Dock       = System.Windows.Forms.DockStyle.Left;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle  = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font       = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor  = System.Drawing.Color.White;
            this.btnEdit.Name       = "btnEdit";
            this.btnEdit.Size       = new System.Drawing.Size(160, 39);
            this.btnEdit.TabIndex   = 0;
            this.btnEdit.Text       = "✏️ تعديل المدرب";
            this.btnEdit.UseVisualStyleBackColor = false;

            this.btnDelete.BackColor  = System.Drawing.Color.FromArgb(231, 0, 11);
            this.btnDelete.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Dock       = System.Windows.Forms.DockStyle.Left;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle  = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font       = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor  = System.Drawing.Color.White;
            this.btnDelete.Margin     = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDelete.Name       = "btnDelete";
            this.btnDelete.Size       = new System.Drawing.Size(160, 39);
            this.btnDelete.TabIndex   = 1;
            this.btnDelete.Text       = "🗑️  حذف المدرب";
            this.btnDelete.UseVisualStyleBackColor = false;

            this.lblCount.Dock      = System.Windows.Forms.DockStyle.Right;
            this.lblCount.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(140, 140, 150);
            this.lblCount.Name      = "lblCount";
            this.lblCount.Size      = new System.Drawing.Size(200, 39);
            this.lblCount.TabIndex  = 2;
            this.lblCount.Text      = "إجمالي المدربين: 0";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── pnlForm overlay ──
            this.pnlForm.FillColor = System.Drawing.Color.FromArgb(32, 32, 40);
            this.pnlForm.BorderRadius = 14;
            this.pnlForm.ShadowDecoration.Enabled = true;
            this.pnlForm.Controls.Add(this.btnFormCancel);
            this.pnlForm.Controls.Add(this.btnFormSave);
            this.pnlForm.Controls.Add(this.dtpFJoinDate);
            this.pnlForm.Controls.Add(this.lblFJoinDate);
            this.pnlForm.Controls.Add(this.numFSalary);
            this.pnlForm.Controls.Add(this.lblFSalary);
            this.pnlForm.Controls.Add(this.txtFSpecialty);
            this.pnlForm.Controls.Add(this.lblFSpecialty);
            this.pnlForm.Controls.Add(this.txtFPhone);
            this.pnlForm.Controls.Add(this.lblFPhone);
            this.pnlForm.Controls.Add(this.txtFName);
            this.pnlForm.Controls.Add(this.lblFName);
            this.pnlForm.Controls.Add(this.lblFormTitle);
            this.pnlForm.Location = new System.Drawing.Point(220, 80);
            this.pnlForm.Name     = "pnlForm";
            this.pnlForm.Padding  = new System.Windows.Forms.Padding(25);
            this.pnlForm.Size     = new System.Drawing.Size(500, 420);
            this.pnlForm.TabIndex = 10;
            this.pnlForm.Visible  = false;

            // lblFormTitle
            this.lblFormTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblFormTitle.Font      = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Name      = "lblFormTitle";
            this.lblFormTitle.Size      = new System.Drawing.Size(450, 40);
            this.lblFormTitle.TabIndex  = 0;
            this.lblFormTitle.Text      = "➕  إضافة مدرب جديد";
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Name
            this.lblFName.AutoSize = true; this.lblFName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFName.ForeColor = System.Drawing.Color.FromArgb(180,180,190); this.lblFName.Location = new System.Drawing.Point(395, 58);
            this.lblFName.Name = "lblFName"; this.lblFName.TabIndex = 1; this.lblFName.Text = "الاسم الكامل:";

            this.txtFName.BackColor = System.Drawing.Color.FromArgb(45,45,55); this.txtFName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFName.Font = new System.Drawing.Font("Segoe UI", 11F); this.txtFName.ForeColor = System.Drawing.Color.White;
            this.txtFName.Location = new System.Drawing.Point(35, 80); this.txtFName.Name = "txtFName";
            this.txtFName.RightToLeft = System.Windows.Forms.RightToLeft.Yes; this.txtFName.Size = new System.Drawing.Size(435, 27); this.txtFName.TabIndex = 2;

            // Phone
            this.lblFPhone.AutoSize = true; this.lblFPhone.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFPhone.ForeColor = System.Drawing.Color.FromArgb(180,180,190); this.lblFPhone.Location = new System.Drawing.Point(400, 118);
            this.lblFPhone.Name = "lblFPhone"; this.lblFPhone.TabIndex = 3; this.lblFPhone.Text = "رقم الهاتف:";

            this.txtFPhone.BackColor = System.Drawing.Color.FromArgb(45,45,55); this.txtFPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFPhone.Font = new System.Drawing.Font("Segoe UI", 11F); this.txtFPhone.ForeColor = System.Drawing.Color.White;
            this.txtFPhone.Location = new System.Drawing.Point(35, 140); this.txtFPhone.Name = "txtFPhone";
            this.txtFPhone.Size = new System.Drawing.Size(435, 27); this.txtFPhone.TabIndex = 4;

            // Specialty
            this.lblFSpecialty.AutoSize = true; this.lblFSpecialty.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFSpecialty.ForeColor = System.Drawing.Color.FromArgb(180,180,190); this.lblFSpecialty.Location = new System.Drawing.Point(380, 178);
            this.lblFSpecialty.Name = "lblFSpecialty"; this.lblFSpecialty.TabIndex = 5; this.lblFSpecialty.Text = "التخصص:";

            this.txtFSpecialty.BackColor = System.Drawing.Color.FromArgb(45,45,55); this.txtFSpecialty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFSpecialty.Font = new System.Drawing.Font("Segoe UI", 11F); this.txtFSpecialty.ForeColor = System.Drawing.Color.White;
            this.txtFSpecialty.Location = new System.Drawing.Point(35, 200); this.txtFSpecialty.Name = "txtFSpecialty";
            this.txtFSpecialty.RightToLeft = System.Windows.Forms.RightToLeft.Yes; this.txtFSpecialty.Size = new System.Drawing.Size(435, 27); this.txtFSpecialty.TabIndex = 6;

            // Salary
            this.lblFSalary.AutoSize = true; this.lblFSalary.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFSalary.ForeColor = System.Drawing.Color.FromArgb(180,180,190); this.lblFSalary.Location = new System.Drawing.Point(395, 238);
            this.lblFSalary.Name = "lblFSalary"; this.lblFSalary.TabIndex = 7; this.lblFSalary.Text = "الراتب $:";

            this.numFSalary.BackColor = System.Drawing.Color.FromArgb(45,45,55); this.numFSalary.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.numFSalary.ForeColor = System.Drawing.Color.White; this.numFSalary.Location = new System.Drawing.Point(35, 260);
            this.numFSalary.Maximum = new decimal(new int[]{99999,0,0,0}); this.numFSalary.Minimum = new decimal(new int[]{0,0,0,0});
            this.numFSalary.Name = "numFSalary"; this.numFSalary.Size = new System.Drawing.Size(435, 27); this.numFSalary.TabIndex = 8;
            this.numFSalary.Value = new decimal(new int[]{1500,0,0,0});

            // Join Date
            this.lblFJoinDate.AutoSize = true; this.lblFJoinDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFJoinDate.ForeColor = System.Drawing.Color.FromArgb(180,180,190); this.lblFJoinDate.Location = new System.Drawing.Point(370, 298);
            this.lblFJoinDate.Name = "lblFJoinDate"; this.lblFJoinDate.TabIndex = 9; this.lblFJoinDate.Text = "تاريخ التعيين:";

            this.dtpFJoinDate.Font    = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpFJoinDate.Format  = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFJoinDate.Location= new System.Drawing.Point(35, 320); this.dtpFJoinDate.Name = "dtpFJoinDate";
            this.dtpFJoinDate.Size    = new System.Drawing.Size(435, 27); this.dtpFJoinDate.TabIndex = 10;

            // Save/Cancel
            this.btnFormSave.FillColor = System.Drawing.Color.FromArgb(0, 166, 62); this.btnFormSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFormSave.BorderRadius = 8;
            this.btnFormSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold); this.btnFormSave.ForeColor = System.Drawing.Color.White;
            this.btnFormSave.Location = new System.Drawing.Point(245, 362); this.btnFormSave.Name = "btnFormSave";
            this.btnFormSave.Size = new System.Drawing.Size(225, 40); this.btnFormSave.TabIndex = 11; this.btnFormSave.Text = "✓  حفظ";

            this.btnFormCancel.FillColor = System.Drawing.Color.FromArgb(55,55,65); this.btnFormCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFormCancel.BorderRadius = 8;
            this.btnFormCancel.Font = new System.Drawing.Font("Segoe UI", 12F); this.btnFormCancel.ForeColor = System.Drawing.Color.LightGray;
            this.btnFormCancel.Location = new System.Drawing.Point(35, 362); this.btnFormCancel.Name = "btnFormCancel";
            this.btnFormCancel.Size = new System.Drawing.Size(200, 40); this.btnFormCancel.TabIndex = 12; this.btnFormCancel.Text = "إلغاء";

            // TrainersForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor     = System.Drawing.Color.FromArgb(18, 18, 22);
            this.ClientSize    = new System.Drawing.Size(950, 660);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.gridTrainers);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.lblTitle);
            this.Name        = "TrainersForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text        = "إدارة المدربين";

            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTrainers)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFSalary)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
