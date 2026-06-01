namespace gym_mangment_system
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel sidebar;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Button btnNavHome;
        private System.Windows.Forms.Button btnNavMembers;
        private System.Windows.Forms.Button btnNavSubs;
        private System.Windows.Forms.Button btnNavStore;
        private System.Windows.Forms.Button btnNavDiet;
        private System.Windows.Forms.Button btnNavReports;
        private System.Windows.Forms.Button btnNavTrainers;
        private System.Windows.Forms.Button btnNavUsers;

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblDashTitle;
        private System.Windows.Forms.Button btnThemeToggle;
        private System.Windows.Forms.Button btnNotifications;
        private System.Windows.Forms.Panel pnlNotifDropdown;
        private System.Windows.Forms.Label lblNotifHeader;
        private System.Windows.Forms.FlowLayoutPanel flowNotifications;

        // ── Dashboard home panel (stat cards only) ──
        private System.Windows.Forms.Panel pnlDashboardHome;
        private System.Windows.Forms.FlowLayoutPanel flowStatCards;

        // ── Content host panel (for embedded pages) ──
        private System.Windows.Forms.Panel pnlContentHost;

        private System.Windows.Forms.Panel statusBar;
        private System.Windows.Forms.Label lblStatusLeft;
        private System.Windows.Forms.Label lblStatusCenter;
        private System.Windows.Forms.Label lblStatusRight;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.sidebar                = new System.Windows.Forms.Panel();
            this.btnNavReports          = new System.Windows.Forms.Button();
            this.btnNavTrainers         = new System.Windows.Forms.Button();
            this.btnNavUsers            = new System.Windows.Forms.Button();
            this.btnNavDiet             = new System.Windows.Forms.Button();
            this.btnNavStore            = new System.Windows.Forms.Button();
            this.btnNavSubs             = new System.Windows.Forms.Button();
            this.btnNavMembers          = new System.Windows.Forms.Button();
            this.btnNavHome             = new System.Windows.Forms.Button();
            this.pnlLogo                = new System.Windows.Forms.Panel();
            this.lblLogo                = new System.Windows.Forms.Label();
            this.topBar                 = new System.Windows.Forms.Panel();
            this.btnThemeToggle         = new System.Windows.Forms.Button();
            this.btnNotifications       = new System.Windows.Forms.Button();
            this.lblDashTitle           = new System.Windows.Forms.Label();
            this.pnlNotifDropdown       = new System.Windows.Forms.Panel();
            this.flowNotifications      = new System.Windows.Forms.FlowLayoutPanel();
            this.lblNotifHeader         = new System.Windows.Forms.Label();
            this.pnlContentHost         = new System.Windows.Forms.Panel();
            this.pnlDashboardHome       = new System.Windows.Forms.Panel();
            this.flowStatCards          = new System.Windows.Forms.FlowLayoutPanel();
            this.statusBar              = new System.Windows.Forms.Panel();
            this.lblStatusCenter        = new System.Windows.Forms.Label();
            this.lblStatusRight         = new System.Windows.Forms.Label();
            this.lblStatusLeft          = new System.Windows.Forms.Label();

            this.sidebar.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.topBar.SuspendLayout();
            this.pnlNotifDropdown.SuspendLayout();
            this.pnlDashboardHome.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();

            // ── sidebar ──
            this.sidebar.BackColor = System.Drawing.Color.FromArgb(22, 22, 26);
            this.sidebar.Controls.Add(this.btnNavUsers);
            this.sidebar.Controls.Add(this.btnNavTrainers);
            this.sidebar.Controls.Add(this.btnNavReports);
            this.sidebar.Controls.Add(this.btnNavDiet);
            this.sidebar.Controls.Add(this.btnNavStore);
            this.sidebar.Controls.Add(this.btnNavSubs);
            this.sidebar.Controls.Add(this.btnNavMembers);
            this.sidebar.Controls.Add(this.btnNavHome);
            this.sidebar.Controls.Add(this.pnlLogo);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(250, 720);
            this.sidebar.TabIndex = 1;

            // nav buttons helper
            SetupNavBtn(this.btnNavUsers,    "👤  المستخدمون", 7);
            SetupNavBtn(this.btnNavTrainers, "🏋️  المدربون",   6);
            SetupNavBtn(this.btnNavReports,  "📊  المالية",    5);
            SetupNavBtn(this.btnNavDiet,     "🍏  التغذية",    4);
            SetupNavBtn(this.btnNavStore,    "🛒  المتجر",     3);
            SetupNavBtn(this.btnNavSubs,     "📋  الاشتراكات", 2);
            SetupNavBtn(this.btnNavMembers,  "👥  الأعضاء",    1);

            this.btnNavHome.BackColor = System.Drawing.Color.FromArgb(40, 40, 48);
            this.btnNavHome.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnNavHome.Dock      = System.Windows.Forms.DockStyle.Top;
            this.btnNavHome.FlatAppearance.BorderSize = 0;
            this.btnNavHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40,40,48);
            this.btnNavHome.FlatStyle  = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavHome.Font       = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnNavHome.ForeColor  = System.Drawing.Color.White;
            this.btnNavHome.Name       = "btnNavHome";
            this.btnNavHome.Padding    = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnNavHome.Size       = new System.Drawing.Size(250, 60);
            this.btnNavHome.TabIndex   = 0;
            this.btnNavHome.Text       = "🏠  الرئيسية";
            this.btnNavHome.TextAlign  = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNavHome.UseVisualStyleBackColor = false;

            // pnlLogo
            this.pnlLogo.Controls.Add(this.lblLogo);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(250, 100);
            this.pnlLogo.TabIndex = 6;

            this.lblLogo.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblLogo.Name      = "lblLogo";
            this.lblLogo.Size      = new System.Drawing.Size(250, 100);
            this.lblLogo.TabIndex  = 0;
            this.lblLogo.Text      = "🏋️ GLORY GYM\r\nنظام إدارة الصالة";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── topBar ──
            this.topBar.BackColor = System.Drawing.Color.FromArgb(26, 26, 30);
            this.topBar.Controls.Add(this.lblDashTitle);
            this.topBar.Controls.Add(this.btnThemeToggle);
            this.topBar.Controls.Add(this.btnNotifications);
            this.topBar.Dock     = System.Windows.Forms.DockStyle.Top;
            this.topBar.Name     = "topBar";
            this.topBar.Size     = new System.Drawing.Size(950, 55);
            this.topBar.TabIndex = 10;

            this.btnNotifications.Cursor  = System.Windows.Forms.Cursors.Hand;
            this.btnNotifications.FlatAppearance.BorderSize = 0;
            this.btnNotifications.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 50);
            this.btnNotifications.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotifications.Font     = new System.Drawing.Font("Segoe UI", 16F);
            this.btnNotifications.ForeColor= System.Drawing.Color.White;
            this.btnNotifications.Location = new System.Drawing.Point(12, 6);
            this.btnNotifications.Name     = "btnNotifications";
            this.btnNotifications.Size     = new System.Drawing.Size(50, 42);
            this.btnNotifications.TabIndex = 1;
            this.btnNotifications.Text     = "🔔";
            this.btnNotifications.UseVisualStyleBackColor = true;

            this.btnThemeToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemeToggle.FlatAppearance.BorderSize = 0;
            this.btnThemeToggle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 50);
            this.btnThemeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemeToggle.Font = new System.Drawing.Font("Segoe UI Emoji", 14F);
            this.btnThemeToggle.ForeColor = System.Drawing.Color.White;
            this.btnThemeToggle.Location = new System.Drawing.Point(66, 6);
            this.btnThemeToggle.Name = "btnThemeToggle";
            this.btnThemeToggle.Size = new System.Drawing.Size(44, 42);
            this.btnThemeToggle.TabIndex = 3;
            this.btnThemeToggle.Text = "☀";
            this.btnThemeToggle.UseVisualStyleBackColor = false;

            this.lblDashTitle.Anchor    = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblDashTitle.AutoSize  = true;
            this.lblDashTitle.Font      = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblDashTitle.ForeColor = System.Drawing.Color.White;
            this.lblDashTitle.Location  = new System.Drawing.Point(570, 10);
            this.lblDashTitle.Name      = "lblDashTitle";
            this.lblDashTitle.Size      = new System.Drawing.Size(315, 37);
            this.lblDashTitle.TabIndex  = 0;
            this.lblDashTitle.Text      = "لوحة التحكم (Dashboard)";

            // ── Notifications ──
            this.pnlNotifDropdown.BackColor = System.Drawing.Color.FromArgb(32, 32, 38);
            this.pnlNotifDropdown.Controls.Add(this.flowNotifications);
            this.pnlNotifDropdown.Controls.Add(this.lblNotifHeader);
            this.pnlNotifDropdown.Location  = new System.Drawing.Point(12, 55);
            this.pnlNotifDropdown.Name      = "pnlNotifDropdown";
            this.pnlNotifDropdown.Padding   = new System.Windows.Forms.Padding(1);
            this.pnlNotifDropdown.Size      = new System.Drawing.Size(380, 280);
            this.pnlNotifDropdown.TabIndex  = 20;
            this.pnlNotifDropdown.Visible   = false;

            this.flowNotifications.AutoScroll    = true;
            this.flowNotifications.BackColor     = System.Drawing.Color.FromArgb(32, 32, 38);
            this.flowNotifications.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.flowNotifications.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowNotifications.Name          = "flowNotifications";
            this.flowNotifications.Padding       = new System.Windows.Forms.Padding(5);
            this.flowNotifications.Size          = new System.Drawing.Size(378, 236);
            this.flowNotifications.TabIndex      = 1;
            this.flowNotifications.WrapContents  = false;

            this.lblNotifHeader.BackColor  = System.Drawing.Color.FromArgb(38, 38, 45);
            this.lblNotifHeader.Dock       = System.Windows.Forms.DockStyle.Top;
            this.lblNotifHeader.Font       = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblNotifHeader.ForeColor  = System.Drawing.Color.White;
            this.lblNotifHeader.Name       = "lblNotifHeader";
            this.lblNotifHeader.Padding    = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.lblNotifHeader.Size       = new System.Drawing.Size(378, 42);
            this.lblNotifHeader.TabIndex   = 0;
            this.lblNotifHeader.Text       = "🔔  التنبيهات";
            this.lblNotifHeader.TextAlign  = System.Drawing.ContentAlignment.MiddleRight;

            // ── Content host ──
            this.pnlContentHost.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentHost.Name     = "pnlContentHost";
            this.pnlContentHost.TabIndex = 12;
            this.pnlContentHost.Visible  = false;

            // ── Dashboard home (just a flow of stat cards) ──
            this.pnlDashboardHome.BackColor = System.Drawing.Color.Transparent;
            this.pnlDashboardHome.Controls.Add(this.flowStatCards);
            this.pnlDashboardHome.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.pnlDashboardHome.Name      = "pnlDashboardHome";
            this.pnlDashboardHome.Padding   = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.pnlDashboardHome.TabIndex  = 11;

            this.flowStatCards.AutoScroll    = true;
            this.flowStatCards.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.flowStatCards.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowStatCards.Name          = "flowStatCards";
            this.flowStatCards.Padding       = new System.Windows.Forms.Padding(5);
            this.flowStatCards.WrapContents  = true;
            this.flowStatCards.TabIndex      = 0;

            // ── Status bar ──
            this.statusBar.BackColor = System.Drawing.Color.FromArgb(18, 18, 22);
            this.statusBar.Controls.Add(this.lblStatusCenter);
            this.statusBar.Controls.Add(this.lblStatusRight);
            this.statusBar.Controls.Add(this.lblStatusLeft);
            this.statusBar.Dock     = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Name     = "statusBar";
            this.statusBar.Size     = new System.Drawing.Size(1200, 30);
            this.statusBar.TabIndex = 3;

            this.lblStatusCenter.Anchor   = System.Windows.Forms.AnchorStyles.Top;
            this.lblStatusCenter.AutoSize = true;
            this.lblStatusCenter.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusCenter.ForeColor= System.Drawing.Color.FromArgb(120, 120, 130);
            this.lblStatusCenter.Location = new System.Drawing.Point(553, 6);
            this.lblStatusCenter.Name     = "lblStatusCenter";
            this.lblStatusCenter.TabIndex = 2;
            this.lblStatusCenter.Text     = "التاريخ والوقت";

            this.lblStatusRight.AutoSize  = true;
            this.lblStatusRight.Dock      = System.Windows.Forms.DockStyle.Right;
            this.lblStatusRight.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusRight.ForeColor = System.Drawing.Color.FromArgb(120, 120, 130);
            this.lblStatusRight.Name      = "lblStatusRight";
            this.lblStatusRight.Padding   = new System.Windows.Forms.Padding(0, 7, 10, 0);
            this.lblStatusRight.TabIndex  = 1;
            this.lblStatusRight.Text      = "اسم المستخدم: المدير";

            this.lblStatusLeft.AutoSize  = true;
            this.lblStatusLeft.Dock      = System.Windows.Forms.DockStyle.Left;
            this.lblStatusLeft.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusLeft.ForeColor = System.Drawing.Color.FromArgb(0, 166, 62);
            this.lblStatusLeft.Name      = "lblStatusLeft";
            this.lblStatusLeft.Padding   = new System.Windows.Forms.Padding(10, 7, 0, 0);
            this.lblStatusLeft.TabIndex  = 0;
            this.lblStatusLeft.Text      = "● متصل : WhatsApp API";

            // ── Main content panel ──
            var contentPanel = new System.Windows.Forms.Panel();
            contentPanel.Controls.Add(this.pnlNotifDropdown);
            contentPanel.Controls.Add(this.pnlContentHost);
            contentPanel.Controls.Add(this.pnlDashboardHome);
            contentPanel.Controls.Add(this.topBar);
            contentPanel.Dock     = System.Windows.Forms.DockStyle.Fill;
            contentPanel.Name     = "contentPanel";
            contentPanel.TabIndex = 2;

            // ── DashboardForm ──
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.FromArgb(18, 18, 22);
            this.ClientSize          = new System.Drawing.Size(1200, 750);
            this.Controls.Add(contentPanel);
            this.Controls.Add(this.sidebar);
            this.Controls.Add(this.statusBar);
            this.MinimumSize         = new System.Drawing.Size(1000, 600);
            this.Name                = "DashboardForm";
            this.RightToLeft         = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition       = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text                = "Gym Management System";
            this.WindowState         = System.Windows.Forms.FormWindowState.Maximized;
            this.Load               += new System.EventHandler(this.DashboardForm_Load);

            this.sidebar.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.pnlNotifDropdown.ResumeLayout(false);
            this.pnlDashboardHome.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
        }

        private void SetupNavBtn(System.Windows.Forms.Button btn, string text, int tabIndex)
        {
            btn.Cursor    = System.Windows.Forms.Cursors.Hand;
            btn.Dock      = System.Windows.Forms.DockStyle.Top;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 48);
            btn.FlatStyle  = System.Windows.Forms.FlatStyle.Flat;
            btn.Font       = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            btn.ForeColor  = System.Drawing.Color.FromArgb(160, 160, 170);
            btn.Padding    = new System.Windows.Forms.Padding(0, 0, 15, 0);
            btn.Size       = new System.Drawing.Size(250, 60);
            btn.TabIndex   = tabIndex;
            btn.Text       = text;
            btn.TextAlign  = System.Drawing.ContentAlignment.MiddleRight;
            btn.UseVisualStyleBackColor = true;
        }
    }
}
