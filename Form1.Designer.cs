namespace gym_mangment_system
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblTitleBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Panel pnlPass;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.LinkLabel lnkForgot;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.topBar = new System.Windows.Forms.Panel();
            this.lblTitleBar = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.pnlPass = new System.Windows.Forms.Panel();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.lnkForgot = new System.Windows.Forms.LinkLabel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            
            this.topBar.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlPass.SuspendLayout();
            this.SuspendLayout();

            // topBar
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Height = 30;
            this.topBar.BackColor = System.Drawing.Color.FromArgb(15, 15, 15);
            this.topBar.Controls.Add(this.lblTitleBar);
            this.topBar.Controls.Add(this.btnClose);

            // lblTitleBar
            this.lblTitleBar.Text = "Gym Management System - Login";
            this.lblTitleBar.ForeColor = System.Drawing.Color.White;
            this.lblTitleBar.AutoSize = true;
            this.lblTitleBar.Location = new System.Drawing.Point(10, 8);
            this.lblTitleBar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);

            // btnClose
            this.btnClose.Text = "X";
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Click += new System.EventHandler(this.BtnExit_Click);

            // mainPanel
            this.mainPanel.Size = new System.Drawing.Size(400, 500);
            this.mainPanel.Location = new System.Drawing.Point(50, 80);
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.mainPanel.Controls.Add(this.lblTitle);
            this.mainPanel.Controls.Add(this.lblUser);
            this.mainPanel.Controls.Add(this.pnlUser);
            this.mainPanel.Controls.Add(this.lblPass);
            this.mainPanel.Controls.Add(this.pnlPass);
            this.mainPanel.Controls.Add(this.chkRemember);
            this.mainPanel.Controls.Add(this.lnkForgot);
            this.mainPanel.Controls.Add(this.btnLogin);
            this.mainPanel.Controls.Add(this.btnExit);

            // lblTitle
            this.lblTitle.Text = "تسجيل الدخول";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(110, 50);

            // lblUser
            this.lblUser.Text = "اسم المستخدم:";
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUser.ForeColor = System.Drawing.Color.LightGray;
            this.lblUser.Location = new System.Drawing.Point(50, 140);
            this.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblUser.AutoSize = true;

            // pnlUser
            this.pnlUser.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.pnlUser.Size = new System.Drawing.Size(300, 35);
            this.pnlUser.Location = new System.Drawing.Point(50, 165);
            this.pnlUser.Controls.Add(this.txtUser);

            // txtUser
            this.txtUser.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.txtUser.ForeColor = System.Drawing.Color.White;
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUser.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUser.Location = new System.Drawing.Point(10, 7);
            this.txtUser.Size = new System.Drawing.Size(280, 25);
            this.txtUser.Text = "Admin";

            // lblPass
            this.lblPass.Text = "كلمة المرور:";
            this.lblPass.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPass.ForeColor = System.Drawing.Color.LightGray;
            this.lblPass.Location = new System.Drawing.Point(50, 220);
            this.lblPass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPass.AutoSize = true;

            // pnlPass
            this.pnlPass.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.pnlPass.Size = new System.Drawing.Size(300, 35);
            this.pnlPass.Location = new System.Drawing.Point(50, 245);
            this.pnlPass.Controls.Add(this.txtPass);

            // txtPass
            this.txtPass.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.txtPass.ForeColor = System.Drawing.Color.White;
            this.txtPass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPass.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPass.Location = new System.Drawing.Point(10, 7);
            this.txtPass.Size = new System.Drawing.Size(280, 25);
            this.txtPass.PasswordChar = '•';
            this.txtPass.Text = "Password";

            // chkRemember
            this.chkRemember.Text = "تذكرني";
            this.chkRemember.ForeColor = System.Drawing.Color.LightGray;
            this.chkRemember.Location = new System.Drawing.Point(50, 290);
            this.chkRemember.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

            // lnkForgot
            this.lnkForgot.Text = "نسيت كلمة المرور؟";
            this.lnkForgot.LinkColor = System.Drawing.Color.SteelBlue;
            this.lnkForgot.Location = new System.Drawing.Point(190, 292);
            this.lnkForgot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lnkForgot.AutoSize = true;

            // btnLogin
            this.btnLogin.Text = "تسجيل الدخول  [->";
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Size = new System.Drawing.Size(300, 45);
            this.btnLogin.Location = new System.Drawing.Point(50, 340);
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);

            // btnExit
            this.btnExit.Text = "خروج";
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnExit.Size = new System.Drawing.Size(300, 45);
            this.btnExit.Location = new System.Drawing.Point(50, 400);
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);

            // Form1
            this.Text = "Gym Management System - Login";
            this.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.ClientSize = new System.Drawing.Size(500, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.mainPanel);

            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlPass.ResumeLayout(false);
            this.pnlPass.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
