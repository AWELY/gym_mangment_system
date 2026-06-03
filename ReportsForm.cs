using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace gym_mangment_system
{
    public partial class ReportsForm : Form, IThemeAware
    {
        private static readonly Color Green = Color.FromArgb(0, 166, 62);
        private static readonly Color Red   = Color.FromArgb(231, 0, 11);
        private static readonly Color Blue  = Color.FromArgb(43, 127, 255);
        private static readonly Color Purple = Color.FromArgb(124, 58, 237);

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

            decimal subscriptions = GymDataStore.TotalSubscriptionRevenue();
            decimal store         = GymDataStore.TotalStoreRevenue();
            decimal salaries      = GymDataStore.TotalMonthlySalaries();
            decimal revenue       = subscriptions + store;
            decimal net           = revenue - salaries;
            decimal margin        = revenue > 0 ? (net / revenue) * 100m : 0m;

            BackColor = s.ContentHost;

            var titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent
            };
            var title = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = s.TextPrimary,
                Padding = new Padding(15, 8, 15, 0),
                Text = "📊  التقرير المالي",
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };
            var btnPrint = new Guna2Button
            {
                Dock = DockStyle.Left,
                Width = 150,
                Text = "🖨️ طباعة التقرير",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                FillColor = Purple,
                BorderRadius = 8,
                Cursor = Cursors.Hand,
                Margin = new Padding(15, 10, 15, 10)
            };
            btnPrint.Click += (_, __) =>
            {
                try
                {
                    using (var report = new FinanceReportForm())
                        report.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    GunaUi.Show("تعذر فتح التقرير المالي.\n\n" + ex.Message,
                        "خطأ في الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Opens the SalesRevune.rpt Crystal report file directly (same as the
            // members report opens GymMembers.rpt).
            var btnSalesReport = new Guna2Button
            {
                Dock = DockStyle.Left,
                Width = 175,
                Text = "📄 تقرير إيرادات المبيعات",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                FillColor = Green,
                BorderRadius = 8,
                Cursor = Cursors.Hand,
                Margin = new Padding(15, 10, 15, 10)
            };
            btnSalesReport.Click += (_, __) => OpenReportFile("SalesRevune.rpt", "إيرادات المبيعات");

            titleBar.Controls.Add(title);
            titleBar.Controls.Add(btnPrint);
            titleBar.Controls.Add(btnSalesReport);

            // ── KPI cards row ──
            var cards = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 140,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(3),
                BackColor = Color.Transparent
            };
            for (int i = 0; i < 4; i++)
                cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            cards.Controls.Add(CreateFinancialCard("💰 إجمالي الإيرادات", FormatMoney(revenue), Green, s), 3, 0);
            cards.Controls.Add(CreateFinancialCard("🏋️ المصروفات (رواتب/شهر)", "-" + FormatMoney(salaries), Red, s), 2, 0);
            cards.Controls.Add(CreateFinancialCard("✅ صافي الربح", FormatMoney(net), net >= 0 ? Green : Red, s), 1, 0);
            cards.Controls.Add(CreateFinancialCard("📈 هامش الربح", margin.ToString("0.#") + "%", net >= 0 ? Blue : Red, s), 0, 0);

            // ── Income statement (قائمة الدخل) ──
            var statement = BuildIncomeStatement(s, subscriptions, store, revenue, salaries, net, margin);

            // ── Monthly cash-flow chart ──
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
            Controls.Add(statement);
            Controls.Add(cards);
            Controls.Add(titleBar);
        }

        // Builds a finance-style income statement card.
        private Control BuildIncomeStatement(UiColorScheme s, decimal subscriptions, decimal store,
                                             decimal revenue, decimal salaries, decimal net, decimal margin)
        {
            var card = new Guna2Panel
            {
                Dock = DockStyle.Top,
                Height = 295,
                FillColor = s.PanelElevated,
                BorderColor = s.BorderSubtle,
                BorderRadius = 12,
                Padding = new Padding(20, 14, 20, 16),
                Margin = new Padding(0, 12, 0, 12)
            };
            card.ShadowDecoration.Enabled = true;

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                BackColor = Color.Transparent
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F)); // amount (left)
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F)); // label  (right)

            int row = 0;
            AddStatementRow(grid, ref row, s, "قائمة الدخل", "", s.TextPrimary, 15F, FontStyle.Bold, false);
            AddStatementRow(grid, ref row, s, "الإيرادات", "", s.TextMuted, 12F, FontStyle.Bold, false);
            AddStatementRow(grid, ref row, s, "اشتراكات الأعضاء", FormatMoney(subscriptions), Green, 12F, FontStyle.Regular, false);
            AddStatementRow(grid, ref row, s, "مبيعات المتجر", FormatMoney(store), Green, 12F, FontStyle.Regular, false);
            AddStatementRow(grid, ref row, s, "إجمالي الإيرادات", FormatMoney(revenue), Green, 12.5F, FontStyle.Bold, true);
            AddStatementRow(grid, ref row, s, "المصروفات", "", s.TextMuted, 12F, FontStyle.Bold, false);
            AddStatementRow(grid, ref row, s, "رواتب المدربين (شهرياً)", "-" + FormatMoney(salaries), Red, 12F, FontStyle.Regular, false);
            AddStatementRow(grid, ref row, s, "صافي الربح", FormatMoney(net), net >= 0 ? Green : Red, 13.5F, FontStyle.Bold, true);

            card.Controls.Add(grid);
            return card;
        }

        private void AddStatementRow(TableLayoutPanel grid, ref int row, UiColorScheme s,
                                     string label, string amount, Color amountColor,
                                     float fontSize, FontStyle style, bool divider)
        {
            grid.RowCount = row + 1;
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));

            var lblAmount = new Label
            {
                Dock = DockStyle.Fill,
                Text = amount,
                ForeColor = amountColor,
                Font = new Font("Segoe UI", fontSize, style),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            var lblTitle = new Label
            {
                Dock = DockStyle.Fill,
                Text = label,
                ForeColor = s.TextPrimary,
                Font = new Font("Segoe UI", fontSize, style),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            if (divider)
            {
                PaintEventHandler topLine = (sender, e) =>
                {
                    using (var pen = new Pen(s.BorderSubtle))
                        e.Graphics.DrawLine(pen, 0, 0, ((Control)sender).Width, 0);
                };
                lblAmount.Paint += topLine;
                lblTitle.Paint += topLine;
            }

            grid.Controls.Add(lblAmount, 0, row);
            grid.Controls.Add(lblTitle, 1, row);
            row++;
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
            int monthCount = Math.Min(dSubs.Length, dStore.Length);
            float[] gross = new float[monthCount];
            for (int i = 0; i < monthCount; i++)
                gross[i] = (float)(dSubs[i] + dStore[i]);
            decimal salaryTotal = 0;
            foreach (var t in GymDataStore.Data.Trainers)
                salaryTotal += t.Salary;
            float salary = (float)salaryTotal;
            float[] net = new float[gross.Length];
            for (int i = 0; i < gross.Length; i++)
                net[i] = gross[i] - salary;
            string[] months = { "ين", "فب", "مار", "أبر", "ماي", "يون", "يول", "أغس", "سبت", "أكت", "نوف", "ديس" };
            float maxGross = 0f;
            for (int i = 0; i < gross.Length; i++)
                if (gross[i] > maxGross) maxGross = gross[i];
            float maxVal = Math.Max(500f, Math.Max(maxGross, salary) * 1.20f);
            int barW = Math.Max(8, chartW / 34);

            using (Font axisFont = new Font("Segoe UI", 8F))
            using (Brush axisBrush = new SolidBrush(s.TextMuted))
            {
                for (int i = 0; i <= 6; i++)
                {
                    int y = padT + (chartH * i / 6);
                    string val = ((int)(maxVal - maxVal * i / 6)).ToString("N0") + " د.ل";
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

        // Opens a Crystal report (.rpt) file with the system's default handler.
        private void OpenReportFile(string fileName, string friendlyName)
        {
            string path = System.IO.Path.Combine(Application.StartupPath, fileName);

            if (!System.IO.File.Exists(path))
            {
                GunaUi.Show(
                    "لم يتم العثور على ملف التقرير:\n" + path,
                    "خطأ في الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                GunaUi.Show(
                    "تعذر فتح ملف التقرير (" + friendlyName + ").\n\nتفاصيل الخطأ:\n" + ex.Message,
                    "خطأ في الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string FormatMoney(decimal value)
        {
            return value.ToString("N0") + " د.ل";
        }
    }
}
