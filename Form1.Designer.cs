namespace gym_mangment_system
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private Guna.UI2.WinForms.Guna2Button btnClose;
        private System.Windows.Forms.Panel pnlIcon;
        private System.Windows.Forms.Label lblGymName;
        private System.Windows.Forms.Label lblSubtitle;

        private Guna.UI2.WinForms.Guna2Panel card;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Label lblUserIcon;
        private Guna.UI2.WinForms.Guna2TextBox txtUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Panel pnlPass;
        private System.Windows.Forms.Label lblPassIcon;
        private Guna.UI2.WinForms.Guna2TextBox txtPass;
        private Guna.UI2.WinForms.Guna2GradientButton btnLogin;
        private System.Windows.Forms.Label lblDefaults;
        private Guna.UI2.WinForms.Guna2Panel chipAdmin;
        private System.Windows.Forms.Label lblChipAdminRole;
        private System.Windows.Forms.Label lblChipAdminCreds;
        private Guna.UI2.WinForms.Guna2Panel chipReception;
        private System.Windows.Forms.Label lblChipRecRole;
        private System.Windows.Forms.Label lblChipRecCreds;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnClose         = new Guna.UI2.WinForms.Guna2Button();
            this.pnlIcon          = new System.Windows.Forms.Panel();
            this.lblGymName       = new System.Windows.Forms.Label();
            this.lblSubtitle      = new System.Windows.Forms.Label();
            this.card             = new Guna.UI2.WinForms.Guna2Panel();
            this.lblUser          = new System.Windows.Forms.Label();
            this.pnlUser          = new System.Windows.Forms.Panel();
            this.lblUserIcon      = new System.Windows.Forms.Label();
            this.txtUser          = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblPass          = new System.Windows.Forms.Label();
            this.pnlPass          = new System.Windows.Forms.Panel();
            this.lblPassIcon      = new System.Windows.Forms.Label();
            this.txtPass          = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnLogin         = new Guna.UI2.WinForms.Guna2GradientButton();
            this.lblDefaults      = new System.Windows.Forms.Label();
            this.chipAdmin        = new Guna.UI2.WinForms.Guna2Panel();
            this.lblChipAdminRole = new System.Windows.Forms.Label();
            this.lblChipAdminCreds= new System.Windows.Forms.Label();
            this.chipReception    = new Guna.UI2.WinForms.Guna2Panel();
            this.lblChipRecRole   = new System.Windows.Forms.Label();
            this.lblChipRecCreds  = new System.Windows.Forms.Label();

            this.card.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlPass.SuspendLayout();
            this.chipAdmin.SuspendLayout();
            this.chipReception.SuspendLayout();
            this.SuspendLayout();

            // btnClose (top-right)
            this.btnClose.Text = "✕";
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FillColor = System.Drawing.Color.Transparent;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnClose.BorderRadius = 8;
            this.btnClose.Size = new System.Drawing.Size(40, 36);
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.Location = new System.Drawing.Point(948, 14);
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.HoverState.FillColor = System.Drawing.Color.FromArgb(80, 255, 255, 255);
            this.btnClose.Click += new System.EventHandler(this.BtnExit_Click);

            // pnlIcon (gradient dumbbell square, painted at runtime)
            this.pnlIcon.Size = new System.Drawing.Size(76, 76);
            this.pnlIcon.Location = new System.Drawing.Point(462, 64);
            this.pnlIcon.BackColor = System.Drawing.Color.Transparent;
            this.pnlIcon.Anchor = System.Windows.Forms.AnchorStyles.Top;

            // lblGymName
            this.lblGymName.Text = "Glory Gym";
            this.lblGymName.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
            this.lblGymName.ForeColor = System.Drawing.Color.White;
            this.lblGymName.Size = new System.Drawing.Size(500, 52);
            this.lblGymName.Location = new System.Drawing.Point(250, 152);
            this.lblGymName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGymName.BackColor = System.Drawing.Color.Transparent;
            this.lblGymName.Anchor = System.Windows.Forms.AnchorStyles.Top;

            // lblSubtitle
            this.lblSubtitle.Text = "نظام إدارة الصالة الرياضية";
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(0xBF, 0xD6, 0xD2);
            this.lblSubtitle.Size = new System.Drawing.Size(500, 26);
            this.lblSubtitle.Location = new System.Drawing.Point(250, 206);
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.Anchor = System.Windows.Forms.AnchorStyles.Top;

            // card
            this.card.FillColor = System.Drawing.Color.FromArgb(0x12, 0x3A, 0x37);
            this.card.BorderColor = System.Drawing.Color.FromArgb(0x2E, 0x6B, 0x64);
            this.card.BorderThickness = 1;
            this.card.BorderRadius = 20;
            this.card.Size = new System.Drawing.Size(440, 360);
            this.card.Location = new System.Drawing.Point(280, 250);
            this.card.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.card.ShadowDecoration.Enabled = true;
            this.card.ShadowDecoration.Depth = 12;
            this.card.Controls.Add(this.lblUser);
            this.card.Controls.Add(this.pnlUser);
            this.card.Controls.Add(this.lblPass);
            this.card.Controls.Add(this.pnlPass);
            this.card.Controls.Add(this.btnLogin);
            this.card.Controls.Add(this.lblDefaults);
            this.card.Controls.Add(this.chipAdmin);
            this.card.Controls.Add(this.chipReception);

            // lblUser
            this.lblUser.Text = "اسم المستخدم";
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(0xBF, 0xD6, 0xD2);
            this.lblUser.Size = new System.Drawing.Size(376, 22);
            this.lblUser.Location = new System.Drawing.Point(32, 22);
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUser.BackColor = System.Drawing.Color.Transparent;

            // pnlUser
            this.pnlUser.BackColor = System.Drawing.Color.FromArgb(0x16, 0x40, 0x3B);
            this.pnlUser.Size = new System.Drawing.Size(376, 46);
            this.pnlUser.Location = new System.Drawing.Point(32, 48);
            this.pnlUser.Controls.Add(this.txtUser);
            this.pnlUser.Controls.Add(this.lblUserIcon);

            // lblUserIcon
            this.lblUserIcon.Text = "👤";
            this.lblUserIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblUserIcon.Width = 40;
            this.lblUserIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 12F);
            this.lblUserIcon.ForeColor = System.Drawing.Color.FromArgb(0xBF, 0xD6, 0xD2);
            this.lblUserIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblUserIcon.BackColor = System.Drawing.Color.Transparent;

            // txtUser
            this.txtUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUser.FillColor = System.Drawing.Color.FromArgb(0x16, 0x40, 0x3B);
            this.txtUser.ForeColor = System.Drawing.Color.White;
            this.txtUser.BorderRadius = 10;
            this.txtUser.BorderColor = System.Drawing.Color.FromArgb(0x2E, 0x6B, 0x64);
            this.txtUser.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUser.PlaceholderText = "admin أو reception";
            this.txtUser.PlaceholderForeColor = System.Drawing.Color.FromArgb(0x94, 0xA3, 0xB8);

            // lblPass
            this.lblPass.Text = "كلمة المرور";
            this.lblPass.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPass.ForeColor = System.Drawing.Color.FromArgb(0xBF, 0xD6, 0xD2);
            this.lblPass.Size = new System.Drawing.Size(376, 22);
            this.lblPass.Location = new System.Drawing.Point(32, 104);
            this.lblPass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPass.BackColor = System.Drawing.Color.Transparent;

            // pnlPass
            this.pnlPass.BackColor = System.Drawing.Color.FromArgb(0x16, 0x40, 0x3B);
            this.pnlPass.Size = new System.Drawing.Size(376, 46);
            this.pnlPass.Location = new System.Drawing.Point(32, 130);
            this.pnlPass.Controls.Add(this.txtPass);
            this.pnlPass.Controls.Add(this.lblPassIcon);

            // lblPassIcon
            this.lblPassIcon.Text = "🔒";
            this.lblPassIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPassIcon.Width = 40;
            this.lblPassIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 12F);
            this.lblPassIcon.ForeColor = System.Drawing.Color.FromArgb(0xBF, 0xD6, 0xD2);
            this.lblPassIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPassIcon.BackColor = System.Drawing.Color.Transparent;

            // txtPass
            this.txtPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPass.FillColor = System.Drawing.Color.FromArgb(0x16, 0x40, 0x3B);
            this.txtPass.ForeColor = System.Drawing.Color.White;
            this.txtPass.BorderRadius = 10;
            this.txtPass.BorderColor = System.Drawing.Color.FromArgb(0x2E, 0x6B, 0x64);
            this.txtPass.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPass.PasswordChar = '•';
            this.txtPass.PlaceholderText = "••••••••";
            this.txtPass.PlaceholderForeColor = System.Drawing.Color.FromArgb(0x94, 0xA3, 0xB8);

            // btnLogin
            this.btnLogin.Text = "تسجيل الدخول";
            this.btnLogin.FillColor = System.Drawing.Color.FromArgb(0x0F, 0x76, 0x6E);
            this.btnLogin.FillColor2 = System.Drawing.Color.FromArgb(0x0D, 0x94, 0x88);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.BorderRadius = 12;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Size = new System.Drawing.Size(376, 48);
            this.btnLogin.Location = new System.Drawing.Point(32, 190);
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);

            // lblDefaults
            this.lblDefaults.Text = "الحسابات الافتراضية";
            this.lblDefaults.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDefaults.ForeColor = System.Drawing.Color.FromArgb(0x94, 0xA3, 0xB8);
            this.lblDefaults.Size = new System.Drawing.Size(376, 22);
            this.lblDefaults.Location = new System.Drawing.Point(32, 250);
            this.lblDefaults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDefaults.BackColor = System.Drawing.Color.Transparent;

            // chipAdmin
            this.chipAdmin.FillColor = System.Drawing.Color.FromArgb(0x16, 0x40, 0x3B);
            this.chipAdmin.BorderColor = System.Drawing.Color.FromArgb(0x2E, 0x6B, 0x64);
            this.chipAdmin.BorderThickness = 1;
            this.chipAdmin.BorderRadius = 12;
            this.chipAdmin.Size = new System.Drawing.Size(182, 60);
            this.chipAdmin.Location = new System.Drawing.Point(226, 280);
            this.chipAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chipAdmin.Controls.Add(this.lblChipAdminRole);
            this.chipAdmin.Controls.Add(this.lblChipAdminCreds);

            this.lblChipAdminRole.Text = "مدير";
            this.lblChipAdminRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChipAdminRole.ForeColor = System.Drawing.Color.White;
            this.lblChipAdminRole.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChipAdminRole.Height = 28;
            this.lblChipAdminRole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblChipAdminRole.BackColor = System.Drawing.Color.Transparent;

            this.lblChipAdminCreds.Text = "admin / admin";
            this.lblChipAdminCreds.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChipAdminCreds.ForeColor = System.Drawing.Color.FromArgb(0xA9, 0xC7, 0xC3);
            this.lblChipAdminCreds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChipAdminCreds.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblChipAdminCreds.BackColor = System.Drawing.Color.Transparent;

            // chipReception
            this.chipReception.FillColor = System.Drawing.Color.FromArgb(0x16, 0x40, 0x3B);
            this.chipReception.BorderColor = System.Drawing.Color.FromArgb(0x2E, 0x6B, 0x64);
            this.chipReception.BorderThickness = 1;
            this.chipReception.BorderRadius = 12;
            this.chipReception.Size = new System.Drawing.Size(182, 60);
            this.chipReception.Location = new System.Drawing.Point(32, 280);
            this.chipReception.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chipReception.Controls.Add(this.lblChipRecRole);
            this.chipReception.Controls.Add(this.lblChipRecCreds);

            this.lblChipRecRole.Text = "استقبال";
            this.lblChipRecRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChipRecRole.ForeColor = System.Drawing.Color.White;
            this.lblChipRecRole.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChipRecRole.Height = 28;
            this.lblChipRecRole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblChipRecRole.BackColor = System.Drawing.Color.Transparent;

            this.lblChipRecCreds.Text = "reception / 1234";
            this.lblChipRecCreds.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChipRecCreds.ForeColor = System.Drawing.Color.FromArgb(0xA9, 0xC7, 0xC3);
            this.lblChipRecCreds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChipRecCreds.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblChipRecCreds.BackColor = System.Drawing.Color.Transparent;

            // Form1
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(0x0B, 0x1F, 0x1E);
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "Glory Gym - تسجيل الدخول";
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlIcon);
            this.Controls.Add(this.lblGymName);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.card);

            this.card.ResumeLayout(false);
            this.pnlUser.ResumeLayout(false);
            this.pnlPass.ResumeLayout(false);
            this.chipAdmin.ResumeLayout(false);
            this.chipReception.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
