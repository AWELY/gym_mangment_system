using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    /// <summary>
    /// Self-contained, in-app financial report (income statement) with print preview.
    /// Uses the built-in .NET printing engine — no Crystal Reports runtime required.
    /// Mirrors the members report styling.
    /// </summary>
    public sealed class FinanceReportForm : PrintPreviewDialog
    {
        private readonly PrintDocument _doc = new PrintDocument();

        private static readonly Color Teal     = Color.FromArgb(15, 118, 110);
        private static readonly Color Green     = Color.FromArgb(0, 140, 60);
        private static readonly Color Red       = Color.FromArgb(231, 0, 11);
        private static readonly Color Blue      = Color.FromArgb(43, 127, 255);
        private static readonly Color GridLine  = Color.FromArgb(210, 210, 210);
        private static readonly Color TextColor = Color.FromArgb(30, 30, 30);
        private static readonly Color MutedColor = Color.FromArgb(110, 110, 120);

        public FinanceReportForm()
        {
            Text = "التقرير المالي";
            UseAntiAlias = true;
            WindowState = FormWindowState.Maximized;
            ImageAssets.ApplyAppIcon(this);

            _doc.DocumentName = "التقرير المالي";
            _doc.DefaultPageSettings.Landscape = false;
            _doc.PrintPage += Doc_PrintPage;

            Document = _doc;
        }

        private static string Money(decimal v) => v.ToString("N0") + " د.ل";

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            decimal subs     = GymDataStore.TotalSubscriptionRevenue();
            decimal store    = GymDataStore.TotalStoreRevenue();
            decimal salaries = GymDataStore.TotalMonthlySalaries();
            decimal revenue  = subs + store;
            decimal net      = revenue - salaries;
            decimal margin   = revenue > 0 ? (net / revenue) * 100m : 0m;

            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle area = e.MarginBounds;

            using (var titleFont = new Font("Segoe UI", 18F, FontStyle.Bold))
            using (var subFont   = new Font("Segoe UI", 10F))
            using (var kpiTitle  = new Font("Segoe UI", 9F))
            using (var kpiValue  = new Font("Segoe UI", 13F, FontStyle.Bold))
            using (var secFont   = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (var rowFont   = new Font("Segoe UI", 11F))
            using (var totFont   = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (var footFont  = new Font("Segoe UI", 8F))
            using (var rtl = new StringFormat(StringFormatFlags.DirectionRightToLeft)
                   { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var rtlRight = new StringFormat(StringFormatFlags.DirectionRightToLeft)
                   { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center })
            using (var rtlLeft = new StringFormat(StringFormatFlags.DirectionRightToLeft)
                   { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            using (var tealBrush = new SolidBrush(Teal))
            using (var white = new SolidBrush(Color.White))
            using (var textBrush = new SolidBrush(TextColor))
            using (var mutedBrush = new SolidBrush(MutedColor))
            using (var gridPen = new Pen(GridLine))
            {
                int y = area.Top;

                // ── Title band ──
                g.FillRectangle(tealBrush, area.Left, y, area.Width, 46);
                g.DrawString("Glory Gym  —  التقرير المالي", titleFont, white,
                    new Rectangle(area.Left, y, area.Width, 46), rtl);
                y += 54;

                g.DrawString("تاريخ التقرير: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    subFont, mutedBrush, new Rectangle(area.Left, y, area.Width, 20), rtl);
                y += 30;

                // ── KPI boxes ──
                var kpis = new[]
                {
                    Tuple.Create("إجمالي الإيرادات", Money(revenue),               Green),
                    Tuple.Create("المصروفات (رواتب/شهر)", "-" + Money(salaries),    Red),
                    Tuple.Create("صافي الربح", Money(net),                          net >= 0 ? Green : Red),
                    Tuple.Create("هامش الربح", margin.ToString("0.#") + "%",        net >= 0 ? Blue : Red),
                };
                int gap = 12;
                int boxW = (area.Width - gap * 3) / 4;
                int boxH = 70;
                for (int i = 0; i < kpis.Length; i++)
                {
                    // right-to-left placement: first KPI on the right
                    int bx = area.Right - boxW - i * (boxW + gap);
                    var rect = new Rectangle(bx, y, boxW, boxH);
                    using (var border = new Pen(GridLine))
                        g.DrawRectangle(border, rect);
                    using (var accent = new SolidBrush(kpis[i].Item3))
                        g.FillRectangle(accent, bx, y + boxH - 5, boxW, 5);
                    g.DrawString(kpis[i].Item1, kpiTitle, mutedBrush,
                        new Rectangle(bx, y + 8, boxW, 20), rtl);
                    using (var vb = new SolidBrush(kpis[i].Item3))
                        g.DrawString(kpis[i].Item2, kpiValue, vb,
                            new Rectangle(bx, y + 26, boxW, 30), rtl);
                }
                y += boxH + 28;

                // ── Income statement ──
                g.DrawString("قائمة الدخل", secFont, textBrush,
                    new Rectangle(area.Left, y, area.Width, 26), rtlRight);
                y += 32;

                int amountW = (int)(area.Width * 0.40);
                int labelX = area.Left + amountW;     // label region (right side)
                int labelW = area.Width - amountW;
                const int rowH = 34;

                Action<string, string, Color, Font, bool> drawRow = (label, amount, amountColor, font, line) =>
                {
                    if (line)
                        g.DrawLine(gridPen, area.Left, y, area.Right, y);
                    using (var ab = new SolidBrush(amountColor))
                        g.DrawString(amount, font, ab,
                            new Rectangle(area.Left, y, amountW, rowH), rtlLeft);
                    g.DrawString(label, font, textBrush,
                        new Rectangle(labelX, y, labelW, rowH), rtlRight);
                    y += rowH;
                };

                // Revenue section
                g.DrawString("الإيرادات", rowFont, mutedBrush,
                    new Rectangle(labelX, y, labelW, rowH), rtlRight);
                y += rowH;
                drawRow("اشتراكات الأعضاء", Money(subs),  Green, rowFont, false);
                drawRow("مبيعات المتجر",   Money(store), Green, rowFont, false);
                drawRow("إجمالي الإيرادات", Money(revenue), Green, totFont, true);

                y += 6;
                // Expenses section
                g.DrawString("المصروفات", rowFont, mutedBrush,
                    new Rectangle(labelX, y, labelW, rowH), rtlRight);
                y += rowH;
                drawRow("رواتب المدربين (شهرياً)", "-" + Money(salaries), Red, rowFont, false);

                y += 6;
                drawRow("صافي الربح", Money(net), net >= 0 ? Green : Red, totFont, true);

                // ── Counts summary ──
                y += 28;
                g.DrawString("ملخص عام", secFont, textBrush,
                    new Rectangle(area.Left, y, area.Width, 26), rtlRight);
                y += 32;
                drawRow("عدد الأعضاء", GymDataStore.Data.Members.Count.ToString(), TextColor, rowFont, true);
                drawRow("عدد المدربين", GymDataStore.Data.Trainers.Count.ToString(), TextColor, rowFont, false);
                drawRow("عدد منتجات المتجر", GymDataStore.Data.StoreProducts.Count.ToString(), TextColor, rowFont, false);
                drawRow("عدد عمليات البيع", GymDataStore.Data.StoreSales.Count.ToString(), TextColor, rowFont, false);

                // ── Footer ──
                g.DrawString("Glory Gym", footFont, mutedBrush,
                    new Rectangle(area.Left, area.Bottom - 18, area.Width, 16), rtl);

                e.HasMorePages = false;
            }
        }
    }
}
