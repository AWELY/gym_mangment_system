using System;
using System.Drawing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class SplashForm : Form
    {
        private readonly Timer _timer = new Timer();

        public SplashForm()
        {
            InitializeComponent();
            ImageAssets.ApplyAppIcon(this);
            ApplyBranding();
            _timer.Interval = 2000;
            _timer.Tick += Timer_Tick;
            this.Shown += SplashForm_Shown;
            this.FormClosing += SplashForm_FormClosing;
        }

        private void ApplyBranding()
        {
            System.Drawing.Image logo = ImageAssets.TryLoad(ImageAssets.LogoHeartDumbbell);
            if (logo != null)
            {
                picLogo.Image = logo;
                picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            }

            System.Drawing.Image bg = ImageAssets.TryLoadToughBackground("splash", 0.22f);
            if (bg != null)
            {
                this.BackgroundImage = bg;
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            // The splash no longer uses video; hide the host panel.
            pnlVideoHost.Visible = false;
        }

        private void SplashForm_Shown(object sender, EventArgs e)
        {
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            this.Close();
        }

        private void SplashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop();
        }
    }
}
