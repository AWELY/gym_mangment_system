using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class Form1 : Form, IThemeAware
    {
        public Form1()
        {
            InitializeComponent();
            ImageAssets.ApplyAppIcon(this);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            ThemeManager.ThemeChanged += OnThemeManagerThemeChanged;
            this.FormClosed += (_, __) => ThemeManager.ThemeChanged -= OnThemeManagerThemeChanged;

            this.Paint += Form1_PaintBackground;
            pnlIcon.Paint += PnlIcon_Paint;

            chipAdmin.Click += (_, __) => FillCredentials("admin", "admin");
            lblChipAdminRole.Click += (_, __) => FillCredentials("admin", "admin");
            lblChipAdminCreds.Click += (_, __) => FillCredentials("admin", "admin");
            chipReception.Click += (_, __) => FillCredentials("reception", "1234");
            lblChipRecRole.Click += (_, __) => FillCredentials("reception", "1234");
            lblChipRecCreds.Click += (_, __) => FillCredentials("reception", "1234");

            EnableDrag(this);
            EnableDrag(lblGymName);
            EnableDrag(lblSubtitle);

            AcceptButton = btnLogin;
            ApplyTheme(ThemeManager.Current);
            txtUser.Focus();
        }

        private void FillCredentials(string user, string pass)
        {
            txtUser.Text = user;
            txtPass.Text = pass;
            btnLogin.Focus();
        }

        private void OnThemeManagerThemeChanged(object sender, EventArgs e) => ApplyTheme(ThemeManager.Current);

        // Login screen keeps the Figma purple identity regardless of the app theme.
        public void ApplyTheme(UiColorScheme s)
        {
            GunaUi.ApplyBrandGradient(btnLogin);
            Invalidate();
        }

        // ── deep teal/slate gradient backdrop (clean, professional) ──
        private void Form1_PaintBackground(object sender, PaintEventArgs e)
        {
            var r = ClientRectangle;
            if (r.Width <= 0 || r.Height <= 0) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new LinearGradientBrush(r, Color.FromArgb(0x0B, 0x1F, 0x1E), Color.FromArgb(0x0F, 0x76, 0x6E), LinearGradientMode.ForwardDiagonal))
            {
                brush.InterpolationColors = new ColorBlend(3)
                {
                    Colors = new[]
                    {
                        Color.FromArgb(0x0B, 0x1F, 0x1E),
                        Color.FromArgb(0x0E, 0x3B, 0x38),
                        Color.FromArgb(0x0F, 0x76, 0x6E)
                    },
                    Positions = new[] { 0f, 0.55f, 1f }
                };
                e.Graphics.FillRectangle(brush, r);
            }
        }

        private void PnlIcon_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, pnlIcon.Width - 1, pnlIcon.Height - 1);
            int d = 20, dd = d * 2;
            using (var path = new GraphicsPath())
            {
                path.AddArc(r.X, r.Y, dd, dd, 180, 90);
                path.AddArc(r.Right - dd, r.Y, dd, dd, 270, 90);
                path.AddArc(r.Right - dd, r.Bottom - dd, dd, dd, 0, 90);
                path.AddArc(r.X, r.Bottom - dd, dd, dd, 90, 90);
                path.CloseFigure();
                using (var br = new LinearGradientBrush(r, FigmaPalette.LogoStart, FigmaPalette.LogoEnd, LinearGradientMode.ForwardDiagonal))
                    e.Graphics.FillPath(br, path);
            }
            TextRenderer.DrawText(e.Graphics, "🏋️", new Font("Segoe UI Emoji", 26F),
                new Rectangle(0, 0, pnlIcon.Width, pnlIcon.Height), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void BtnExit_Click(object sender, EventArgs e) => Application.Exit();

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser?.Text?.Trim() ?? "";
            string pass = txtPass?.Text ?? "";

            if (UserDirectory.TryAuthenticate(user, pass, out string displayName, out AppSession.UserRole role))
            {
                AppSession.CurrentRole = role;
                AppSession.Username    = displayName;
            }
            else
            {
                GunaUi.Show(
                    "اسم المستخدم أو كلمة المرور غير صحيحة.\n\nمدير: admin / admin\nاستقبال: reception / 1234",
                    "خطأ في تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DashboardForm dashboard = new DashboardForm();
            this.Hide();
            dashboard.ShowDialog();
            if (dashboard.RequestSignOut)
            {
                AppSession.Username = "";
                txtUser.Text = "";
                txtPass.Text = "";
                txtUser.Focus();
                this.Show();
                return;
            }

            // Real exit (not a sign-out): terminate the whole application. Closing the
            // hidden login form alone can leave the process running, so force a full
            // shutdown of every window and message loop.
            Application.Exit();
        }

        // ── drag the borderless window ──────────────────────
        [DllImport("user32.dll")] private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")] private static extern bool ReleaseCapture();
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        private void EnableDrag(Control c)
        {
            c.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            };
        }
    }
}
