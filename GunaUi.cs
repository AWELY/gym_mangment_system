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
    }
}
