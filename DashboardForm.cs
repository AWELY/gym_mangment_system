using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class DashboardForm : Form
    {
        private Button _activeNavButton;
        private static readonly Regex ArabicRegex = new Regex(@"[\u0600-\u06FF]", RegexOptions.Compiled);
        private Timer  _commercialTicker;
        private Label  _commercialLabel;
        private Timer  _statusClock;
        private Panel  _pnlDashboardQuick;
        private Button _btnSignOut;
        private ListBox _lstQuickMembers;
        private ListBox _lstQuickStock;
        private ListBox _lstQuickSales;
        private const string CommercialText = "أهلاً بكم في نادي غلوري، هنا تُصنع القوة وتولد الأساطير، انطلق اليوم نحو نسختك الأقوى.";
        public bool RequestSignOut { get; private set; }

        public DashboardForm()
        {
            InitializeComponent();
            ThemeManager.ThemeChanged += OnGlobalThemeChanged;
            btnThemeToggle.Click += (_, __) => ThemeManager.Toggle();

            ApplyDashboardDesignerTheme();
            UpdateThemeToggleGlyph();
            ApplyBrandImages();
            ApplyArabicTypography(this);
            AssignNavEvents();
            _activeNavButton = btnNavHome;
            HighlightNavButton(btnNavHome);
            BuildDashboardQuickPanel();
            RefreshDashboardHomeData();
            AddSignOutButton();
            StyleSignOutButton();
            InitializeCommercialBanner();
            lblStatusRight.Text = "اسم المستخدم: " + AppSession.Username;
            _statusClock = new Timer { Interval = 1000 };
            _statusClock.Tick += (_, __) => lblStatusCenter.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
            _statusClock.Start();
            this.FormClosing += (s, e) =>
            {
                ThemeManager.ThemeChanged -= OnGlobalThemeChanged;
                if (_commercialTicker != null) _commercialTicker.Stop();
                if (_statusClock != null) _statusClock.Stop();
                GymDataStore.Save();
            };
            ApplyRoleVisibility();
        }

        private void OnGlobalThemeChanged(object sender, EventArgs e)
        {
            ApplyDashboardDesignerTheme();
            UpdateThemeToggleGlyph();
            ApplyBrandImages();
            RebuildDashboardQuickPanel();
            RefreshDashboardHomeData();
            BuildNotificationItems();
            StyleSignOutButton();
            if (_activeNavButton != null)
                HighlightNavButton(_activeNavButton);
            ApplyThemeToEmbeddedChild();
            EnsureTopBarInteractiveZOrder();
        }

        private void RebuildDashboardQuickPanel()
        {
            DisposeDashboardQuickPanel();
            BuildDashboardQuickPanel();
            RefreshQuickLists();
        }

        private void DisposeDashboardQuickPanel()
        {
            if (_pnlDashboardQuick == null) return;
            pnlDashboardHome.Controls.Remove(_pnlDashboardQuick);
            _pnlDashboardQuick.Dispose();
            _pnlDashboardQuick = null;
            _lstQuickMembers = null;
            _lstQuickStock = null;
            _lstQuickSales = null;
        }

        private void ApplyDashboardDesignerTheme()
        {
            UiColorScheme t = ThemeManager.Current;
            BackColor = t.FormBackground;
            sidebar.BackColor = t.Sidebar;
            topBar.BackColor = t.TopBar;
            lblDashTitle.ForeColor = t.TextPrimary;
            btnNotifications.BackColor = t.TopBar;
            btnNotifications.ForeColor = t.TextPrimary;
            btnNotifications.FlatAppearance.MouseOverBackColor = t.TopBarButtonHover;
            btnNotifications.UseVisualStyleBackColor = false;
            btnThemeToggle.BackColor = t.TopBar;
            btnThemeToggle.ForeColor = t.TextPrimary;
            btnThemeToggle.FlatAppearance.MouseOverBackColor = t.TopBarButtonHover;
            statusBar.BackColor = t.StatusBar;
            lblStatusCenter.ForeColor = t.TextMuted;
            lblStatusRight.ForeColor = t.TextMuted;
            pnlContentHost.BackColor = t.ContentHost;
            pnlNotifDropdown.BackColor = t.Panel;
            flowNotifications.BackColor = t.Panel;
            lblNotifHeader.BackColor = t.PanelElevated;
            lblNotifHeader.ForeColor = t.TextPrimary;

            foreach (Control c in sidebar.Controls)
            {
                if (c is Button b)
                    b.FlatAppearance.MouseOverBackColor = t.SidebarNavActive;
            }
            EnsureTopBarInteractiveZOrder();
        }

        private void UpdateThemeToggleGlyph()
        {
            btnThemeToggle.Text = ThemeManager.IsLight ? "🌙" : "☀";
        }

        private void StyleSignOutButton()
        {
            if (_btnSignOut == null) return;
            UiColorScheme t = ThemeManager.Current;
            _btnSignOut.BackColor = t.SecondaryButton;
            _btnSignOut.ForeColor = ThemeManager.IsLight ? t.TextPrimary : t.TextOnAccent;
            _btnSignOut.FlatAppearance.MouseOverBackColor = t.SecondaryButtonHover;
        }

        private void ApplyThemeToEmbeddedChild()
        {
            foreach (Control c in pnlContentHost.Controls)
            {
                if (c is IThemeAware th)
                    th.ApplyTheme(ThemeManager.Current);
            }
        }

        // ── branding ──────────────────────────────────────
        private void ApplyBrandImages()
        {
            float dashOp = ThemeManager.BrandingOpacity(isDashboard: true);
            float homeOp = ThemeManager.IsLight ? 0.28f : 0.60f;
            Image bg = ImageAssets.TryLoadToughBackground("dashboard", dashOp);
            if (bg != null)
            {
                this.BackgroundImage = bg;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                pnlDashboardHome.BackgroundImage = ImageAssets.TryLoadToughBackground("dashboard-home", homeOp);
                pnlDashboardHome.BackgroundImageLayout = ImageLayout.Stretch;
            }

            Image logo = ImageAssets.TryLoad(ImageAssets.LogoHeartDumbbell);
            if (logo != null)
            {
                pnlLogo.BackgroundImage = logo;
                pnlLogo.BackgroundImageLayout = ImageLayout.Zoom;
                lblLogo.Text = string.Empty;
                lblLogo.BackColor = Color.Transparent;
            }
        }

        // ── dashboard home: stats + quick lists (live from GymDataStore) ──
        private void RefreshDashboardHomeData()
        {
            if (!pnlDashboardHome.Visible && AppSession.IsReceptionist) return;

            flowStatCards.Controls.Clear();
            UiColorScheme t = ThemeManager.Current;

            int activeSubs = GymDataStore.Data.Members.Count(m => !string.IsNullOrWhiteSpace(m.PlanName));
            decimal monthTotal = GymDataStore.StoreRevenueThisMonth() + GymDataStore.SubscriptionCashInThisMonth();

            var statSpecs = new (string Icon, string Title, string Value, Color Accent)[]
            {
                ("👥", "إجمالي الأعضاء",         GymDataStore.Data.Members.Count.ToString("N0"),                    Color.FromArgb(33,  150, 243)),
                ("✅", "اشتراك مسجّل",           activeSubs.ToString("N0"),                                         Color.FromArgb(76,  175,  80)),
                ("⚠️", "تنتهي خلال 21 يوماً", GymDataStore.MembersExpiringWithinDays(21).ToString("N0"),        Color.FromArgb(255, 152,   0)),
                ("💰", "إيرادات الشهر (تقدير)", monthTotal.ToString("N0") + " $",                                  Color.FromArgb(76,  175,  80)),
                ("🛒", "مبيعات المتجر اليوم",   GymDataStore.StoreSalesToday().ToString("N0") + " $",               Color.FromArgb(156,  39, 176)),
                ("🏋️", "المدربون",              GymDataStore.Data.Trainers.Count.ToString("N0"),                   Color.FromArgb(33,  150, 243)),
            };

            int cardW = 240, cardH = 130;
            foreach (var (icon, title, value, accent) in statSpecs)
            {
                Panel card = new Panel
                {
                    Size      = new Size(cardW, cardH),
                    BackColor = t.Card,
                    Margin    = new Padding(12),
                    Cursor    = Cursors.Hand
                };

                Panel strip = new Panel { Size = new Size(5, cardH), BackColor = accent, Dock = DockStyle.Left };

                Label lblIcon = new Label
                {
                    Text      = icon,
                    Font      = new Font("Segoe UI Emoji", 24F),
                    Size      = new Size(60, 60),
                    Location  = new Point(cardW - 65, (cardH - 60) / 2),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };

                Label lblVal = new Label
                {
                    Text      = value,
                    Font      = new Font("Segoe UI", 22F, FontStyle.Bold),
                    ForeColor = accent,
                    Size      = new Size(cardW - 80, 55),
                    Location  = new Point(10, 30),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent
                };

                Label lblTitle = new Label
                {
                    Text      = title,
                    Font      = new Font("Segoe UI", 10F),
                    ForeColor = t.TextMuted,
                    Size      = new Size(cardW - 80, 24),
                    Location  = new Point(10, 86),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent
                };

                Color hover = t.CardHover;
                Color normal = t.Card;
                card.MouseEnter += (s, e) => card.BackColor = hover;
                card.MouseLeave += (s, e) => card.BackColor = normal;
                foreach (Control c in new Control[] { lblIcon, lblVal, lblTitle })
                {
                    c.MouseEnter += (s, e) => card.BackColor = hover;
                    c.MouseLeave += (s, e) => card.BackColor = normal;
                }

                card.Controls.Add(strip);
                card.Controls.Add(lblIcon);
                card.Controls.Add(lblVal);
                card.Controls.Add(lblTitle);
                flowStatCards.Controls.Add(card);
            }

            RefreshQuickLists();
        }

        private void BuildDashboardQuickPanel()
        {
            UiColorScheme t = ThemeManager.Current;
            _pnlDashboardQuick = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 280,
                BackColor = t.QuickPanel,
                Padding   = new Padding(16, 10, 16, 10)
            };

            var lblQuick = new Label
            {
                Text      = "⚡ إجراءات سريعة وقوائم",
                Dock      = DockStyle.Top,
                Height    = 28,
                ForeColor = t.TextPrimary,
                Font      = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            var flowBtns = new FlowLayoutPanel
            {
                Dock            = DockStyle.Top,
                Height          = 44,
                FlowDirection   = FlowDirection.RightToLeft,
                WrapContents    = false,
                Padding         = new Padding(0, 0, 0, 6),
                BackColor       = t.QuickPanel
            };

            void AddQuickBtn(string text, Action onClick)
            {
                var b = new Button
                {
                    Text      = text,
                    AutoSize  = true,
                    Padding   = new Padding(14, 6, 14, 6),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = t.SecondaryButton,
                    ForeColor = t.TextOnAccent,
                    Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor    = Cursors.Hand,
                    Margin    = new Padding(6, 0, 0, 0)
                };
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = t.SecondaryButtonHover;
                b.Click += (_, __) => onClick();
                flowBtns.Controls.Add(b);
            }

            if (AppSession.IsReceptionist)
            {
                AddQuickBtn("➕ عضو", () => ShowEmbeddedPage(new MembersForm(true), btnNavMembers, "👥  إدارة الأعضاء"));
                AddQuickBtn("🛒 منتج", () => ShowEmbeddedPage(new StoreForm(true), btnNavStore, "🛒  المتجر (POS)"));
                AddQuickBtn("🍏 تغذية", () => ShowEmbeddedPage(new DietPlanForm(), btnNavDiet, "🍏  التغذية"));
            }
            else
            {
                AddQuickBtn("➕ عضو", () => ShowEmbeddedPage(new MembersForm(true), btnNavMembers, "👥  إدارة الأعضاء"));
                AddQuickBtn("🛒 منتج", () => ShowEmbeddedPage(new StoreForm(true), btnNavStore, "🛒  المتجر (POS)"));
                AddQuickBtn("📋 اشتراك", () => ShowEmbeddedPage(new SubscriptionsForm(true), btnNavSubs, "📋  الاشتراكات"));
                AddQuickBtn("🏋️ مدرب", () => ShowEmbeddedPage(new TrainersForm(true), btnNavTrainers, "🏋️  إدارة المدربين"));
            }

            var hdr = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 34,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = t.QuickPanel,
                RightToLeft = RightToLeft.Yes,
                Padding = new Padding(4, 0, 4, 4)
            };
            hdr.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34f));
            hdr.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            hdr.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));

            Label MakeHdrLabel(string text, Action onClick)
            {
                var l = new Label
                {
                    Text = text,
                    Dock = DockStyle.Fill,
                    BackColor = t.TableHeaderBand,
                    ForeColor = ThemeManager.IsLight ? t.TextPrimary : t.TextOnAccent,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(8, 0, 8, 0),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 0, 6, 0)
                };
                l.Click += (_, __) => onClick();
                return l;
            }

            var tbl = new TableLayoutPanel
            {
                Dock              = DockStyle.Fill,
                ColumnCount       = 3,
                RowCount          = 1,
                BackColor         = t.QuickPanel,
                RightToLeft       = RightToLeft.Yes
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));

            Panel MakeListColumn(out ListBox lb)
            {
                var p = new Panel { Dock = DockStyle.Fill, Padding = new Padding(4), BackColor = Color.Transparent };
                lb = new ListBox
                {
                    Dock = DockStyle.Fill,
                    BackColor = t.ListBackground,
                    ForeColor = t.ListForeground,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 10F),
                    IntegralHeight = false,
                    ItemHeight = 22
                };

                p.Controls.Add(lb);
                return p;
            }

            hdr.Controls.Add(MakeHdrLabel("آخر الأعضاء", () => ShowEmbeddedPage(new MembersForm(true), btnNavMembers, "👥  إدارة الأعضاء")), 0, 0);
            hdr.Controls.Add(MakeHdrLabel("مخزون منخفض (≤5)", () => ShowEmbeddedPage(new StoreForm(true), btnNavStore, "🛒  المتجر (POS)")), 1, 0);
            hdr.Controls.Add(MakeHdrLabel("آخر مبيعات المتجر", () => ShowEmbeddedPage(new StoreForm(true), btnNavStore, "🛒  المتجر (POS)")), 2, 0);

            tbl.Controls.Add(MakeListColumn(out _lstQuickMembers), 0, 0);
            tbl.Controls.Add(MakeListColumn(out _lstQuickStock), 1, 0);
            tbl.Controls.Add(MakeListColumn(out _lstQuickSales), 2, 0);

            _pnlDashboardQuick.Controls.Add(tbl);
            _pnlDashboardQuick.Controls.Add(hdr);
            _pnlDashboardQuick.Controls.Add(flowBtns);
            _pnlDashboardQuick.Controls.Add(lblQuick);

            pnlDashboardHome.Controls.Add(_pnlDashboardQuick);
        }

        private void RefreshQuickLists()
        {
            if (_lstQuickMembers == null) return;

            _lstQuickMembers.Items.Clear();
            foreach (var m in GymDataStore.Data.Members.OrderByDescending(x => x.Id).Take(8))
                _lstQuickMembers.Items.Add(m.FullName + "  —  " + m.Phone);

            _lstQuickStock.Items.Clear();
            foreach (var p in GymDataStore.Data.StoreProducts.Where(x => x.StockQty <= 5).OrderBy(x => x.StockQty).Take(10))
                _lstQuickStock.Items.Add(p.Name + "  (" + p.StockQty + ")");

            _lstQuickSales.Items.Clear();
            foreach (var s in GymDataStore.Data.StoreSales.OrderByDescending(x => x.SoldAt).Take(8))
                _lstQuickSales.Items.Add((s.SoldAt ?? "").Substring(0, Math.Min(16, (s.SoldAt ?? "").Length)) + "  —  " + s.Total.ToString("0.##") + " $");
        }

        // ── navigation ────────────────────────────────────
        private void AssignNavEvents()
        {
            btnNavHome.Click     += (s, e) => ShowDashboardHome();
            btnNavMembers.Click  += (s, e) => ShowEmbeddedPage(new MembersForm(),       btnNavMembers,  "👥  إدارة الأعضاء");
            btnNavSubs.Click     += (s, e) => ShowEmbeddedPage(new SubscriptionsForm(), btnNavSubs,     "📋  الاشتراكات");
            btnNavStore.Click    += (s, e) => ShowEmbeddedPage(new StoreForm(),         btnNavStore,    "🛒  المتجر (POS)");
            btnNavDiet.Click     += (s, e) => ShowEmbeddedPage(new DietPlanForm(),      btnNavDiet,     "🍏  التغذية");
            btnNavReports.Click  += (s, e) => ShowEmbeddedPage(new ReportsForm(),       btnNavReports,  "📊  المالية");
            btnNavTrainers.Click += (s, e) => ShowEmbeddedPage(new TrainersForm(),      btnNavTrainers, "🏋️  إدارة المدربين");
            btnNavUsers.Click    += (s, e) => ShowEmbeddedPage(new UsersForm(),         btnNavUsers,    "👤  إدارة المستخدمين");
            btnNotifications.Click += (s, e) => ToggleNotifications();
        }

        // ── Role-based nav visibility ───────────────────────────────
        private void ApplyRoleVisibility()
        {
            if (AppSession.IsReceptionist)
            {
                btnNavHome.Visible     = false;
                btnNavSubs.Visible     = false;
                btnNavReports.Visible  = false;
                btnNavTrainers.Visible = false;
                btnNavUsers.Visible    = false;
                btnNotifications.Visible = false;
                btnThemeToggle.Location = new Point(12, 6);
                lblStatusLeft.Text = "● واتساب عبر المتصفح (wa.me)";
                ShowEmbeddedPage(new MembersForm(), btnNavMembers, "👥  إدارة الأعضاء");
            }
        }

        private void InitializeCommercialBanner()
        {
            _commercialLabel = new Label
            {
                AutoSize  = true,
                ForeColor = Color.FromArgb(255, 193, 7),
                Font      = new Font("Tahoma", 10F, FontStyle.Bold),
                Text      = CommercialText,
                BackColor = Color.Transparent,
                Top       = 18,
                Left      = topBar.Width
            };
            topBar.Controls.Add(_commercialLabel);
            _commercialLabel.SendToBack();
            EnsureTopBarInteractiveZOrder();

            _commercialTicker = new Timer { Interval = 35 };
            _commercialTicker.Tick += (s, e) =>
            {
                _commercialLabel.Left -= 2;
                if (_commercialLabel.Right < 120)
                    _commercialLabel.Left = topBar.Width;
            };
            _commercialTicker.Start();
        }

        /// <summary>Keeps the scrolling banner behind notification, theme, title, and sign-out.</summary>
        private void EnsureTopBarInteractiveZOrder()
        {
            if (_commercialLabel != null)
                _commercialLabel.SendToBack();
            lblDashTitle.BringToFront();
            btnThemeToggle.BringToFront();
            btnNotifications.BringToFront();
            if (_btnSignOut != null)
                _btnSignOut.BringToFront();
        }

        private void ShowEmbeddedPage(Form childForm, Button navButton, string title)
        {
            pnlDashboardHome.Visible = false;
            pnlContentHost.Visible   = true;
            pnlNotifDropdown.Visible = false;

            foreach (Control c in pnlContentHost.Controls)
                if (c is Form f) f.Close();
            pnlContentHost.Controls.Clear();

            UiColorScheme t = ThemeManager.Current;
            childForm.TopLevel         = false;
            childForm.FormBorderStyle  = FormBorderStyle.None;
            childForm.Dock             = DockStyle.Fill;
            childForm.BackColor        = t.ContentHost;

            pnlContentHost.Controls.Add(childForm);
            ApplyArabicTypography(childForm);
            childForm.Show();

            if (childForm is IThemeAware th)
                th.ApplyTheme(t);

            lblDashTitle.Text = title;
            HighlightNavButton(navButton);
        }

        private void ShowDashboardHome()
        {
            foreach (Control c in pnlContentHost.Controls)
                if (c is Form f) f.Close();
            pnlContentHost.Controls.Clear();
            pnlContentHost.Visible   = false;
            pnlDashboardHome.Visible = true;
            pnlNotifDropdown.Visible = false;

            lblDashTitle.Text = "لوحة التحكم (Dashboard)";
            HighlightNavButton(btnNavHome);
            RefreshDashboardHomeData();
            BuildNotificationItems();
        }

        private void HighlightNavButton(Button btn)
        {
            UiColorScheme t = ThemeManager.Current;
            Color normalBg = t.SidebarNav;
            Color activeBg = t.SidebarNavActive;
            Color normalFg = t.TextMuted;
            Color activeFg = t.TextPrimary;

            foreach (Control c in sidebar.Controls)
                if (c is Button b && b != btn)
                { b.BackColor = normalBg; b.ForeColor = normalFg; }

            btn.BackColor  = activeBg;
            btn.ForeColor  = activeFg;
            _activeNavButton = btn;
        }

        // ── notifications ─────────────────────────────────
        private void ToggleNotifications()
        {
            pnlNotifDropdown.Visible = !pnlNotifDropdown.Visible;
            if (pnlNotifDropdown.Visible) pnlNotifDropdown.BringToFront();
        }

        private void AddSignOutButton()
        {
            _btnSignOut = new Button
            {
                Text = "تسجيل الخروج",
                Size = new Size(118, 34),
                Location = new Point(118, 10),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnSignOut.FlatAppearance.BorderSize = 0;
            _btnSignOut.Click += (s, e) =>
            {
                if (MessageBox.Show("هل تريد تسجيل الخروج؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                RequestSignOut = true;
                Close();
            };
            topBar.Controls.Add(_btnSignOut);
            _btnSignOut.BringToFront();
        }

        private void BuildNotificationItems()
        {
            flowNotifications.Controls.Clear();
            foreach (var p in GymDataStore.Data.StoreProducts.Where(x => x.StockQty <= 5).Take(3))
                AddNotifCard("⚠️", "مخزون منخفض", p.Name + " — المتبقي " + p.StockQty, Color.FromArgb(255, 152, 0));

            int soon = GymDataStore.MembersExpiringWithinDays(14);
            if (soon > 0)
                AddNotifCard("🔴", "اشتراكات قريبة الانتهاء", soon + " عضو خلال أسبوعين", Color.FromArgb(220, 53, 69));

            foreach (var s in GymDataStore.Data.StoreSales.OrderByDescending(x => x.SoldAt).Take(3))
                AddNotifCard("💰", "بيع متجر", (s.Summary ?? "").Trim(), Color.FromArgb(76, 175, 80));

            if (flowNotifications.Controls.Count == 0)
                AddNotifCard("✅", "لا تنبيهات", "كل شيء يبدو جيداً", Color.FromArgb(76, 175, 80));
        }

        private void AddNotifCard(string icon, string title, string body, Color accent)
        {
            UiColorScheme t = ThemeManager.Current;
            Panel card = new Panel { Size = new Size(356, 60), BackColor = t.NotifCard, Margin = new Padding(3, 2, 3, 2), Cursor = Cursors.Hand };
            Panel strip = new Panel { Size = new Size(4, 60), BackColor = accent, Dock = DockStyle.Right };
            Label lblIcon  = new Label { Text = icon,  Font = new Font("Segoe UI", 16F), Size = new Size(40, 55), Location = new Point(305, 2), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            Label lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = t.TextPrimary, Size = new Size(260, 20), Location = new Point(10, 8),  TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label lblBody  = new Label { Text = body,  Font = new Font("Segoe UI",  9F), ForeColor = t.TextMuted, Size = new Size(290, 18), Location = new Point(10, 32), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };

            Color hover = t.NotifCardHover;
            Color normal = t.NotifCard;
            card.MouseEnter  += (s, e) => card.BackColor = hover;
            card.MouseLeave  += (s, e) => card.BackColor = normal;
            foreach (Control c in new Control[] { lblIcon, lblTitle, lblBody })
            {
                c.MouseEnter += (s, e) => card.BackColor = hover;
                c.MouseLeave += (s, e) => card.BackColor = normal;
            }

            card.Controls.Add(strip); card.Controls.Add(lblIcon); card.Controls.Add(lblTitle); card.Controls.Add(lblBody);
            ApplyArabicTypography(card);
            flowNotifications.Controls.Add(card);
        }

        // ── Arabic typography ──────────────────────────────
        private void ApplyArabicTypography(Control root)
        {
            if (root == null) return;
            bool hasArabic = HasArabicText(root.Text);
            bool hasEmoji  = ContainsEmoji(root.Text);

            if (hasArabic) root.Font = CreateArabicFriendlyFont(root.Font, root.Text);

            if      (root is Label lb)           lb.UseCompatibleTextRendering = hasArabic && !hasEmoji;
            else if (root is ButtonBase bb)      bb.UseCompatibleTextRendering = hasArabic && !hasEmoji;
            else if (root is DataGridView dgv)
            {
                string sample = string.Concat(dgv.Columns.Cast<DataGridViewColumn>().Select(col => col.HeaderText ?? ""));
                dgv.DefaultCellStyle.Font               = CreateArabicFriendlyFont(dgv.DefaultCellStyle.Font ?? dgv.Font, sample);
                dgv.ColumnHeadersDefaultCellStyle.Font  = CreateArabicFriendlyFont(dgv.ColumnHeadersDefaultCellStyle.Font ?? dgv.Font, sample);
            }

            foreach (Control child in root.Controls) ApplyArabicTypography(child);
        }

        private static bool HasArabicText(string t)   => !string.IsNullOrWhiteSpace(t) && ArabicRegex.IsMatch(t);
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

        private void DashboardForm_Load(object sender, EventArgs e) { }
    }
}
