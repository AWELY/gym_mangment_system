namespace gym_mangment_system
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblTitleBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel brandPanel;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblGymName;
        private System.Windows.Forms.Label lblWelcome;
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
            this.brandPanel = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblGymName = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
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
            this.brandPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
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
            this.mainPanel.Size = new System.Drawing.Size(540, 560);
            this.mainPanel.Location = new System.Drawing.Point(40, 70);
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

            // brandPanel (right)
            this.brandPanel.Size = new System.Drawing.Size(430, 560);
            this.brandPanel.Location = new System.Drawing.Point(600, 70);
            this.brandPanel.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.brandPanel.Padding = new System.Windows.Forms.Padding(20);
            this.brandPanel.Controls.Add(this.lblWelcome);
            this.brandPanel.Controls.Add(this.lblGymName);
            this.brandPanel.Controls.Add(this.picLogo);

            // picLogo
            this.picLogo.Location = new System.Drawing.Point(70, 50);
            this.picLogo.Size = new System.Drawing.Size(290, 230);
            this.picLogo.BackColor = System.Drawing.Color.Transparent;

            // lblGymName
            this.lblGymName.Location = new System.Drawing.Point(70, 300);
            this.lblGymName.Size = new System.Drawing.Size(290, 80);
            this.lblGymName.Text = "GLORY";
            this.lblGymName.Font = new System.Drawing.Font("Segoe UI", 34F, System.Drawing.FontStyle.Bold);
            this.lblGymName.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblGymName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblWelcome
            this.lblWelcome.Location = new System.Drawing.Point(70, 390);
            this.lblWelcome.Size = new System.Drawing.Size(290, 120);
            this.lblWelcome.Text = "Welcome back!\r\nمرحباً بك في نظام Glory";
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblTitle
            this.lblTitle.Text = "تسجيل الدخول";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(170, 55);

            // lblUser
            this.lblUser.Text = "اسم المستخدم:";
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUser.ForeColor = System.Drawing.Color.LightGray;
            this.lblUser.Location = new System.Drawing.Point(70, 160);
            this.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblUser.AutoSize = true;

            // pnlUser
            this.pnlUser.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.pnlUser.Size = new System.Drawing.Size(400, 38);
            this.pnlUser.Location = new System.Drawing.Point(70, 185);
            this.pnlUser.Controls.Add(this.txtUser);

            // txtUser
            this.txtUser.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.txtUser.ForeColor = System.Drawing.Color.White;
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUser.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUser.Location = new System.Drawing.Point(10, 7);
            this.txtUser.Size = new System.Drawing.Size(380, 25);
            this.txtUser.Text = "Admin";

            // lblPass
            this.lblPass.Text = "كلمة المرور:";
            this.lblPass.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPass.ForeColor = System.Drawing.Color.LightGray;
            this.lblPass.Location = new System.Drawing.Point(70, 250);
            this.lblPass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPass.AutoSize = true;

            // pnlPass
            this.pnlPass.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.pnlPass.Size = new System.Drawing.Size(400, 38);
            this.pnlPass.Location = new System.Drawing.Point(70, 275);
            this.pnlPass.Controls.Add(this.txtPass);

            // txtPass
            this.txtPass.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.txtPass.ForeColor = System.Drawing.Color.White;
            this.txtPass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPass.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPass.Location = new System.Drawing.Point(10, 7);
            this.txtPass.Size = new System.Drawing.Size(380, 25);
            this.txtPass.PasswordChar = '•';
            this.txtPass.Text = "Password";

            // chkRemember
            this.chkRemember.Text = "تذكرني";
            this.chkRemember.ForeColor = System.Drawing.Color.LightGray;
            this.chkRemember.Location = new System.Drawing.Point(70, 325);
            this.chkRemember.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

            // lnkForgot
            this.lnkForgot.Text = "نسيت كلمة المرور؟";
            this.lnkForgot.LinkColor = System.Drawing.Color.SteelBlue;
            this.lnkForgot.Location = new System.Drawing.Point(290, 327);
            this.lnkForgot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lnkForgot.AutoSize = true;

            // btnLogin
            this.btnLogin.Text = "تسجيل الدخول  [->";
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(79, 57, 246);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(67, 45, 212);
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Size = new System.Drawing.Size(400, 50);
            this.btnLogin.Location = new System.Drawing.Point(70, 385);
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);

            // btnExit
            this.btnExit.Text = "خروج";
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnExit.Size = new System.Drawing.Size(400, 46);
            this.btnExit.Location = new System.Drawing.Point(70, 445);
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);

            // Form1
            this.Text = "Gym Management System - Login";
            this.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.ClientSize = new System.Drawing.Size(1080, 680);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.brandPanel);

            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.brandPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlPass.ResumeLayout(false);
            this.pnlPass.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
