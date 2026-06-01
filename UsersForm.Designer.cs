namespace gym_mangment_system
{
    partial class UsersForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView gridUsers;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Label lblCount;

        // Overlay form
        private Guna.UI2.WinForms.Guna2Panel pnlForm;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFUsername;
        private System.Windows.Forms.TextBox txtFUsername;
        private System.Windows.Forms.Label lblFPassword;
        private System.Windows.Forms.TextBox txtFPassword;
        private System.Windows.Forms.Label lblFRole;
        private System.Windows.Forms.ComboBox cmbFRole;
        private System.Windows.Forms.Label lblFFullName;
        private System.Windows.Forms.TextBox txtFFullName;
        private Guna.UI2.WinForms.Guna2Button btnFormSave;
        private Guna.UI2.WinForms.Guna2Button btnFormCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle     = new System.Windows.Forms.Label();
            this.gridUsers    = new System.Windows.Forms.DataGridView();
            this.pnlActions   = new System.Windows.Forms.Panel();
            this.lblCount     = new System.Windows.Forms.Label();
            this.btnDeleteUser= new System.Windows.Forms.Button();
            this.btnEditUser  = new System.Windows.Forms.Button();
            this.btnAddUser   = new System.Windows.Forms.Button();
            this.pnlForm      = new Guna.UI2.WinForms.Guna2Panel();
            this.btnFormCancel= new Guna.UI2.WinForms.Guna2Button();
            this.btnFormSave  = new Guna.UI2.WinForms.Guna2Button();
            this.cmbFRole     = new System.Windows.Forms.ComboBox();
            this.lblFRole     = new System.Windows.Forms.Label();
            this.txtFPassword = new System.Windows.Forms.TextBox();
            this.lblFPassword = new System.Windows.Forms.Label();
            this.txtFUsername = new System.Windows.Forms.TextBox();
            this.lblFUsername = new System.Windows.Forms.Label();
            this.txtFFullName = new System.Windows.Forms.TextBox();
            this.lblFFullName = new System.Windows.Forms.Label();
            this.lblFormTitle = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.Padding   = new System.Windows.Forms.Padding(15, 8, 15, 0);
            this.lblTitle.Size      = new System.Drawing.Size(950, 50);
            this.lblTitle.TabIndex  = 0;
            this.lblTitle.Text      = "👤  إدارة المستخدمين";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // gridUsers
            this.gridUsers.AllowUserToAddRows    = false;
            this.gridUsers.AllowUserToDeleteRows = false;
            this.gridUsers.BackgroundColor       = System.Drawing.Color.FromArgb(24, 24, 30);
            this.gridUsers.BorderStyle           = System.Windows.Forms.BorderStyle.None;
            this.gridUsers.CellBorderStyle       = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridUsers.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(38,38,45), ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = System.Drawing.Color.FromArgb(38,38,45), SelectionForeColor = System.Drawing.Color.White
            };
            this.gridUsers.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(26,26,32), ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                SelectionBackColor = System.Drawing.Color.FromArgb(50,50,60), SelectionForeColor = System.Drawing.Color.White,
                Padding = new System.Windows.Forms.Padding(5,3,5,3)
            };
            this.gridUsers.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(30,30,36) };
            this.gridUsers.ColumnHeadersHeight         = 42;
            this.gridUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridUsers.Dock                        = System.Windows.Forms.DockStyle.Fill;
            this.gridUsers.EnableHeadersVisualStyles   = false;
            this.gridUsers.GridColor                   = System.Drawing.Color.FromArgb(40,40,48);
            this.gridUsers.Name                        = "gridUsers";
            this.gridUsers.ReadOnly                    = true;
            this.gridUsers.RightToLeft                 = System.Windows.Forms.RightToLeft.Yes;
            this.gridUsers.RowHeadersVisible           = false;
            this.gridUsers.RowTemplate.Height          = 44;
            this.gridUsers.SelectionMode               = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUsers.TabIndex                    = 1;

            // pnlActions
            this.pnlActions.Controls.Add(this.lblCount);
            this.pnlActions.Controls.Add(this.btnDeleteUser);
            this.pnlActions.Controls.Add(this.btnEditUser);
            this.pnlActions.Controls.Add(this.btnAddUser);
            this.pnlActions.Dock    = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Name    = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(20,8,20,8);
            this.pnlActions.Size    = new System.Drawing.Size(950,55);
            this.pnlActions.TabIndex= 2;

            void StyleBtn(System.Windows.Forms.Button b, string text, System.Drawing.Color col, int tab)
            {
                b.BackColor = col; b.Cursor = System.Windows.Forms.Cursors.Hand;
                b.FlatAppearance.BorderSize = 0; b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                b.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
                b.ForeColor = System.Drawing.Color.White; b.Dock = System.Windows.Forms.DockStyle.Left;
                b.Size = new System.Drawing.Size(160, 39); b.TabIndex = tab; b.Text = text;
                b.UseVisualStyleBackColor = false;
            }
            StyleBtn(this.btnAddUser,    "➕ إضافة مستخدم",  System.Drawing.Color.FromArgb(79, 57, 246),  0);
            StyleBtn(this.btnEditUser,   "✏️ تعديل",         System.Drawing.Color.FromArgb(43, 127, 255), 1);
            StyleBtn(this.btnDeleteUser, "🗑️ حذف",          System.Drawing.Color.FromArgb(231, 0, 11),  2);

            this.lblCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(140,140,150);
            this.lblCount.Name = "lblCount"; this.lblCount.Size = new System.Drawing.Size(200, 39);
            this.lblCount.TabIndex = 3; this.lblCount.Text = "المستخدمون: 0";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // pnlForm overlay
            this.pnlForm.FillColor = System.Drawing.Color.FromArgb(32, 32, 40);
            this.pnlForm.BorderRadius = 14;
            this.pnlForm.ShadowDecoration.Enabled = true;
            this.pnlForm.Controls.Add(this.btnFormCancel);
            this.pnlForm.Controls.Add(this.btnFormSave);
            this.pnlForm.Controls.Add(this.cmbFRole);
            this.pnlForm.Controls.Add(this.lblFRole);
            this.pnlForm.Controls.Add(this.txtFPassword);
            this.pnlForm.Controls.Add(this.lblFPassword);
            this.pnlForm.Controls.Add(this.txtFUsername);
            this.pnlForm.Controls.Add(this.lblFUsername);
            this.pnlForm.Controls.Add(this.txtFFullName);
            this.pnlForm.Controls.Add(this.lblFFullName);
            this.pnlForm.Controls.Add(this.lblFormTitle);
            this.pnlForm.Location = new System.Drawing.Point(220, 100);
            this.pnlForm.Name = "pnlForm"; this.pnlForm.Padding = new System.Windows.Forms.Padding(25);
            this.pnlForm.Size = new System.Drawing.Size(500, 380); this.pnlForm.TabIndex = 10; this.pnlForm.Visible = false;

            this.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White; this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(450, 40); this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "➕  إضافة مستخدم جديد"; this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            void MkLbl(System.Windows.Forms.Label l, string text, int x, int y, int tab)
            { l.AutoSize=true; l.Font=new System.Drawing.Font("Segoe UI",11F); l.ForeColor=System.Drawing.Color.FromArgb(180,180,190); l.Location=new System.Drawing.Point(x,y); l.TabIndex=tab; l.Text=text; }
            void MkTxt(System.Windows.Forms.TextBox t, int y, int tab, bool pwd=false)
            { t.BackColor=System.Drawing.Color.FromArgb(45,45,55); t.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle; t.Font=new System.Drawing.Font("Segoe UI",11F); t.ForeColor=System.Drawing.Color.White; t.Location=new System.Drawing.Point(35,y); t.Size=new System.Drawing.Size(435,27); t.TabIndex=tab; if(pwd) t.PasswordChar='●'; }

            MkLbl(this.lblFFullName, "الاسم الكامل:", 390, 55, 1); MkTxt(this.txtFFullName, 78, 2);
            MkLbl(this.lblFUsername, "اسم المستخدم:", 390, 115, 3); MkTxt(this.txtFUsername, 138, 4);
            MkLbl(this.lblFPassword, "كلمة المرور:",  395, 175, 5); MkTxt(this.txtFPassword, 198, 6, true);
            MkLbl(this.lblFRole, "الصلاحية:", 405, 235, 7);

            this.cmbFRole.BackColor = System.Drawing.Color.FromArgb(45,45,55);
            this.cmbFRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFRole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFRole.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbFRole.ForeColor = System.Drawing.Color.White;
            this.cmbFRole.Items.AddRange(new object[] { "Admin  (مدير)", "Recipient  (مستلم)" });
            this.cmbFRole.Location = new System.Drawing.Point(35, 258); this.cmbFRole.Name = "cmbFRole";
            this.cmbFRole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbFRole.Size = new System.Drawing.Size(435, 28); this.cmbFRole.TabIndex = 8;

            this.btnFormSave.FillColor = System.Drawing.Color.FromArgb(0, 166, 62); this.btnFormSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFormSave.BorderRadius = 8;
            this.btnFormSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold); this.btnFormSave.ForeColor = System.Drawing.Color.White;
            this.btnFormSave.Location = new System.Drawing.Point(245, 320); this.btnFormSave.Name = "btnFormSave";
            this.btnFormSave.Size = new System.Drawing.Size(225, 40); this.btnFormSave.TabIndex = 9; this.btnFormSave.Text = "✓  حفظ";

            this.btnFormCancel.FillColor = System.Drawing.Color.FromArgb(55,55,65); this.btnFormCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFormCancel.BorderRadius = 8;
            this.btnFormCancel.Font = new System.Drawing.Font("Segoe UI", 12F); this.btnFormCancel.ForeColor = System.Drawing.Color.LightGray;
            this.btnFormCancel.Location = new System.Drawing.Point(35, 320); this.btnFormCancel.Name = "btnFormCancel";
            this.btnFormCancel.Size = new System.Drawing.Size(200, 40); this.btnFormCancel.TabIndex = 10; this.btnFormCancel.Text = "إلغاء";

            // UsersForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor     = System.Drawing.Color.FromArgb(18, 18, 22);
            this.ClientSize    = new System.Drawing.Size(950, 660);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.gridUsers);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.lblTitle);
            this.Name        = "UsersForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text        = "إدارة المستخدمين";

            ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
