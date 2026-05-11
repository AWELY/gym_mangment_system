using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private const string CommercialText = "أهلاً بكم في نادي غلوري، هنا تُصنع القوة وتولد الأساطير، انطلق اليوم نحو نسختك الأقوى.";

        // ── stat card data ──
        private static readonly (string Icon, string Title, string Value, Color Accent)[] StatCards =
        {
            ("👥", "إجمالي الأعضاء",         "248",        Color.FromArgb(33,  150, 243)),
            ("✅", "الاشتراكات النشطة",       "195",        Color.FromArgb(76,  175,  80)),
            ("⚠️", "اشتراكات تنتهي قريباً",  "12",         Color.FromArgb(255, 152,   0)),
            ("💰", "إيرادات هذا الشهر",      "5,200 $",    Color.FromArgb(76,  175,  80)),
            ("🛒", "مبيعات المتجر اليوم",     "340 $",      Color.FromArgb(156,  39, 176)),
            ("🏋️","حضور اليوم",              "85",         Color.FromArgb(33,  150, 243)),
        };

        public DashboardForm()
        {
            InitializeComponent();
            ApplyBrandImages();
            ApplyArabicTypography(this);
            AssignNavEvents();
            ApplyRoleVisibility();
            _activeNavButton = btnNavHome;
            HighlightNavButton(btnNavHome);
            BuildStatCards();
            BuildNotificationItems();
            InitializeCommercialBanner();
            lblStatusRight.Text = "اسم المستخدم: " + AppSession.Username;
            this.FormClosing += (s, e) => { if (_commercialTicker != null) _commercialTicker.Stop(); };
        }

        // ── branding ──────────────────────────────────────
        private void ApplyBrandImages()
        {
            Image bg = ImageAssets.TryLoadToughBackground("dashboard", 0.55f);
            if (bg != null)
            {
                this.BackgroundImage = bg;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                pnlDashboardHome.BackgroundImage = ImageAssets.TryLoadToughBackground("dashboard-home", 0.60f);
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

        // ── stat cards ────────────────────────────────────
        private void BuildStatCards()
        {
            flowStatCards.Controls.Clear();

            // Decide card size relative to available width
            int cardW = 240;
            int cardH = 130;

            foreach (var (icon, title, value, accent) in StatCards)
            {
                Panel card = new Panel
                {
                    Size      = new Size(cardW, cardH),
                    BackColor = Color.FromArgb(32, 32, 40),
                    Margin    = new Padding(12),
                    Cursor    = Cursors.Hand
                };

                // Left accent strip
                Panel strip = new Panel
                {
                    Size      = new Size(5, cardH),
                    BackColor = accent,
                    Dock      = DockStyle.Left
                };

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
                    Font      = new Font("Segoe UI", 26F, FontStyle.Bold),
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
                    ForeColor = Color.FromArgb(160, 160, 175),
                    Size      = new Size(cardW - 80, 24),
                    Location  = new Point(10, 86),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent
                };

                // Hover glow
                card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(42, 42, 52);
                card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(32, 32, 40);
                foreach (Control c in new Control[] { lblIcon, lblVal, lblTitle })
                {
                    c.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(42, 42, 52);
                    c.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(32, 32, 40);
                }

                card.Controls.Add(strip);
                card.Controls.Add(lblIcon);
                card.Controls.Add(lblVal);
                card.Controls.Add(lblTitle);

                flowStatCards.Controls.Add(card);
            }
        }

        // ── navigation ────────────────────────────────────
        private void AssignNavEvents()
        {
            btnNavHome.Click     += (s, e) => ShowDashboardHome();
            btnNavMembers.Click  += (s, e) => ShowEmbeddedPage(new MembersForm(),       btnNavMembers,  "👥  إدارة الأعضاء");
            btnNavSubs.Click     += (s, e) => ShowEmbeddedPage(new SubscriptionsForm(), btnNavSubs,     "📋  الاشتراكات");
            btnNavStore.Click    += (s, e) => ShowEmbeddedPage(new StoreForm(),         btnNavStore,    "🛒  المتجر (POS)");
            btnNavDiet.Click     += (s, e) => ShowEmbeddedPage(new DietPlanForm(),      btnNavDiet,     "🍏  التغذية");
            btnNavReports.Click  += (s, e) => ShowEmbeddedPage(new ReportsForm(),       btnNavReports,  "📊  التقارير المالية");
            btnNavTrainers.Click += (s, e) => ShowEmbeddedPage(new TrainersForm(),      btnNavTrainers, "🏋️  إدارة المدربين");
            btnNavUsers.Click    += (s, e) => ShowEmbeddedPage(new UsersForm(),         btnNavUsers,    "👤  إدارة المستخدمين");
            btnNotifications.Click += (s, e) => ToggleNotifications();
        }

        // ── Role-based nav visibility ───────────────────────────────
        private void ApplyRoleVisibility()
        {
            if (AppSession.IsReceptionist)
            {
                // Recipient: only Members, Store, Diet (feeding)
                btnNavHome.Visible     = false;
                btnNavSubs.Visible     = false;
                btnNavReports.Visible  = false;
                btnNavTrainers.Visible = false;
                btnNavUsers.Visible    = false;
                btnNotifications.Visible = false;
                lblStatusLeft.Text = "● واتساب عبر المتصفح (wa.me)";
                // Start on Members page directly
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
            _commercialLabel.BringToFront();

            _commercialTicker = new Timer { Interval = 35 };
            _commercialTicker.Tick += (s, e) =>
            {
                _commercialLabel.Left -= 2;
                if (_commercialLabel.Right < 120)
                    _commercialLabel.Left = topBar.Width;
            };
            _commercialTicker.Start();
        }

        private void ShowEmbeddedPage(Form childForm, Button navButton, string title)
        {
            pnlDashboardHome.Visible = false;
            pnlContentHost.Visible   = true;
            pnlNotifDropdown.Visible = false;

            foreach (Control c in pnlContentHost.Controls)
                if (c is Form f) f.Close();
            pnlContentHost.Controls.Clear();

            childForm.TopLevel         = false;
            childForm.FormBorderStyle  = FormBorderStyle.None;
            childForm.Dock             = DockStyle.Fill;
            childForm.BackColor        = Color.FromArgb(18, 18, 22);

            pnlContentHost.Controls.Add(childForm);
            ApplyArabicTypography(childForm);
            childForm.Show();

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
        }

        private void HighlightNavButton(Button btn)
        {
            Color normalBg = Color.FromArgb(22, 22, 26);
            Color activeBg = Color.FromArgb(40, 40, 48);
            Color normalFg = Color.FromArgb(160, 160, 170);
            Color activeFg = Color.White;

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

        private void BuildNotificationItems()
        {
            flowNotifications.Controls.Clear();
            AddNotifCard("⚠️", "المخزون منخفض",  "بروتين مصل اللبن - أقل من 5 وحدات",  Color.FromArgb(255, 152, 0));
            AddNotifCard("🔴", "انتهاء اشتراك",   "أحمد محمد - ينتهي خلال 3 أيام",       Color.FromArgb(220,  53, 69));
            AddNotifCard("🔴", "انتهاء اشتراك",   "سارة علي - ينتهي غداً",               Color.FromArgb(220,  53, 69));
            AddNotifCard("🔴", "انتهاء اشتراك",   "خالد إبراهيم - ينتهي خلال 5 أيام",   Color.FromArgb(220,  53, 69));
            AddNotifCard("💰", "دفعة جديدة",      "تم استلام 150$ من يوسف كمال",          Color.FromArgb( 76, 175, 80));
            AddNotifCard("👤", "عضو جديد",        "انضمت فاطمة سعيد اليوم",              Color.FromArgb( 33, 150, 243));
        }

        private void AddNotifCard(string icon, string title, string body, Color accent)
        {
            Panel card = new Panel { Size = new Size(356, 60), BackColor = Color.FromArgb(38, 38, 45), Margin = new Padding(3, 2, 3, 2), Cursor = Cursors.Hand };
            Panel strip = new Panel { Size = new Size(4, 60), BackColor = accent, Dock = DockStyle.Right };
            Label lblIcon  = new Label { Text = icon,  Font = new Font("Segoe UI", 16F), Size = new Size(40, 55), Location = new Point(305, 2), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            Label lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.White,                    Size = new Size(260, 20), Location = new Point(10, 8),  TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label lblBody  = new Label { Text = body,  Font = new Font("Segoe UI",  9F),                 ForeColor = Color.FromArgb(150, 150, 160), Size = new Size(290, 18), Location = new Point(10, 32), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };

            card.MouseEnter  += (s, e) => card.BackColor = Color.FromArgb(48, 48, 55);
            card.MouseLeave  += (s, e) => card.BackColor = Color.FromArgb(38, 38, 45);
            foreach (Control c in new Control[] { lblIcon, lblTitle, lblBody })
            {
                c.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(48, 48, 55);
                c.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(38, 38, 45);
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
