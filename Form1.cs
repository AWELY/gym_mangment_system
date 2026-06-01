using System;
using System.Drawing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class Form1 : Form, IThemeAware
    {
        private bool _autoLoginOnFirstShow = true;

        public Form1()
        {
            InitializeComponent();
            ThemeManager.ThemeChanged += OnThemeManagerThemeChanged;
            this.FormClosed += (_, __) => ThemeManager.ThemeChanged -= OnThemeManagerThemeChanged;
            ApplyBranding();
            ApplyTheme(ThemeManager.Current);
            txtUser.Text = "admin";
            txtPass.Text = "admin";
            this.Shown += Form1_Shown;
        }

        private void OnThemeManagerThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme(ThemeManager.Current);
            ApplyBranding();
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.FormBackground;
            topBar.BackColor = ThemeManager.IsLight ? s.StatusBar : Color.FromArgb(15, 15, 15);
            lblTitleBar.ForeColor = s.TextPrimary;
            btnClose.ForeColor = s.TextPrimary;

            mainPanel.BackColor = ThemeManager.IsLight ? s.Panel : Color.FromArgb(30, 30, 30);
            brandPanel.BackColor = ThemeManager.IsLight ? s.PanelElevated : Color.FromArgb(20, 20, 20);

            lblTitle.ForeColor = s.TextPrimary;
            if (lblGymName != null) lblGymName.ForeColor = s.TextPrimary;
            lblUser.ForeColor = s.TextMuted;
            lblPass.ForeColor = s.TextMuted;
            chkRemember.ForeColor = s.TextMuted;

            pnlUser.BackColor = s.InputBackground;
            txtUser.BackColor = s.InputBackground;
            txtUser.ForeColor = s.InputForeground;
            pnlPass.BackColor = s.InputBackground;
            txtPass.BackColor = s.InputBackground;
            txtPass.ForeColor = s.InputForeground;

            lblWelcome.ForeColor = s.TextPrimary;

            btnExit.BackColor = s.SecondaryButton;
            btnExit.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.White;
            btnExit.FlatAppearance.MouseOverBackColor = s.SecondaryButtonHover;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!_autoLoginOnFirstShow) return;
            _autoLoginOnFirstShow = false;
            BtnLogin_Click(this, EventArgs.Empty);
        }

        private void ApplyBranding()
        {
            if (ThemeManager.IsLight)
            {
                // Figma login is a clean light surface (no photo backdrop).
                this.BackgroundImage = null;
            }
            else
            {
                Image bg = ImageAssets.TryLoad(ImageAssets.BgGym);
                if (bg != null)
                {
                    this.BackgroundImage = ImageAssets.CreateWithOpacity(bg, 0.28f);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }

            Image logo = ImageAssets.TryLoad(ImageAssets.LogoHeartDumbbell);
            if (logo != null && picLogo != null)
            {
                picLogo.Image    = logo;
                picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e) => Application.Exit();

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser?.Text?.Trim() ?? "";
            string pass = txtPass?.Text ?? "";

            if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(pass))
            {
                user = "admin";
                pass = "admin";
            }

            if (UserDirectory.TryAuthenticate(user, pass, out string displayName, out AppSession.UserRole role))
            {
                AppSession.CurrentRole = role;
                AppSession.Username    = displayName;
            }
            else
            {
                MessageBox.Show(
                    "اسم المستخدم أو كلمة المرور غير صحيحة.\n\nافتراضي: admin / admin\nمستلم: reception / 1234\n(يمكن إضافة مستخدمين من شاشة المدير)",
                    "خطأ في تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DashboardForm dashboard = new DashboardForm();
            this.Hide();
            dashboard.ShowDialog();
            if (dashboard.RequestSignOut)
            {
                AppSession.Username = "";
                txtUser.Text = "admin";
                txtPass.Text = "admin";
                txtUser.Focus();
                txtUser.SelectAll();
                this.Show();
                return;
            }
            this.Close();
        }
    }
}
