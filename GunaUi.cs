using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace gym_mangment_system
{
    /// <summary>
    /// Small factory helpers that build Guna.UI2 controls styled to the Figma look,
    /// so every page shares the same rounded-card / button language.
    /// </summary>
    internal static class GunaUi
    {
        /// <summary>A rounded, soft-shadow card panel (Figma card).</summary>
        internal static Guna2Panel Card(int width, int height, Color fill, Color border)
        {
            var card = new Guna2Panel
            {
                Size            = new Size(width, height),
                Margin          = new Padding(10),
                FillColor       = fill,
                BorderRadius    = 14,
                BorderColor     = border,
                BorderThickness = 1
            };
            card.ShadowDecoration.Enabled = true;
            card.ShadowDecoration.Depth   = 8;
            return card;
        }

        /// <summary>A rounded solid action button (add / edit / delete / send).</summary>
        internal static Guna2Button Button(string text, Color fill, Point location, Size size)
        {
            return new Guna2Button
            {
                Text         = text,
                FillColor    = fill,
                ForeColor    = Color.White,
                Font         = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderRadius = 8,
                Location     = location,
                Size         = size,
                Cursor       = Cursors.Hand
            };
        }

        /// <summary>
        /// A larger primary toolbar button (e.g. "+ إضافة"). Rendered with the brand
        /// purple→pink gradient to match the Figma design. The <paramref name="fill"/>
        /// argument is kept for call-site compatibility but the brand gradient is used.
        /// </summary>
        internal static Guna2GradientButton ToolbarButton(string text, Color fill, Point location)
        {
            return new Guna2GradientButton
            {
                Text         = text,
                FillColor    = FigmaPalette.GradientStart,
                FillColor2   = FigmaPalette.GradientEnd,
                ForeColor    = Color.White,
                Font         = new Font("Segoe UI", 11F, FontStyle.Bold),
                BorderRadius = 12,
                Location     = location,
                Size         = new Size(165, 40),
                Cursor       = Cursors.Hand
            };
        }

        /// <summary>Apply the brand purple→pink gradient to a gradient button.</summary>
        internal static void ApplyBrandGradient(Guna2GradientButton btn)
        {
            if (btn == null) return;
            btn.FillColor  = FigmaPalette.GradientStart;
            btn.FillColor2 = FigmaPalette.GradientEnd;
            btn.ForeColor  = Color.White;
            if (btn.BorderRadius < 10) btn.BorderRadius = 12;
        }

        // ---------------------------------------------------------------------
        // Guna styled message dialogs — drop-in replacements for MessageBox.Show
        // so the whole app shares one themed, RTL-friendly dialog instead of the
        // native Windows message boxes.
        // ---------------------------------------------------------------------

        internal static DialogResult Show(string text)
            => Core(null, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);

        internal static DialogResult Show(string text, string caption)
            => Core(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);

        internal static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
            => Core(null, text, caption, buttons, MessageBoxIcon.None);

        internal static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
            => Core(null, text, caption, buttons, icon);

        internal static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
            => Core(owner, text, caption, buttons, icon);

        private static DialogResult Core(IWin32Window owner, string text, string caption,
            MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            var dialog = new Guna2MessageDialog
            {
                Caption = caption ?? string.Empty,
                Text    = text ?? string.Empty,
                Buttons = MapButtons(buttons),
                Icon    = MapIcon(icon),
                Style   = ThemeManager.IsLight ? MessageDialogStyle.Light : MessageDialogStyle.Dark
            };

            if (owner is Form ownerForm)
                dialog.Parent = ownerForm;

            return dialog.Show();
        }

        private static MessageDialogButtons MapButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OKCancel:        return MessageDialogButtons.OKCancel;
                case MessageBoxButtons.YesNo:           return MessageDialogButtons.YesNo;
                case MessageBoxButtons.YesNoCancel:     return MessageDialogButtons.YesNoCancel;
                case MessageBoxButtons.RetryCancel:     return MessageDialogButtons.RetryCancel;
                case MessageBoxButtons.AbortRetryIgnore:return MessageDialogButtons.AbortRetryIgnore;
                default:                                return MessageDialogButtons.OK;
            }
        }

        private static MessageDialogIcon MapIcon(MessageBoxIcon icon)
        {
            // MessageBoxIcon members share values (Error/Stop/Hand = 16, etc.),
            // so switching on the documented names covers every alias.
            switch (icon)
            {
                case MessageBoxIcon.Error:       return MessageDialogIcon.Error;       // Error/Stop/Hand
                case MessageBoxIcon.Question:    return MessageDialogIcon.Question;
                case MessageBoxIcon.Warning:     return MessageDialogIcon.Warning;     // Warning/Exclamation
                case MessageBoxIcon.Information:  return MessageDialogIcon.Information;  // Information/Asterisk
                default:                         return MessageDialogIcon.None;
            }
        }
    }
}
