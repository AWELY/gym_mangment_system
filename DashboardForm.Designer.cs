namespace gym_mangment_system
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Sidebar (right) — brand header + nav + footer (theme/logout) ──
        private System.Windows.Forms.Panel sidebar;
        private System.Windows.Forms.Panel pnlBrand;
        private System.Windows.Forms.Button btnNavHome;
        private System.Windows.Forms.Button btnNavMembers;
        private System.Windows.Forms.Button btnNavSubs;
        private System.Windows.Forms.Button btnNavStore;
        private System.Windows.Forms.Button btnNavTrainers;
        private System.Windows.Forms.Button btnNavUsers;
        private System.Windows.Forms.Button btnNavDiet;
        private System.Windows.Forms.Button btnNavReports;
        private System.Windows.Forms.Button btnNavNotifications;
        private System.Windows.Forms.Button btnNavSettings;

        private System.Windows.Forms.Panel pnlSidebarFooter;
        private System.Windows.Forms.Panel pnlSidebarSeparator;
        private System.Windows.Forms.Button btnThemeToggle;

        // ── Content ──
        private System.Windows.Forms.Panel pnlContentHost;
        private System.Windows.Forms.Panel pnlDashboardHome;
        private System.Windows.Forms.FlowLayoutPanel flowStatCards;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.sidebar             = new System.Windows.Forms.Panel();
            this.pnlSidebarFooter    = new System.Windows.Forms.Panel();
            this.pnlSidebarSeparator = new System.Windows.Forms.Panel();
            this.btnThemeToggle      = new System.Windows.Forms.Button();
            this.btnNavSettings      = new System.Windows.Forms.Button();
            this.btnNavNotifications = new System.Windows.Forms.Button();
            this.btnNavReports       = new System.Windows.Forms.Button();
            this.btnNavDiet          = new System.Windows.Forms.Button();
            this.btnNavUsers         = new System.Windows.Forms.Button();
            this.btnNavTrainers      = new System.Windows.Forms.Button();
            this.btnNavStore         = new System.Windows.Forms.Button();
            this.btnNavSubs          = new System.Windows.Forms.Button();
            this.btnNavMembers       = new System.Windows.Forms.Button();
            this.btnNavHome          = new System.Windows.Forms.Button();
            this.pnlBrand            = new System.Windows.Forms.Panel();
            this.pnlContentHost      = new System.Windows.Forms.Panel();
            this.pnlDashboardHome    = new System.Windows.Forms.Panel();
            this.flowStatCards       = new System.Windows.Forms.FlowLayoutPanel();

            this.sidebar.SuspendLayout();
            this.pnlSidebarFooter.SuspendLayout();
            this.pnlDashboardHome.SuspendLayout();
            this.SuspendLayout();

            // ── sidebar ──
            this.sidebar.BackColor = System.Drawing.Color.White;
            // Add footer (bottom) first, then nav from bottom-most to top, then brand last (topmost).
            this.sidebar.Controls.Add(this.pnlSidebarFooter);
            this.sidebar.Controls.Add(this.btnNavSettings);
            this.sidebar.Controls.Add(this.btnNavNotifications);
            this.sidebar.Controls.Add(this.btnNavReports);
            this.sidebar.Controls.Add(this.btnNavDiet);
            this.sidebar.Controls.Add(this.btnNavUsers);
            this.sidebar.Controls.Add(this.btnNavTrainers);
            this.sidebar.Controls.Add(this.btnNavStore);
            this.sidebar.Controls.Add(this.btnNavSubs);
            this.sidebar.Controls.Add(this.btnNavMembers);
            this.sidebar.Controls.Add(this.btnNavHome);
            this.sidebar.Controls.Add(this.pnlBrand);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(264, 720);
            this.sidebar.TabIndex = 1;

            // ── brand header (filled at runtime: gradient icon + Glory Gym + role) ──
            this.pnlBrand.BackColor = System.Drawing.Color.White;
            this.pnlBrand.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBrand.Name = "pnlBrand";
            this.pnlBrand.Size = new System.Drawing.Size(264, 96);
            this.pnlBrand.TabIndex = 0;

            // ── nav buttons ──
            SetupNavBtn(this.btnNavHome,          "🏠   الرئيسية",        0);
            SetupNavBtn(this.btnNavMembers,       "👥   الأعضاء",         1);
            SetupNavBtn(this.btnNavSubs,          "💳   الاشتراكات",      2);
            SetupNavBtn(this.btnNavStore,         "🛒   المتجر",          3);
            SetupNavBtn(this.btnNavTrainers,      "🏋️   المدربين",       4);
            SetupNavBtn(this.btnNavUsers,         "👤   المستخدمين",      5);
            SetupNavBtn(this.btnNavDiet,          "🍎   خطط التغذية",     6);
            SetupNavBtn(this.btnNavReports,       "💲   التقارير المالية", 7);
            SetupNavBtn(this.btnNavNotifications,  "🔔   الإشعارات",       8);
            SetupNavBtn(this.btnNavSettings,      "⚙️   الإعدادات",       9);

            // ── sidebar footer (theme toggle + logout, added at runtime) ──
            this.pnlSidebarFooter.BackColor = System.Drawing.Color.White;
            this.pnlSidebarFooter.Controls.Add(this.btnThemeToggle);
            this.pnlSidebarFooter.Controls.Add(this.pnlSidebarSeparator);
            this.pnlSidebarFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSidebarFooter.Name = "pnlSidebarFooter";
            this.pnlSidebarFooter.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
            this.pnlSidebarFooter.Size = new System.Drawing.Size(264, 112);
            this.pnlSidebarFooter.TabIndex = 20;

            this.pnlSidebarSeparator.BackColor = System.Drawing.Color.FromArgb(0xE5, 0xE7, 0xEB);
            this.pnlSidebarSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarSeparator.Name = "pnlSidebarSeparator";
            this.pnlSidebarSeparator.Size = new System.Drawing.Size(240, 1);
            this.pnlSidebarSeparator.TabIndex = 0;

            this.btnThemeToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemeToggle.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnThemeToggle.FlatAppearance.BorderSize = 0;
            this.btnThemeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemeToggle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnThemeToggle.ForeColor = System.Drawing.Color.FromArgb(0x37, 0x41, 0x51);
            this.btnThemeToggle.Name = "btnThemeToggle";
            this.btnThemeToggle.Padding = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.btnThemeToggle.Size = new System.Drawing.Size(240, 44);
            this.btnThemeToggle.TabIndex = 1;
            this.btnThemeToggle.Text = "🌙   الوضع الداكن";
            this.btnThemeToggle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnThemeToggle.UseVisualStyleBackColor = false;

            // ── Content host (embedded pages) ──
            this.pnlContentHost.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentHost.Name     = "pnlContentHost";
            this.pnlContentHost.TabIndex = 12;
            this.pnlContentHost.Visible  = false;

            // ── Dashboard home ──
            this.pnlDashboardHome.AutoScroll = true;
            this.pnlDashboardHome.BackColor  = System.Drawing.Color.Transparent;
            this.pnlDashboardHome.Controls.Add(this.flowStatCards);
            this.pnlDashboardHome.Dock       = System.Windows.Forms.DockStyle.Fill;
            this.pnlDashboardHome.Name       = "pnlDashboardHome";
            this.pnlDashboardHome.Padding    = new System.Windows.Forms.Padding(32, 110, 32, 24);
            this.pnlDashboardHome.TabIndex   = 11;

            this.flowStatCards.AutoSize      = true;
            this.flowStatCards.AutoSizeMode  = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowStatCards.BackColor     = System.Drawing.Color.Transparent;
            this.flowStatCards.Dock          = System.Windows.Forms.DockStyle.Top;
            this.flowStatCards.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowStatCards.Name          = "flowStatCards";
            this.flowStatCards.Padding       = new System.Windows.Forms.Padding(0);
            this.flowStatCards.WrapContents  = true;
            this.flowStatCards.TabIndex      = 0;

            // ── Content container ──
            var contentPanel = new System.Windows.Forms.Panel();
            contentPanel.Controls.Add(this.pnlContentHost);
            contentPanel.Controls.Add(this.pnlDashboardHome);
            contentPanel.Dock     = System.Windows.Forms.DockStyle.Fill;
            contentPanel.Name     = "contentPanel";
            contentPanel.TabIndex = 2;

            // ── DashboardForm ──
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.FromArgb(0xF9, 0xFA, 0xFB);
            this.ClientSize          = new System.Drawing.Size(1200, 750);
            this.Controls.Add(contentPanel);
            this.Controls.Add(this.sidebar);
            this.MinimumSize         = new System.Drawing.Size(1000, 600);
            this.Name                = "DashboardForm";
            this.RightToLeft         = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition       = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text                = "Glory Gym — نظام إدارة الصالة الرياضية";
            this.WindowState         = System.Windows.Forms.FormWindowState.Maximized;
            this.Load               += new System.EventHandler(this.DashboardForm_Load);

            this.sidebar.ResumeLayout(false);
            this.pnlSidebarFooter.ResumeLayout(false);
            this.pnlDashboardHome.ResumeLayout(false);
            this.pnlDashboardHome.PerformLayout();
            this.ResumeLayout(false);
        }

        private void SetupNavBtn(System.Windows.Forms.Button btn, string text, int tabIndex)
        {
            btn.Cursor    = System.Windows.Forms.Cursors.Hand;
            btn.Dock      = System.Windows.Forms.DockStyle.Top;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0xF0, 0xFD, 0xFA);
            btn.FlatStyle  = System.Windows.Forms.FlatStyle.Flat;
            btn.Font       = new System.Drawing.Font("Segoe UI", 11.5F);
            btn.ForeColor  = System.Drawing.Color.FromArgb(0x37, 0x41, 0x51);
            btn.Padding    = new System.Windows.Forms.Padding(0, 0, 16, 0);
            btn.Size       = new System.Drawing.Size(264, 48);
            btn.TabIndex   = tabIndex;
            btn.Text       = text;
            btn.TextAlign  = System.Drawing.ContentAlignment.MiddleRight;
            btn.UseVisualStyleBackColor = false;
        }
    }
}
