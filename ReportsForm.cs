using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            ApplyBackgroundBranding();
            pnlChart.Paint += PnlChart_Paint;
        }

        private void ApplyBackgroundBranding()
        {
            Image faded = ImageAssets.TryLoadToughBackground("reports", 0.22f);
            if (faded == null) return;
            this.BackgroundImage = faded;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            pnlChart.BackgroundImage = faded;
            pnlChart.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void PnlChart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlChart.Width;
            int h = pnlChart.Height;
            int padL = 70, padR = 20, padT = 20, padB = 40;
            int chartW = w - padL - padR;
            int chartH = h - padT - padB;

            if (chartW <= 0 || chartH <= 0) return;

            // Grid
            using (Pen gridPen = new Pen(Color.FromArgb(40, 40, 48)))
            {
                for (int i = 0; i <= 6; i++)
                {
                    int y = padT + (chartH * i / 6);
                    g.DrawLine(gridPen, padL, y, w - padR, y);
                }
            }

            decimal[] dSubs  = GymDataStore.SubscriptionTotalsByMonthCurrentYear();
            decimal[] dStore = GymDataStore.StoreTotalsByMonthCurrentYear();
            float[] subs  = dSubs.Select(x => (float)x).ToArray();
            float[] store = dStore.Select(x => (float)x).ToArray();
            string[] months = { "ين", "فب", "مار", "أبر", "ماي", "يون", "يول", "أغس", "سبت", "أكت", "نوف", "ديس" };
            float mSubs  = subs.Length > 0 ? subs.Max() : 0f;
            float mStore = store.Length > 0 ? store.Max() : 0f;
            float maxVal = Math.Max(500f, Math.Max(mSubs, mStore) * 1.15f);
            int barW = Math.Max(10, chartW / 24);

            // Y axis labels
            using (Font axisFont = new Font("Segoe UI", 8F))
            using (Brush axisBrush = new SolidBrush(Color.FromArgb(120, 120, 130)))
            {
                for (int i = 0; i <= 6; i++)
                {
                    int y = padT + (chartH * i / 6);
                    string val = ((int)(maxVal - maxVal * i / 6)).ToString("N0") + "$";
                    g.DrawString(val, axisFont, axisBrush, 2, y - 7);
                }
            }

            // Draw bars
            for (int i = 0; i < 12; i++)
            {
                int x = padL + (chartW * i / 12) + barW / 2;

                // Subscriptions bar (blue)
                int hSubs = (int)(chartH * subs[i] / maxVal);
                using (Brush bSubs = new SolidBrush(Color.FromArgb(33, 150, 243)))
                {
                    g.FillRectangle(bSubs, x, padT + chartH - hSubs, barW, hSubs);
                }

                // Store bar (green, stacked on top)
                int hStore = (int)(chartH * store[i] / maxVal);
                using (Brush bStore = new SolidBrush(Color.FromArgb(76, 175, 80)))
                {
                    g.FillRectangle(bStore, x + barW + 2, padT + chartH - hStore, barW, hStore);
                }

                // Month label
                using (Font mFont = new Font("Segoe UI", 7F))
                using (Brush mBrush = new SolidBrush(Color.FromArgb(130, 130, 140)))
                {

                    g.DrawString(months[i], mFont, mBrush, x, padT + chartH + 8);
                }
            }

            // Legend
            int lx = w - 250;
            int ly = 8;
            using (Font lFont = new Font("Segoe UI", 9F, FontStyle.Bold))
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(33, 150, 243)), lx, ly + 3, 14, 14);
                g.DrawString("الاشتراكات", lFont, Brushes.White, lx + 20, ly);

                g.FillRectangle(new SolidBrush(Color.FromArgb(76, 175, 80)), lx + 110, ly + 3, 14, 14);
                g.DrawString("المتجر", lFont, Brushes.White, lx + 130, ly);
            }
        }
    }
}
        