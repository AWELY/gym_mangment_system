using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace gym_mangment_system
{
    /// <summary>
    /// A small, themed, RTL-friendly replacement for the native message box where
    /// the caption and message are horizontally centered. Built by hand (no
    /// designer) so the layout and text alignment are fully under our control.
    /// </summary>
    internal sealed class GunaMessageForm : Form
    {
        private const int Pad     = 26;
        private const int Radius  = 16;
        private readonly Color _border;

        internal static DialogResult Show(IWin32Window owner, string text, string caption,
            MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            using (var dlg = new GunaMessageForm(text, caption, buttons, icon))
            {
                if (owner is IWin32Window)
                {
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    return dlg.ShowDialog(owner);
                }

                dlg.StartPosition = FormStartPosition.CenterScreen;
                return dlg.ShowDialog();
            }
        }

        private GunaMessageForm(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            UiColorScheme s = ThemeManager.Current;
            _border = s.BorderSubtle;

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar   = false;
            RightToLeft     = RightToLeft.Yes;
            BackColor       = s.Panel;
            Font            = new Font("Segoe UI", 10F);
            KeyPreview      = true;
            MaximizeBox     = false;
            MinimizeBox     = false;

            text    = text ?? string.Empty;
            caption = caption ?? string.Empty;

            Color accent;
            string glyph;
            GetIcon(icon, out accent, out glyph);

            var msgFont = new Font("Segoe UI", 11.5F);
            var capFont = new Font("Segoe UI", 13F, FontStyle.Bold);
            var btnFont = new Font("Segoe UI", 10F, FontStyle.Bold);

            const int maxWidth = 560;
            const int minWidth = 360;
            const TextFormatFlags wrap = TextFormatFlags.WordBreak | TextFormatFlags.RightToLeft;

            int contentWidth = maxWidth - Pad * 2;
            Size msgSize = TextRenderer.MeasureText(text, msgFont, new Size(contentWidth, 0), wrap);

            int formWidth = Math.Max(minWidth, Math.Min(maxWidth, msgSize.Width + Pad * 2));
            contentWidth  = formWidth - Pad * 2;
            msgSize       = TextRenderer.MeasureText(text, msgFont, new Size(contentWidth, 0), wrap);

            int y = Pad;

            if (!string.IsNullOrEmpty(glyph))
            {
                Controls.Add(CenteredLabel(glyph, new Font("Segoe UI Symbol", 26F, FontStyle.Bold),
                    accent, new Rectangle(Pad, y, contentWidth, 46)));
                y += 54;
            }

            if (!string.IsNullOrEmpty(caption))
            {
                Controls.Add(CenteredLabel(caption, capFont, s.TextPrimary,
                    new Rectangle(Pad, y, contentWidth, 30)));
                y += 38;
            }

            Controls.Add(CenteredLabel(text, msgFont, s.TextPrimary,
                new Rectangle(Pad, y, contentWidth, msgSize.Height)));
            y += msgSize.Height + 24;

            Color primaryFill = accent.IsEmpty ? FigmaPalette.Primary : accent;
            var buttonList = new List<Guna2Button>();
            int totalWidth = 0;
            const int btnH = 40, gap = 12;

            foreach (var def in ButtonDefs(buttons))
            {
                int w = Math.Max(100, TextRenderer.MeasureText(def.Text, btnFont).Width + 36);
                var button = new Guna2Button
                {
                    Text         = def.Text,
                    Size         = new Size(w, btnH),
                    BorderRadius = 10,
                    Font         = btnFont,
                    Cursor       = Cursors.Hand,
                    DialogResult = def.Result,
                    ForeColor    = def.Primary ? Color.White : s.TextPrimary,
                    FillColor    = def.Primary ? primaryFill : s.SecondaryButton
                };

                // Guna2Button does not auto-propagate its DialogResult to the form on
                // click (unlike the native Button), so close the dialog explicitly.
                DialogResult result = def.Result;
                button.Click += (sender, args) =>
                {
                    DialogResult = result;
                    Close();
                };

                buttonList.Add(button);
                totalWidth += w;
            }
            totalWidth += gap * Math.Max(0, buttonList.Count - 1);

            int bx = (formWidth - totalWidth) / 2;
            foreach (var b in buttonList)
            {
                b.Location = new Point(bx, y);
                Controls.Add(b);
                bx += b.Width + gap;
            }

            y += btnH + Pad;
            ClientSize = new Size(formWidth, y);

            if (buttonList.Count > 0)
            {
                AcceptButton = buttonList[0];
                var cancel = buttonList.Find(b =>
                    b.DialogResult == DialogResult.Cancel || b.DialogResult == DialogResult.No);
                if (cancel != null) CancelButton = cancel;
            }

            Region = RoundedRegion(ClientSize, Radius);
        }

        private static Label CenteredLabel(string text, Font font, Color fore, Rectangle bounds)
        {
            return new Label
            {
                Text      = text,
                Font      = font,
                ForeColor = fore,
                AutoSize  = false,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Bounds    = bounds
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedPath(new Rectangle(0, 0, Width - 1, Height - 1), Radius))
            using (var pen = new Pen(_border))
                e.Graphics.DrawPath(pen, path);
        }

        private static Region RoundedRegion(Size size, int radius)
        {
            using (var path = RoundedPath(new Rectangle(0, 0, size.Width, size.Height), radius))
                return new Region(path);
        }

        private static GraphicsPath RoundedPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void GetIcon(MessageBoxIcon icon, out Color accent, out string glyph)
        {
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    accent = FigmaPalette.Red;    glyph = "\u2716"; break;   // ✖
                case MessageBoxIcon.Warning:
                    accent = FigmaPalette.Orange; glyph = "\u26A0"; break;   // ⚠
                case MessageBoxIcon.Question:
                    accent = FigmaPalette.Primary; glyph = "\u003F"; break;  // ?
                case MessageBoxIcon.Information:
                    accent = FigmaPalette.Blue;   glyph = "\u2139"; break;   // ℹ
                default:
                    accent = Color.Empty;         glyph = string.Empty; break;
            }
        }

        private struct ButtonDef
        {
            public string Text;
            public DialogResult Result;
            public bool Primary;
            public ButtonDef(string text, DialogResult result, bool primary)
            {
                Text = text; Result = result; Primary = primary;
            }
        }

        private static IEnumerable<ButtonDef> ButtonDefs(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OKCancel:
                    return new[]
                    {
                        new ButtonDef("موافق", DialogResult.OK, true),
                        new ButtonDef("إلغاء", DialogResult.Cancel, false)
                    };
                case MessageBoxButtons.YesNo:
                    return new[]
                    {
                        new ButtonDef("نعم", DialogResult.Yes, true),
                        new ButtonDef("لا", DialogResult.No, false)
                    };
                case MessageBoxButtons.YesNoCancel:
                    return new[]
                    {
                        new ButtonDef("نعم", DialogResult.Yes, true),
                        new ButtonDef("لا", DialogResult.No, false),
                        new ButtonDef("إلغاء", DialogResult.Cancel, false)
                    };
                case MessageBoxButtons.RetryCancel:
                    return new[]
                    {
                        new ButtonDef("إعادة المحاولة", DialogResult.Retry, true),
                        new ButtonDef("إلغاء", DialogResult.Cancel, false)
                    };
                case MessageBoxButtons.AbortRetryIgnore:
                    return new[]
                    {
                        new ButtonDef("إيقاف", DialogResult.Abort, true),
                        new ButtonDef("إعادة المحاولة", DialogResult.Retry, false),
                        new ButtonDef("تجاهل", DialogResult.Ignore, false)
                    };
                default:
                    return new[] { new ButtonDef("موافق", DialogResult.OK, true) };
            }
        }
    }
}
