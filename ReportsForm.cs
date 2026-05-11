using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class ReportsForm : Form
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

        private void ApplyBackgroundBranding()
        {
            Image faded = ImageAssets.TryLoadToughBackground("reports", 0.22f);
            if (faded == null) return;
            this.BackgroundImage = faded;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            pnlChart.BackgroundImage = faded;
            pnlChart.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BuildFinancialReport()
        {
            Controls.Clear();

            _monthlySubscriptions = GymDataStore.SubscriptionCashInThisMonth();
            _monthlyStore = GymDataStore.StoreRevenueThisMonth();
            _monthlySalaries = GymDataStore.Data.Trainers.Sum(t => t.Salary);
            _monthlyGross = _monthlySubscriptions + _monthlyStore;
            _monthlyNet = _monthlyGross - _monthlySalaries;

            var title = new Label
            {
                Dock = DockStyle.Top,
                Height = 70,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Padding = new Padding(15, 10, 15, 5),
                Text = "📊  المالية",
                TextAlign = ContentAlignment.MiddleRight
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

            cards.Controls.Add(CreateFinancialCard("💵 ربح الاشتراكات", FormatMoney(_monthlySubscriptions), Color.FromArgb(33, 150, 243)), 3, 0);
            cards.Controls.Add(CreateFinancialCard("🛒 ربح المتجر", FormatMoney(_monthlyStore), Color.FromArgb(76, 175, 80)), 2, 0);
            cards.Controls.Add(CreateFinancialCard("🏋️ رواتب الشهر", "-" + FormatMoney(_monthlySalaries), Color.FromArgb(244, 67, 54)), 1, 0);
            cards.Controls.Add(CreateFinancialCard("✅ المتبقي بعد الرواتب", FormatMoney(_monthlyNet), _monthlyNet >= 0 ? Color.FromArgb(76, 175, 80) : Color.FromArgb(244, 67, 54)), 0, 0);

            var summary = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = Color.FromArgb(32, 32, 38),
                Padding = new Padding(20, 12, 20, 12),
                Margin = new Padding(0, 12, 0, 12)
            };

            var formula = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleRight,
                Text = "المعادلة: الاشتراكات + المتجر - الرواتب = المتبقي"
            };

            var details = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(210, 210, 220),
                TextAlign = ContentAlignment.MiddleRight,
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
                BackColor = Color.FromArgb(35, 35, 35),
                Margin = new Padding(0, 12, 0, 0)
            };
            pnlChart.Paint += PnlChart_Paint;

            Controls.Add(pnlChart);
            Controls.Add(summary);
            Controls.Add(cards);
            Controls.Add(title);
        }

        private Panel CreateFinancialCard(string title, string value, Color accent)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 35),
                Margin = new Padding(8)
            };

            var titleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.LightGray,
                Text = title,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var valueLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = accent,
                Text = value,
                TextAlign = ContentAlignment.MiddleCenter
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
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlChart.Width;
            int h = pnlChart.Height;
            int padL = 70, padR = 20, padT = 20, padB = 40;
            int chartW = w - padL - padR;
            int chartH = h - padT - padB;

            if (chartW <= 0 || chartH <= 0) return;

            using (Font headerFont = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (Brush headerBrush = new SolidBrush(Color.White))
            {
                g.DrawString("تدفق السنة: الدخل الشهري، الرواتب، والمتبقي", headerFont, headerBrush, w - 390, 8);
            }

            // Grid
            using (Pen gridPen = new Pen(Color.FromArgb(55, 55, 65)))
            {
                for (int i = 0; i <= 6; i++)
                {
                    int y = padT + (chartH * i / 6);
                    g.DrawLine(gridPen, padL, y, w - padR, y);
                }
            }

            decimal[] dSubs  = GymDataStore.SubscriptionTotalsByMonthCurrentYear();
            decimal[] dStore = GymDataStore.StoreTotalsByMonthCurrentYear();
            float[] gross = dSubs.Zip(dStore, (s, st) => (float)(s + st)).ToArray();
            float salary = (float)GymDataStore.Data.Trainers.Sum(t => t.Salary);
            float[] net = gross.Select(x => x - salary).ToArray();
            string[] months = { "ين", "فب", "مار", "أبر", "ماي", "يون", "يول", "أغس", "سبت", "أكت", "نوف", "ديس" };
            float maxGross = gross.Length > 0 ? gross.Max() : 0f;
            float maxVal = Math.Max(500f, Math.Max(maxGross, salary) * 1.20f);
            int barW = Math.Max(8, chartW / 34);

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
                int x = padL + (chartW * i / 12) + barW;

                int hGross = (int)(chartH * gross[i] / maxVal);
                using (Brush bGross = new SolidBrush(Color.FromArgb(76, 175, 80)))
                {
                    g.FillRectangle(bGross, x, padT + chartH - hGross, barW, hGross);
                }

                int hSalary = (int)(chartH * salary / maxVal);
                using (Brush bSalary = new SolidBrush(Color.FromArgb(244, 67, 54)))
                {
                    g.FillRectangle(bSalary, x + barW + 2, padT + chartH - hSalary, barW, hSalary);
                }

                int hNet = (int)(chartH * Math.Abs(net[i]) / maxVal);
                Color netColor = net[i] >= 0 ? Color.FromArgb(33, 150, 243) : Color.FromArgb(255, 152, 0);
                using (Brush bNet = new SolidBrush(netColor))
                {
                    g.FillRectangle(bNet, x + (barW * 2) + 4, padT + chartH - hNet, barW, hNet);
                }

                // Month label
                using (Font mFont = new Font("Segoe UI", 7F))
                using (Brush mBrush = new SolidBrush(Color.FromArgb(130, 130, 140)))
                {

                    g.DrawString(months[i], mFont, mBrush, x, padT + chartH + 8);
                }
            }

            // Legend
            int lx = w - 390;
            int ly = 8;
            using (Font lFont = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (Brush incomeBrush = new SolidBrush(Color.FromArgb(76, 175, 80)))
            using (Brush salaryBrush = new SolidBrush(Color.FromArgb(244, 67, 54)))
            using (Brush netBrush = new SolidBrush(Color.FromArgb(33, 150, 243)))
            {
                g.FillRectangle(incomeBrush, lx, ly + 28, 14, 14);
                g.DrawString("الدخل", lFont, Brushes.White, lx + 20, ly + 25);

                g.FillRectangle(salaryBrush, lx + 95, ly + 28, 14, 14);
                g.DrawString("الرواتب", lFont, Brushes.White, lx + 115, ly + 25);

                g.FillRectangle(netBrush, lx + 205, ly + 28, 14, 14);
                g.DrawString("المتبقي", lFont, Brushes.White, lx + 225, ly + 25);
            }
        }

        private static string FormatMoney(decimal value)
        {
            return value.ToString("N0") + " $";
        }
    }
}