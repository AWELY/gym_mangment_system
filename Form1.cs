using System;
using System.Drawing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class Form1 : Form
    {
        private bool _autoLoginOnFirstShow = true;

        public Form1()
        {
            InitializeComponent();
            ApplyBranding();
            txtUser.Text = "admin";
            txtPass.Text = "admin";
            this.Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!_autoLoginOnFirstShow) return;
            _autoLoginOnFirstShow = false;
            BtnLogin_Click(this, EventArgs.Empty);
        }

        private void ApplyBranding()
        {
            Image bg = ImageAssets.TryLoad(ImageAssets.BgGym);
            if (bg != null)
            {
                this.BackgroundImage = ImageAssets.CreateWithOpacity(bg, 0.28f);
                this.BackgroundImageLayout = ImageLayout.Stretch;
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
