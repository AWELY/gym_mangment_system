using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace gym_mangment_system
{
    public partial class ReportsForm : Form, IThemeAware
    {
        private decimal _monthlySubscriptions;
        private decimal _monthlyStore;
        private decimal _monthlySalaries;
        private decimal _monthlyGross;
        private decimal _monthlyNet;

        public ReportsForm()
        {
            InitializeComponent();
            ApplyBackgroundBranding();
            BuildFinancialReport();
        }

        public void ApplyTheme(UiColorScheme _)
        {
            ApplyBackgroundBranding();
            BuildFinancialReport();
        }

        private void ApplyBackgroundBranding()
        {
            float op = ThemeManager.BrandingOpacity();
            Image faded = ImageAssets.TryLoadToughBackground("reports", op);
            if (faded == null) return;
            this.BackgroundImage = faded;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            if (pnlChart != null)
            {
                pnlChart.BackgroundImage = faded;
                pnlChart.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void BuildFinancialReport()
        {
            UiColorScheme s = ThemeManager.Current;
            Controls.Clear();

            _monthlySubscriptions = GymDataStore.SubscriptionCashInThisMonth();
            _monthlyStore = GymDataStore.StoreRevenueThisMonth();
            _monthlySalaries = GymDataStore.Data.Trainers.Sum(t => t.Salary);
            _monthlyGross = _monthlySubscriptions + _monthlyStore;
            _monthlyNet = _monthlyGross - _monthlySalaries;

            BackColor = s.ContentHost;

            var title = new Label
            {
                Dock = DockStyle.Top,
                Height = 70,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = s.TextPrimary,
                Padding = new Padding(15, 10, 15, 5),
                Text = "📊  المالية",
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            var cards = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 145,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(3),
                BackColor = Color.Transparent
            };
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            cards.Controls.Add(CreateFinancialCard("💵 ربح الاشتراكات", FormatMoney(_monthlySubscriptions), Color.FromArgb(43, 127, 255), s), 3, 0);
            cards.Controls.Add(CreateFinancialCard("🛒 ربح المتجر", FormatMoney(_monthlyStore), Color.FromArgb(0, 166, 62), s), 2, 0);
            cards.Controls.Add(CreateFinancialCard("🏋️ رواتب الشهر", "-" + FormatMoney(_monthlySalaries), Color.FromArgb(231, 0, 11), s), 1, 0);
            cards.Controls.Add(CreateFinancialCard("✅ المتبقي بعد الرواتب", FormatMoney(_monthlyNet), _monthlyNet >= 0 ? Color.FromArgb(0, 166, 62) : Color.FromArgb(231, 0, 11), s), 0, 0);

            var summary = new Guna2Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                FillColor = s.PanelElevated,
                BorderColor = s.BorderSubtle,
                BorderRadius = 12,
                Padding = new Padding(20, 12, 20, 12),
                Margin = new Padding(0, 12, 0, 12)
            };
            summary.ShadowDecoration.Enabled = true;

            var formula = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = s.TextPrimary,
                TextAlign = ContentAlignment.MiddleRight,
                Text = "المعادلة: الاشتراكات + المتجر - الرواتب = المتبقي",
                BackColor = Color.Transparent
            };

            var details = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F),
                ForeColor = s.TextMuted,
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent,
                Text = string.Join(Environment.NewLine, new[]
                {
                    "دخل الشهر قبل الرواتب: " + FormatMoney(_monthlyGross),
                    "إجمالي رواتب المدربين الشهرية: " + FormatMoney(_monthlySalaries),
                    "المبلغ الموجود بعد خصم الرواتب: " + FormatMoney(_monthlyNet),
                    _monthlyNet >= 0
                        ? "الحالة: يوجد فائض بعد دفع الرواتب."
                        : "الحالة: يوجد عجز بعد دفع الرواتب."
                })
            };

            summary.Controls.Add(details);
            summary.Controls.Add(formula);

            pnlChart = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = s.Panel,
                Margin = new Padding(0, 12, 0, 0)
            };
            pnlChart.Paint += PnlChart_Paint;

            if (BackgroundImage != null)
            {
                pnlChart.BackgroundImage = BackgroundImage;
                pnlChart.BackgroundImageLayout = ImageLayout.Stretch;
            }

            Controls.Add(pnlChart);
            Controls.Add(summary);
            Controls.Add(cards);
            Controls.Add(title);
        }

        private Control CreateFinancialCard(string title, string value, Color accent, UiColorScheme s)
        {
            var panel = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = s.PanelElevated,
                BorderColor = s.BorderSubtle,
                BorderRadius = 12,
                Margin = new Padding(8)
            };
            panel.ShadowDecoration.Enabled = true;

            var titleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 11F),
                ForeColor = s.TextMuted,
                Text = title,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            var valueLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = accent,
                Text = value,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            var strip = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 5,
                BackColor = accent
            };

            panel.Controls.Add(valueLabel);
            panel.Controls.Add(titleLabel);
            panel.Controls.Add(strip);
            return panel;
        }

        private void PnlChart_Paint(object sender, PaintEventArgs e)
        {
            UiColorScheme s = ThemeManager.Current;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlChart.Width;
            int h = pnlChart.Height;
            int padL = 70, padR = 20, padT = 20, padB = 40;
            int chartW = w - padL - padR;
            int chartH = h - padT - padB;

            if (chartW <= 0 || chartH <= 0) return;

            using (Font headerFont = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (Brush headerBrush = new SolidBrush(s.TextPrimary))
            {
                g.DrawString("تدفق السنة: الدخل الشهري، الرواتب، والمتبقي", headerFont, headerBrush, w - 390, 8);
            }

            using (Pen gridPen = new Pen(s.BorderSubtle))
            {
                for (int i = 0; i <= 6; i++)
                {
                    int y = padT + (chartH * i / 6);
                    g.DrawLine(gridPen, padL, y, w - padR, y);
                }
            }

            decimal[] dSubs = GymDataStore.SubscriptionTotalsByMonthCurrentYear();
            decimal[] dStore = GymDataStore.StoreTotalsByMonthCurrentYear();
            float[] gross = dSubs.Zip(dStore, (a, b) => (float)(a + b)).ToArray();
            float salary = (float)GymDataStore.Data.Trainers.Sum(t => t.Salary);
            float[] net = gross.Select(x => x - salary).ToArray();
            string[] months = { "ين", "فب", "مار", "أبر", "ماي", "يون", "يول", "أغس", "سبت", "أكت", "نوف", "ديس" };
            float maxGross = gross.Length > 0 ? gross.Max() : 0f;
            float maxVal = Math.Max(500f, Math.Max(maxGross, salary) * 1.20f);
            int barW = Math.Max(8, chartW / 34);

            using (Font axisFont = new Font("Segoe UI", 8F))
            using (Brush axisBrush = new SolidBrush(s.TextMuted))
            {
                for (int i = 0; i <= 6; i++)
                {
                    int y = padT + (chartH * i / 6);
                    string val = ((int)(maxVal - maxVal * i / 6)).ToString("N0") + " ريال";
                    g.DrawString(val, axisFont, axisBrush, 2, y - 7);
                }
            }

            for (int i = 0; i < 12; i++)
            {
                int x = padL + (chartW * i / 12) + barW;

                int hGross = (int)(chartH * gross[i] / maxVal);
                using (Brush bGross = new SolidBrush(Color.FromArgb(0, 166, 62)))
                    g.FillRectangle(bGross, x, padT + chartH - hGross, barW, hGross);

                int hSalary = (int)(chartH * salary / maxVal);
                using (Brush bSalary = new SolidBrush(Color.FromArgb(231, 0, 11)))
                    g.FillRectangle(bSalary, x + barW + 2, padT + chartH - hSalary, barW, hSalary);

                int hNet = (int)(chartH * Math.Abs(net[i]) / maxVal);
                Color netColor = net[i] >= 0 ? Color.FromArgb(43, 127, 255) : Color.FromArgb(255, 105, 0);
                using (Brush bNet = new SolidBrush(netColor))
                    g.FillRectangle(bNet, x + (barW * 2) + 4, padT + chartH - hNet, barW, hNet);

                using (Font mFont = new Font("Segoe UI", 7F))
                using (Brush mBrush = new SolidBrush(s.TextMuted))
                    g.DrawString(months[i], mFont, mBrush, x, padT + chartH + 8);
            }

            int lx = w - 390;
            int ly = 8;
            using (Font lFont = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (Brush legBrush = new SolidBrush(s.TextPrimary))
            using (Brush incomeBrush = new SolidBrush(Color.FromArgb(0, 166, 62)))
            using (Brush salaryBrush = new SolidBrush(Color.FromArgb(231, 0, 11)))
            using (Brush netBrush = new SolidBrush(Color.FromArgb(43, 127, 255)))
            {
                g.FillRectangle(incomeBrush, lx, ly + 28, 14, 14);
                g.DrawString("الدخل", lFont, legBrush, lx + 20, ly + 25);

                g.FillRectangle(salaryBrush, lx + 95, ly + 28, 14, 14);
                g.DrawString("الرواتب", lFont, legBrush, lx + 115, ly + 25);

                g.FillRectangle(netBrush, lx + 205, ly + 28, 14, 14);
                g.DrawString("المتبقي", lFont, legBrush, lx + 225, ly + 25);
            }
        }

        private static string FormatMoney(decimal value)
        {
            return value.ToString("N0") + " ريال";
        }
    }
}
