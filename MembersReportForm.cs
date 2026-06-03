using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    /// <summary>
    /// Self-contained, in-app members report. Uses the built-in .NET printing
    /// engine (no Crystal Reports runtime required) and shows a print preview
    /// with a print button so the full members list can be sent to any printer.
    /// </summary>
    public sealed class MembersReportForm : PrintPreviewDialog
    {
        private readonly PrintDocument _doc = new PrintDocument();
        private List<MemberRecord> _members = new List<MemberRecord>();
        private int _rowIndex;
        private int _pageNumber;

        // Display order is right-to-left (الاسم on the far right).
        private static readonly string[] Headers =
            { "تاريخ الانضمام", "المدة", "السعر", "الاشتراك", "الجنس", "الهاتف", "الاسم" };
        private static readonly float[] Widths =
            { 0.16f, 0.12f, 0.12f, 0.16f, 0.09f, 0.15f, 0.20f };

        private static readonly Color Teal      = Color.FromArgb(15, 118, 110);
        private static readonly Color AltRow     = Color.FromArgb(245, 247, 246);
        private static readonly Color GridLine   = Color.FromArgb(210, 210, 210);
        private static readonly Color TextColor  = Color.FromArgb(30, 30, 30);
        private static readonly Color PriceColor = Color.FromArgb(0, 140, 60);

        public MembersReportForm()
        {
            Text = "تقرير الأعضاء";
            UseAntiAlias = true;
            WindowState = FormWindowState.Maximized;
            ImageAssets.ApplyAppIcon(this);

            _doc.DocumentName = "تقرير الأعضاء";
            _doc.DefaultPageSettings.Landscape = true;
            _doc.BeginPrint += Doc_BeginPrint;
            _doc.PrintPage  += Doc_PrintPage;

            Document = _doc;
        }

        private void Doc_BeginPrint(object sender, PrintEventArgs e)
        {
            _members = new List<MemberRecord>(GymDataStore.Data.Members);
            _members.Sort((a, b) => a.Id.CompareTo(b.Id));
            _rowIndex = 0;
            _pageNumber = 0;
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            _pageNumber++;
            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Rectangle area = e.MarginBounds;

            using (var titleFont = new Font("Segoe UI", 18F, FontStyle.Bold))
            using (var subFont   = new Font("Segoe UI", 10F))
            using (var headFont  = new Font("Segoe UI", 10F, FontStyle.Bold))
            using (var cellFont  = new Font("Segoe UI", 9F))
            using (var footFont  = new Font("Segoe UI", 8F))
            using (var rtlCenter = new StringFormat(StringFormatFlags.DirectionRightToLeft)
                   { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var headBg   = new SolidBrush(Teal))
            using (var headText = new SolidBrush(Color.White))
            using (var altBrush = new SolidBrush(AltRow))
            using (var textBrush  = new SolidBrush(TextColor))
            using (var priceBrush = new SolidBrush(PriceColor))
            using (var gridPen  = new Pen(GridLine))
            {
                int y = area.Top;

                // ── Title band ──
                g.FillRectangle(headBg, area.Left, y, area.Width, 46);
                g.DrawString("Glory Gym  —  تقرير الأعضاء", titleFont, headText,
                    new Rectangle(area.Left, y, area.Width, 46), rtlCenter);
                y += 54;

                string sub = "إجمالي الأعضاء: " + _members.Count +
                             "        تاريخ الطباعة: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                g.DrawString(sub, subFont, textBrush,
                    new Rectangle(area.Left, y, area.Width, 22), rtlCenter);
                y += 30;

                // ── Column x-positions ──
                int[] colX = new int[Headers.Length + 1];
                colX[0] = area.Left;
                for (int i = 0; i < Headers.Length; i++)
                    colX[i + 1] = colX[i] + (int)(area.Width * Widths[i]);
                colX[Headers.Length] = area.Right;

                const int rowH = 30;
                int tableTop = y;

                // ── Header row ──
                g.FillRectangle(headBg, area.Left, y, area.Width, rowH);
                for (int i = 0; i < Headers.Length; i++)
                    g.DrawString(Headers[i], headFont, headText,
                        new Rectangle(colX[i], y, colX[i + 1] - colX[i], rowH), rtlCenter);
                y += rowH;

                // ── Data rows ──
                bool alt = false;
                while (_rowIndex < _members.Count)
                {
                    if (y + rowH > area.Bottom - 24)
                    {
                        DrawGrid(g, colX, area, tableTop, y, gridPen);
                        DrawFooter(g, area, footFont, textBrush, rtlCenter);
                        e.HasMorePages = true;
                        return;
                    }

                    MemberRecord m = _members[_rowIndex];
                    if (alt)
                        g.FillRectangle(altBrush, area.Left, y, area.Width, rowH);
                    alt = !alt;

                    string[] cells =
                    {
                        m.JoinDate ?? "",
                        m.DurationText ?? "",
                        m.PriceText ?? "",
                        m.PlanName ?? "",
                        m.Gender ?? "",
                        m.Phone ?? "",
                        m.FullName ?? ""
                    };

                    for (int i = 0; i < cells.Length; i++)
                    {
                        Brush b = (i == 2) ? priceBrush : textBrush; // 2 = السعر
                        g.DrawString(cells[i], cellFont, b,
                            new Rectangle(colX[i], y, colX[i + 1] - colX[i], rowH), rtlCenter);
                    }

                    y += rowH;
                    _rowIndex++;
                }

                DrawGrid(g, colX, area, tableTop, y, gridPen);
                DrawFooter(g, area, footFont, textBrush, rtlCenter);
                e.HasMorePages = false;
            }
        }

        private static void DrawGrid(Graphics g, int[] colX, Rectangle area, int top, int bottom, Pen pen)
        {
            for (int i = 0; i < colX.Length; i++)
                g.DrawLine(pen, colX[i], top, colX[i], bottom);
            g.DrawLine(pen, area.Left, top, area.Right, top);
            g.DrawLine(pen, area.Left, bottom, area.Right, bottom);
        }

        private void DrawFooter(Graphics g, Rectangle area, Font font, Brush brush, StringFormat fmt)
        {
            g.DrawString("صفحة " + _pageNumber, font, brush,
                new Rectangle(area.Left, area.Bottom - 18, area.Width, 16), fmt);
        }
    }
}
