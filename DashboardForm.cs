using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class DashboardForm : Form
    {
        private Button _activeNavButton;
        private static readonly Regex ArabicRegex = new Regex(@"[\u0600-\u06FF]", RegexOptions.Compiled);
        private Button _btnSignOut;

        public bool RequestSignOut { get; private set; }

        public DashboardForm()
        {
            InitializeComponent();
            ImageAssets.ApplyAppIcon(this);
            ThemeManager.ThemeChanged += OnGlobalThemeChanged;
            btnThemeToggle.Click += (_, __) => ThemeManager.Toggle();

            BuildBrandHeader();
            AddSignOutButton();
            ApplyDashboardDesignerTheme();
            UpdateThemeToggleGlyph();
            ApplyArabicTypography(this);
            AssignNavEvents();
            SetupNavPainting();
            _activeNavButton = btnNavHome;
            HighlightNavButton(btnNavHome);
            BuildDashboardHome();

            this.FormClosing += (s, e) =>
            {
                // Only prompt on a real application exit (not when signing out).
                if (!RequestSignOut)
                {
                    var answer = GunaUi.Show(
                        "هل تريد أخذ نسخة احتياطية لقاعدة البيانات قبل الخروج؟",
                        "إغلاق النظام", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (answer == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if (answer == DialogResult.Yes)
                        SettingsForm.BackupInteractive(this);
                }

                ThemeManager.ThemeChanged -= OnGlobalThemeChanged;
                GymDataStore.Save();
            };
            ApplyRoleVisibility();
        }

        private void OnGlobalThemeChanged(object sender, EventArgs e)
        {
            ApplyDashboardDesignerTheme();
            UpdateThemeToggleGlyph();
            BuildBrandHeader();
            BuildDashboardHome();
            StyleSignOutButton();
            if (_activeNavButton != null)
                HighlightNavButton(_activeNavButton);
            ApplyThemeToEmbeddedChild();
        }

        // ── theme application to the shell chrome ──────────
        private void ApplyDashboardDesignerTheme()
        {
            UiColorScheme t = ThemeManager.Current;
            BackColor = t.FormBackground;
            sidebar.BackColor = t.Sidebar;
            pnlBrand.BackColor = t.Sidebar;
            pnlSidebarFooter.BackColor = t.Sidebar;
            pnlSidebarSeparator.BackColor = t.BorderSubtle;
            pnlContentHost.BackColor = t.ContentHost;
            pnlDashboardHome.BackColor = t.ContentHost;

            btnThemeToggle.BackColor = t.Sidebar;
            btnThemeToggle.ForeColor = t.TextPrimary;
            btnThemeToggle.FlatAppearance.MouseOverBackColor = t.SidebarNavActive;
            btnThemeToggle.UseVisualStyleBackColor = false;

            foreach (Control c in sidebar.Controls)
            {
                if (c is Button b)
                    b.FlatAppearance.MouseOverBackColor = t.SidebarNavActive;
            }
            StyleSignOutButton();
        }

        private void UpdateThemeToggleGlyph()
        {
            btnThemeToggle.Text = ThemeManager.IsLight ? "🌙   الوضع الداكن" : "☀   الوضع الفاتح";
        }

        private void StyleSignOutButton()
        {
            if (_btnSignOut == null) return;
            UiColorScheme t = ThemeManager.Current;
            _btnSignOut.BackColor = t.Sidebar;
            _btnSignOut.ForeColor = FigmaPalette.Red;
            _btnSignOut.FlatAppearance.MouseOverBackColor = ThemeManager.IsLight
                ? Color.FromArgb(0xFE, 0xE2, 0xE2)   // red-100
                : Color.FromArgb(60, 30, 34);
        }

        private void ApplyThemeToEmbeddedChild()
        {
            foreach (Control c in pnlContentHost.Controls)
                if (c is IThemeAware th)
                    th.ApplyTheme(ThemeManager.Current);
        }

        // ── brand header (top of sidebar) ─────────────────
        private void BuildBrandHeader()
        {
            UiColorScheme t = ThemeManager.Current;
            pnlBrand.Controls.Clear();

            int iconSz = 52;
            int iconX = pnlBrand.Width - 16 - iconSz;
            int iconY = 22;

            var icon = new Panel
            {
                Size = new Size(iconSz, iconSz),
                Location = new Point(iconX, iconY),
                BackColor = Color.Transparent
            };
            icon.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, iconSz - 1, iconSz - 1);
                using (var path = RoundedRect(r, 14))
                using (var br = new LinearGradientBrush(r, FigmaPalette.LogoStart, FigmaPalette.LogoEnd, LinearGradientMode.ForwardDiagonal))
                    e.Graphics.FillPath(br, path);
                TextRenderer.DrawText(e.Graphics, "🏋️", new Font("Segoe UI Emoji", 18F),
                    new Rectangle(0, 0, iconSz, iconSz), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            var lblName = new Label
            {
                Text = "Glory Gym",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = t.TextPrimary,
                AutoSize = false,
                Size = new Size(iconX - 12, 26),
                Location = new Point(8, 28),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };
            var lblRole = new Label
            {
                Text = AppSession.IsReceptionist ? "موظف الاستقبال" : "المدير",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = FigmaPalette.Pink,
                AutoSize = false,
                Size = new Size(iconX - 12, 20),
                Location = new Point(8, 54),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            pnlBrand.Controls.Add(icon);
            pnlBrand.Controls.Add(lblName);
            pnlBrand.Controls.Add(lblRole);
        }

        // ── sign-out (sidebar footer) ──────────────────────
        private void AddSignOutButton()
        {
            _btnSignOut = new Button
            {
                Text = "🚪   تسجيل الخروج",
                Dock = DockStyle.Top,
                Height = 44,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 14, 0),
                Cursor = Cursors.Hand
            };
            _btnSignOut.FlatAppearance.BorderSize = 0;
            _btnSignOut.Click += (s, e) =>
            {
                if (GunaUi.Show("هل تريد تسجيل الخروج؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                RequestSignOut = true;
                Close();
            };
            pnlSidebarFooter.Controls.Add(_btnSignOut);
            _btnSignOut.BringToFront();
            StyleSignOutButton();
        }

        // ── navigation ────────────────────────────────────
        private void AssignNavEvents()
        {
            btnNavHome.Click          += (s, e) => ShowDashboardHome();
            btnNavMembers.Click       += (s, e) => ShowEmbeddedPage(new MembersForm(),       btnNavMembers,       "إدارة الأعضاء");
            btnNavSubs.Click          += (s, e) => ShowEmbeddedPage(new SubscriptionsForm(), btnNavSubs,          "خطط الاشتراكات");
            btnNavStore.Click         += (s, e) => ShowEmbeddedPage(new StoreForm(),         btnNavStore,         "متجر المكملات");
            btnNavTrainers.Click      += (s, e) => ShowEmbeddedPage(new TrainersForm(),      btnNavTrainers,      "إدارة المدربين");
            btnNavUsers.Click         += (s, e) => ShowEmbeddedPage(new UsersForm(),         btnNavUsers,         "إدارة المستخدمين");
            btnNavDiet.Click          += (s, e) => ShowEmbeddedPage(new DietPlanForm(),      btnNavDiet,          "خطط التغذية");
            btnNavReports.Click       += (s, e) => ShowEmbeddedPage(new ReportsForm(),       btnNavReports,       "التقارير المالية");
            btnNavNotifications.Click += (s, e) => ShowEmbeddedPage(new NotificationsForm(), btnNavNotifications, "الإشعارات");
            btnNavSettings.Click      += (s, e) => ShowEmbeddedPage(new SettingsForm(),      btnNavSettings,      "الإعدادات");
        }

        private void ApplyRoleVisibility()
        {
            if (AppSession.IsReceptionist)
            {
                btnNavHome.Visible          = false;
                btnNavSubs.Visible          = false;
                btnNavReports.Visible       = false;
                btnNavTrainers.Visible      = false;
                btnNavUsers.Visible         = false;
                btnNavNotifications.Visible = false;
                btnNavSettings.Visible      = false;
                ShowEmbeddedPage(new MembersForm(), btnNavMembers, "إدارة الأعضاء");
            }
        }

        private void ShowEmbeddedPage(Form childForm, Button navButton, string title)
        {
            pnlDashboardHome.Visible = false;
            pnlContentHost.Visible   = true;

            foreach (Control c in pnlContentHost.Controls)
                if (c is Form f) f.Close();
            pnlContentHost.Controls.Clear();

            UiColorScheme t = ThemeManager.Current;
            childForm.TopLevel        = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock            = DockStyle.Fill;
            childForm.BackColor       = t.ContentHost;

            pnlContentHost.Controls.Add(childForm);
            ApplyArabicTypography(childForm);
            childForm.Show();

            if (childForm is IThemeAware th)
                th.ApplyTheme(t);

            HighlightNavButton(navButton);
        }

        private void ShowDashboardHome()
        {
            foreach (Control c in pnlContentHost.Controls)
                if (c is Form f) f.Close();
            pnlContentHost.Controls.Clear();
            pnlContentHost.Visible   = false;
            pnlDashboardHome.Visible = true;

            HighlightNavButton(btnNavHome);
            BuildDashboardHome();
        }

        // ── active nav pill painting ───────────────────────
        private void SetupNavPainting()
        {
            foreach (Control c in sidebar.Controls)
            {
                if (c is Button b && b != btnThemeToggle)
                {
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.BorderSize = 0;
                    b.Paint += NavButton_Paint;
                }
            }
        }

        private void NavButton_Paint(object sender, PaintEventArgs e)
        {
            Button b = sender as Button;
            if (b == null || b != _activeNavButton) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int mx = 12, my = 5;
            Rectangle pill = new Rectangle(mx, my, b.Width - mx * 2, b.Height - my * 2);
            if (pill.Width <= 2 || pill.Height <= 2) return;

            using (var path = RoundedRect(pill, 12))
            using (var brush = new LinearGradientBrush(pill, FigmaPalette.GradientStart, FigmaPalette.GradientEnd, LinearGradientMode.Horizontal))
                g.FillPath(brush, path);

            Rectangle textR = new Rectangle(pill.X + 6, pill.Y, pill.Width - 22, pill.Height);
            TextRenderer.DrawText(g, b.Text, b.Font, textR, Color.White,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.RightToLeft | TextFormatFlags.NoPrefix);
        }

        private void HighlightNavButton(Button btn)
        {
            UiColorScheme t = ThemeManager.Current;
            foreach (Control c in sidebar.Controls)
                if (c is Button b && b != btn && b != btnThemeToggle)
                {
                    b.BackColor = t.SidebarNav;
                    b.ForeColor = t.TextPrimary;
                    b.FlatAppearance.MouseOverBackColor = t.SidebarNavActive;
                    b.Invalidate();
                }

            if (btn != null)
            {
                btn.BackColor = t.Sidebar;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.MouseOverBackColor = t.Sidebar;
                btn.Invalidate();
            }
            _activeNavButton = btn;
        }

        // ══════════════════════════════════════════════════
        //  DASHBOARD HOME (matches Figma: heading + 4 stat
        //  cards + gradient banner + two info panels)
        // ══════════════════════════════════════════════════
        private void BuildDashboardHome()
        {
            UiColorScheme t = ThemeManager.Current;
            pnlDashboardHome.SuspendLayout();
            pnlDashboardHome.Controls.Clear();
            pnlDashboardHome.Padding = new Padding(32, 24, 32, 24);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RightToLeft = RightToLeft.Yes,
                BackColor = Color.Transparent
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 84f));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120f));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 360f));

            root.Controls.Add(BuildHomeHeading(t), 0, 0);
            root.Controls.Add(BuildStatCardsRow(t), 0, 1);
            root.Controls.Add(BuildWelcomeBanner(t), 0, 2);
            root.Controls.Add(BuildInfoPanelsRow(t), 0, 3);

            pnlDashboardHome.Controls.Add(root);
            pnlDashboardHome.ResumeLayout(true);
        }

        private Panel BuildHomeHeading(UiColorScheme t)
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var title = new Label
            {
                Text = "مرحباً بك في Glory Gym 👋",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = t.TextPrimary,
                Dock = DockStyle.Top,
                Height = 44,
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };
            var sub = new Label
            {
                Text = "نظرة عامة على أداء صالتك الرياضية",
                Font = new Font("Segoe UI", 11F),
                ForeColor = t.TextMuted,
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };
            p.Controls.Add(sub);
            p.Controls.Add(title);
            return p;
        }

        private FlowLayoutPanel BuildStatCardsRow(UiColorScheme t)
        {
            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 6)
            };

            decimal monthRevenue = GymDataStore.StoreRevenueThisMonth() + GymDataStore.SubscriptionCashInThisMonth();

            flow.Controls.Add(BuildStatCard(t, "👥", "إجمالي الأعضاء",
                GymDataStore.Data.Members.Count.ToString("N0"), FigmaPalette.Blue,
                () => ShowEmbeddedPage(new MembersForm(), btnNavMembers, "إدارة الأعضاء")));
            flow.Controls.Add(BuildStatCard(t, "❗", "اشتراكات تنتهي قريباً",
                GymDataStore.MembersExpiringWithinDays(21).ToString("N0"), FigmaPalette.Orange,
                () => ShowEmbeddedPage(new SubscriptionsForm(), btnNavSubs, "خطط الاشتراكات")));
            flow.Controls.Add(BuildStatCard(t, "💲", "إيرادات الشهر",
                monthRevenue.ToString("N0") + " د.ل", FigmaPalette.Green,
                () => ShowEmbeddedPage(new ReportsForm(), btnNavReports, "التقارير المالية")));
            flow.Controls.Add(BuildStatCard(t, "🛒", "مبيعات اليوم",
                GymDataStore.StoreSalesToday().ToString("N0") + " د.ل", FigmaPalette.Purple,
                () => ShowEmbeddedPage(new StoreForm(), btnNavStore, "متجر المكملات")));

            return flow;
        }

        private Panel BuildStatCard(UiColorScheme t, string icon, string title, string value, Color accent, Action onClick)
        {
            int cardW = 246, cardH = 132;
            var card = new Panel
            {
                Size = new Size(cardW, cardH),
                BackColor = t.Card,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };
            StyleAsRoundedCard(card, t.BorderSubtle, 16);

            // green "نشط" status pill (top-right corner in RTL)
            var badge = new Label
            {
                Text = "● نشط",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = FigmaPalette.GreenBtn,
                BackColor = Color.FromArgb(0xDC, 0xFC, 0xE7),
                AutoSize = false,
                Size = new Size(58, 22),
                Location = new Point(16, 16),
                TextAlign = ContentAlignment.MiddleCenter
            };
            RoundLabel(badge, 11);

            // colored icon square (top-left in RTL)
            int badgeSz = 46;
            var iconPanel = new Panel
            {
                Size = new Size(badgeSz, badgeSz),
                Location = new Point(cardW - 16 - badgeSz, 14),
                BackColor = Color.Transparent
            };
            Color soft = Blend(accent, Color.White, 0.82f);
            iconPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, badgeSz - 1, badgeSz - 1);
                using (var path = RoundedRect(r, 12))
                using (var br = new SolidBrush(soft))
                    e.Graphics.FillPath(br, path);
                TextRenderer.DrawText(e.Graphics, icon, new Font("Segoe UI Emoji", 15F),
                    new Rectangle(0, 0, badgeSz, badgeSz), accent,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F),
                ForeColor = t.TextMuted,
                Size = new Size(cardW - 32, 24),
                Location = new Point(16, 72),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };
            var lblVal = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = t.TextPrimary,
                Size = new Size(cardW - 32, 36),
                Location = new Point(16, 92),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            card.Controls.Add(badge);
            card.Controls.Add(iconPanel);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblVal);

            Color normal = t.Card, hover = t.CardHover;
            foreach (Control c in EnumerateSelfAndChildren(card))
            {
                c.Cursor = Cursors.Hand;
                c.MouseEnter += (s, e) => { card.BackColor = hover; card.Invalidate(); };
                c.MouseLeave += (s, e) => { card.BackColor = normal; card.Invalidate(); };
                c.Click += (s, e) => onClick?.Invoke();
            }
            return card;
        }

        private Panel BuildWelcomeBanner(UiColorScheme t)
        {
            var banner = new Panel { Dock = DockStyle.Fill, Margin = new Padding(10, 8, 10, 8), BackColor = Color.Transparent };
            banner.Paint += (s, e) =>
            {
                var r = banner.ClientRectangle;
                if (r.Width <= 2 || r.Height <= 2) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, r.Width - 1, r.Height - 1), 18))
                using (var brush = new LinearGradientBrush(new Rectangle(0, 0, r.Width, r.Height),
                    FigmaPalette.GradientStart, FigmaPalette.GradientEnd, LinearGradientMode.Horizontal))
                {
                    brush.InterpolationColors = new ColorBlend(3)
                    {
                        Colors = new[] { FigmaPalette.GradientStart, FigmaPalette.GradientEnd, FigmaPalette.GradientStart },
                        Positions = new[] { 0f, 0.5f, 1f }
                    };
                    e.Graphics.FillPath(brush, path);
                }
                var titleR = new Rectangle(24, 22, r.Width - 48, 36);
                TextRenderer.DrawText(e.Graphics, "نظام إدارة متكامل 🏆", new Font("Segoe UI", 17F, FontStyle.Bold),
                    titleR, Color.White, TextFormatFlags.Right | TextFormatFlags.RightToLeft);
                var subR = new Rectangle(24, 60, r.Width - 48, 28);
                TextRenderer.DrawText(e.Graphics, "يمكنك إدارة الأعضاء، الاشتراكات، المتجر، المدربين، وخطط التغذية بكل سهولة من مكان واحد.",
                    new Font("Segoe UI", 11F), subR, Color.FromArgb(0xE6, 0xFF, 0xFA),
                    TextFormatFlags.Right | TextFormatFlags.RightToLeft);
            };
            return banner;
        }

        private TableLayoutPanel BuildInfoPanelsRow(UiColorScheme t)
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                RightToLeft = RightToLeft.Yes,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 4, 0, 4)
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            // خطط الاشتراك
            var plansRows = new List<Tuple<string, string>>();
            foreach (var p in GymDataStore.Data.SubscriptionPlans)
                plansRows.Add(Tuple.Create(p.Name, p.Price.ToString("0.##") + " د.ل"));
            row.Controls.Add(BuildInfoPanel(t, "خطط الاشتراك", "💲", FigmaPalette.Pink, plansRows), 0, 0);

            // إحصائيات سريعة
            var statRows = new List<Tuple<string, string>>
            {
                Tuple.Create("عدد المدربين", GymDataStore.Data.Trainers.Count.ToString("N0")),
                Tuple.Create("منتجات المتجر", GymDataStore.Data.StoreProducts.Count.ToString("N0")),
                Tuple.Create("خطط التغذية", GymDataStore.Data.FeedingPlans.Count.ToString("N0"))
            };
            row.Controls.Add(BuildInfoPanel(t, "إحصائيات سريعة", "📅", FigmaPalette.Blue, statRows), 1, 0);

            return row;
        }

        private Panel BuildInfoPanel(UiColorScheme t, string title, string icon, Color accent, List<Tuple<string, string>> rows)
        {
            var card = new Panel { Dock = DockStyle.Fill, BackColor = t.Card, Margin = new Padding(10, 0, 10, 0), Padding = new Padding(18) };
            StyleAsRoundedCard(card, t.BorderSubtle, 16);

            var body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = rows.Count,
                RightToLeft = RightToLeft.Yes,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 0)
            };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            foreach (var r in rows)
            {
                body.RowStyles.Add(new RowStyle(SizeType.Absolute, 62f));

                var rowPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = t.PanelElevated,
                    Margin = new Padding(0, 0, 0, 10)
                };
                StyleAsRoundedCard(rowPanel, t.BorderSubtle, 10);
                var name = new Label
                {
                    Text = r.Item1,
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = t.TextPrimary,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(0, 0, 14, 0),
                    BackColor = Color.Transparent
                };
                var val = new Label
                {
                    Text = r.Item2,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = accent,
                    Dock = DockStyle.Left,
                    Width = 120,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(14, 0, 0, 0),
                    BackColor = Color.Transparent
                };
                rowPanel.Controls.Add(name);
                rowPanel.Controls.Add(val);
                body.Controls.Add(rowPanel);
            }

            var header = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.Transparent };
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = t.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 44, 0),
                BackColor = Color.Transparent
            };
            int hb = 32;
            var hicon = new Panel { Size = new Size(hb, hb), Dock = DockStyle.Right, BackColor = Color.Transparent };
            hicon.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rr = new Rectangle(0, 4, hb - 1, hb - 1);
                using (var path = RoundedRect(rr, 9))
                using (var br = new LinearGradientBrush(rr, FigmaPalette.LogoStart, FigmaPalette.LogoEnd, LinearGradientMode.ForwardDiagonal))
                    e.Graphics.FillPath(br, path);
                TextRenderer.DrawText(e.Graphics, icon, new Font("Segoe UI Emoji", 11F),
                    new Rectangle(0, 4, hb, hb), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            header.Controls.Add(lblTitle);
            header.Controls.Add(hicon);

            card.Controls.Add(body);
            card.Controls.Add(header);
            return card;
        }

        private static IEnumerable<Control> EnumerateSelfAndChildren(Control root)
        {
            yield return root;
            foreach (Control child in root.Controls)
                foreach (Control c in EnumerateSelfAndChildren(child))
                    yield return c;
        }

        // ── Arabic typography ──────────────────────────────
        private void ApplyArabicTypography(Control root)
        {
            if (root == null) return;
            bool hasArabic = HasArabicText(root.Text);
            bool hasEmoji  = ContainsEmoji(root.Text);

            if (hasArabic) root.Font = CreateArabicFriendlyFont(root.Font, root.Text);

            if      (root is Label lb)      lb.UseCompatibleTextRendering = hasArabic && !hasEmoji;
            else if (root is ButtonBase bb) bb.UseCompatibleTextRendering = hasArabic && !hasEmoji;
            else if (root is DataGridView dgv)
            {
                var sampleSb = new StringBuilder();
                foreach (DataGridViewColumn col in dgv.Columns)
                    sampleSb.Append(col.HeaderText ?? "");
                string sample = sampleSb.ToString();
                dgv.DefaultCellStyle.Font              = CreateArabicFriendlyFont(dgv.DefaultCellStyle.Font ?? dgv.Font, sample);
                dgv.ColumnHeadersDefaultCellStyle.Font = CreateArabicFriendlyFont(dgv.ColumnHeadersDefaultCellStyle.Font ?? dgv.Font, sample);
            }

            foreach (Control child in root.Controls) ApplyArabicTypography(child);
        }

        private static bool HasArabicText(string t) => !string.IsNullOrWhiteSpace(t) && ArabicRegex.IsMatch(t);
        private static Font CreateArabicFriendlyFont(Font src, string text)
        {
            var fb = src ?? SystemFonts.DefaultFont;
            string fam = ContainsEmoji(text) ? "Segoe UI Emoji" : "Tahoma";
            return new Font(fam, fb.Size, fb.Style, fb.Unit, fb.GdiCharSet, fb.GdiVerticalFont);
        }
        private static bool ContainsEmoji(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            foreach (char c in text)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(c);
                if (cat == UnicodeCategory.OtherSymbol || cat == UnicodeCategory.Surrogate) return true;
            }
            return false;
        }

        // ── rounded helpers ────────────────────────────────
        internal static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            if (d > r.Width)  d = r.Width;
            if (d > r.Height) d = r.Height;
            var path = new GraphicsPath();
            if (d <= 0) { path.AddRectangle(r); path.CloseFigure(); return path; }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        internal static void StyleAsRoundedCard(Panel card, Color border, int radius)
        {
            void Apply()
            {
                if (card.Width <= 1 || card.Height <= 1) return;
                using (var p = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), radius))
                    card.Region = new Region(p);
            }
            Apply();
            card.SizeChanged += (s, e) => Apply();
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), radius))
                using (var pen = new Pen(border))
                    e.Graphics.DrawPath(pen, p);
            };
        }

        private static void RoundLabel(Label lbl, int radius)
        {
            void Apply()
            {
                if (lbl.Width <= 1 || lbl.Height <= 1) return;
                using (var p = RoundedRect(new Rectangle(0, 0, lbl.Width, lbl.Height), radius))
                    lbl.Region = new Region(p);
            }
            Apply();
            lbl.SizeChanged += (s, e) => Apply();
        }

        private static Color Blend(Color a, Color b, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            return Color.FromArgb(
                (int)(a.R * (1 - amount) + b.R * amount),
                (int)(a.G * (1 - amount) + b.G * amount),
                (int)(a.B * (1 - amount) + b.B * amount));
        }

        private void DashboardForm_Load(object sender, EventArgs e) { }
    }
}
