using System;
using System.Drawing;
using System.Windows.Forms;
using gym_mangment_system.Properties;

namespace gym_mangment_system
{
    /// <summary>
    /// Exact color palette extracted from the Glory Gym Figma design
    /// (Tailwind-based). Single source of truth for brand + accent colors.
    /// </summary>
    public static class FigmaPalette
    {
        // Brand / primary — clean professional teal. Calm, near-solid gradient (not flashy).
        // Buttons, active nav pill, logo, prices, banner.
        public static readonly Color Primary      = Color.FromArgb(0x0D, 0x94, 0x88); // teal-600
        public static readonly Color PrimaryHover = Color.FromArgb(0x0F, 0x76, 0x6E); // teal-700
        public static readonly Color PrimarySoft  = Color.FromArgb(0xCC, 0xFB, 0xF1); // teal-100 (soft borders / active nav tint)

        // Brand gradient (subtle teal-700 → teal-600) — keeps a hint of depth without the "fancy" look.
        public static readonly Color GradientStart = Color.FromArgb(0x0F, 0x76, 0x6E); // teal-700
        public static readonly Color GradientEnd   = Color.FromArgb(0x0D, 0x94, 0x88); // teal-600
        public static readonly Color Pink          = Color.FromArgb(0x0E, 0x74, 0x90); // cyan-700 (secondary accent)

        // Logo icon square gradient (teal-500 → teal-600)
        public static readonly Color LogoStart = Color.FromArgb(0x14, 0xB8, 0xA6); // teal-500
        public static readonly Color LogoEnd   = Color.FromArgb(0x0D, 0x94, 0x88); // teal-600

        // Accent colors
        public static readonly Color Blue     = Color.FromArgb(0x2B, 0x7F, 0xFF); // blue-500  (stat: members)
        public static readonly Color BlueBtn  = Color.FromArgb(0x15, 0x5D, 0xFC); // blue-600  (edit button)
        public static readonly Color Green    = Color.FromArgb(0x00, 0xC9, 0x51); // green-500 (value text)
        public static readonly Color GreenBtn = Color.FromArgb(0x00, 0xA6, 0x3E); // green-600 (checkout / add-to-cart / price)
        public static readonly Color Orange   = Color.FromArgb(0xFF, 0x69, 0x00); // orange-500
        public static readonly Color Purple   = Color.FromArgb(0x08, 0x91, 0xB2); // cyan-600 (cart / sales accent)
        public static readonly Color Red      = Color.FromArgb(0xE7, 0x00, 0x0B); // red-600   (delete)
        public static readonly Color Amber    = Color.FromArgb(0xF0, 0xB1, 0x00); // amber     (ticker banner)

        // Semantic badge colors (member gender / user role pills)
        public static readonly Color BadgeMaleBg     = Color.FromArgb(0xDB, 0xEA, 0xFE); // blue-100
        public static readonly Color BadgeMaleText   = Color.FromArgb(0x14, 0x47, 0xE6); // blue-700
        public static readonly Color BadgeFemaleBg   = Color.FromArgb(0xFC, 0xE7, 0xF3); // pink-100
        public static readonly Color BadgeFemaleText = Color.FromArgb(0xC6, 0x00, 0x5C); // pink-700

        // Neutrals (gray scale)
        public static readonly Color Gray50  = Color.FromArgb(0xF9, 0xFA, 0xFB);
        public static readonly Color Gray100 = Color.FromArgb(0xF3, 0xF4, 0xF6);
        public static readonly Color Gray200 = Color.FromArgb(0xE5, 0xE7, 0xEB);
        public static readonly Color Gray300 = Color.FromArgb(0xD1, 0xD5, 0xDB);
        public static readonly Color Gray400 = Color.FromArgb(0x9C, 0xA3, 0xAF);
        public static readonly Color Gray500 = Color.FromArgb(0x6B, 0x72, 0x80);
        public static readonly Color Gray700 = Color.FromArgb(0x37, 0x41, 0x51);
        public static readonly Color Gray900 = Color.FromArgb(0x11, 0x18, 0x27);
        public static readonly Color White   = Color.White;

        /// <summary>Soft teal table header band tint.</summary>
        public static readonly Color TableHeaderTint = Color.FromArgb(0xEC, 0xFE, 0xFF);

        /// <summary>Brand purple→pink gradient brush (horizontal). Caller owns/disposes it.</summary>
        public static System.Drawing.Drawing2D.LinearGradientBrush BrandBrush(Rectangle r)
        {
            if (r.Width <= 0) r.Width = 1;
            if (r.Height <= 0) r.Height = 1;
            return new System.Drawing.Drawing2D.LinearGradientBrush(
                r, GradientStart, GradientEnd, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
        }
    }

    public sealed class UiColorScheme
    {
        public Color FormBackground { get; }
        public Color Sidebar { get; }
        public Color SidebarNav { get; }
        public Color SidebarNavActive { get; }
        public Color TopBar { get; }
        public Color StatusBar { get; }
        public Color ContentHost { get; }

        public Color TextPrimary { get; }
        public Color TextMuted { get; }
        public Color TextOnAccent { get; }

        public Color Panel { get; }
        public Color PanelElevated { get; }
        public Color Card { get; }
        public Color CardHover { get; }
        public Color NotifCard { get; }
        public Color NotifCardHover { get; }
        public Color QuickPanel { get; }
        public Color TableHeaderBand { get; }

        public Color InputBackground { get; }
        public Color InputForeground { get; }
        public Color ListBackground { get; }
        public Color ListForeground { get; }

        public Color GridBackground { get; }
        public Color GridRow { get; }
        public Color GridRowAlt { get; }
        public Color GridHeader { get; }
        public Color GridSelection { get; }
        public Color GridSelectionFore { get; }
        public Color GridLines { get; }

        public Color SecondaryButton { get; }
        public Color SecondaryButtonHover { get; }
        public Color BorderSubtle { get; }

        public Color TopBarButtonHover { get; }

        public UiColorScheme(
            Color formBackground, Color sidebar, Color sidebarNav, Color sidebarNavActive,
            Color topBar, Color statusBar, Color contentHost,
            Color textPrimary, Color textMuted, Color textOnAccent,
            Color panel, Color panelElevated, Color card, Color cardHover,
            Color notifCard, Color notifCardHover, Color quickPanel, Color tableHeaderBand,
            Color inputBackground, Color inputForeground, Color listBackground, Color listForeground,
            Color gridBackground, Color gridRow, Color gridRowAlt, Color gridHeader,
            Color gridSelection, Color gridSelectionFore, Color gridLines,
            Color secondaryButton, Color secondaryButtonHover, Color borderSubtle,
            Color topBarButtonHover)
        {
            FormBackground = formBackground;
            Sidebar = sidebar;
            SidebarNav = sidebarNav;
            SidebarNavActive = sidebarNavActive;
            TopBar = topBar;
            StatusBar = statusBar;
            ContentHost = contentHost;
            TextPrimary = textPrimary;
            TextMuted = textMuted;
            TextOnAccent = textOnAccent;
            Panel = panel;
            PanelElevated = panelElevated;
            Card = card;
            CardHover = cardHover;
            NotifCard = notifCard;
            NotifCardHover = notifCardHover;
            QuickPanel = quickPanel;
            TableHeaderBand = tableHeaderBand;
            InputBackground = inputBackground;
            InputForeground = inputForeground;
            ListBackground = listBackground;
            ListForeground = listForeground;
            GridBackground = gridBackground;
            GridRow = gridRow;
            GridRowAlt = gridRowAlt;
            GridHeader = gridHeader;
            GridSelection = gridSelection;
            GridSelectionFore = gridSelectionFore;
            GridLines = gridLines;
            SecondaryButton = secondaryButton;
            SecondaryButtonHover = secondaryButtonHover;
            BorderSubtle = borderSubtle;
            TopBarButtonHover = topBarButtonHover;
        }

        public static UiColorScheme Dark { get; } = new UiColorScheme(
            formBackground: Color.FromArgb(10, 10, 10),
            sidebar: Color.FromArgb(19, 19, 22),
            sidebarNav: Color.FromArgb(19, 19, 22),
            sidebarNavActive: Color.FromArgb(16, 40, 42),
            topBar: Color.FromArgb(19, 19, 22),
            statusBar: Color.FromArgb(10, 10, 10),
            contentHost: Color.FromArgb(10, 10, 10),
            textPrimary: Color.FromArgb(250, 250, 250),
            textMuted: Color.FromArgb(160, 160, 170),
            textOnAccent: Color.White,
            panel: Color.FromArgb(26, 26, 32),
            panelElevated: Color.FromArgb(32, 32, 40),
            card: Color.FromArgb(26, 26, 36),
            cardHover: Color.FromArgb(38, 38, 50),
            notifCard: Color.FromArgb(32, 32, 40),
            notifCardHover: Color.FromArgb(44, 44, 54),
            quickPanel: Color.FromArgb(19, 19, 24),
            tableHeaderBand: Color.FromArgb(17, 46, 48),
            inputBackground: Color.FromArgb(32, 32, 40),
            inputForeground: Color.White,
            listBackground: Color.FromArgb(26, 26, 34),
            listForeground: Color.White,
            gridBackground: Color.FromArgb(10, 10, 12),
            gridRow: Color.FromArgb(18, 18, 24),
            gridRowAlt: Color.FromArgb(24, 24, 30),
            gridHeader: Color.FromArgb(28, 28, 36),
            gridSelection: Color.FromArgb(17, 46, 48),
            gridSelectionFore: Color.White,
            gridLines: Color.FromArgb(38, 38, 38),
            secondaryButton: Color.FromArgb(45, 45, 55),
            secondaryButtonHover: Color.FromArgb(64, 64, 78),
            borderSubtle: Color.FromArgb(38, 38, 38),
            topBarButtonHover: Color.FromArgb(40, 40, 48)
        );

        // Light theme = exact Glory Gym Figma palette.
        public static UiColorScheme Light { get; } = new UiColorScheme(
            formBackground: FigmaPalette.Gray50,
            sidebar: FigmaPalette.White,
            sidebarNav: FigmaPalette.White,
            sidebarNavActive: FigmaPalette.PrimarySoft,
            topBar: FigmaPalette.White,
            statusBar: FigmaPalette.White,
            contentHost: FigmaPalette.Gray50,
            textPrimary: FigmaPalette.Gray900,
            textMuted: FigmaPalette.Gray500,
            textOnAccent: Color.White,
            panel: FigmaPalette.White,
            panelElevated: FigmaPalette.Gray50,
            card: FigmaPalette.White,
            cardHover: FigmaPalette.Gray100,
            notifCard: FigmaPalette.White,
            notifCardHover: FigmaPalette.Gray100,
            quickPanel: FigmaPalette.Gray50,
            tableHeaderBand: FigmaPalette.TableHeaderTint,
            inputBackground: FigmaPalette.White,
            inputForeground: FigmaPalette.Gray900,
            listBackground: FigmaPalette.White,
            listForeground: FigmaPalette.Gray900,
            gridBackground: FigmaPalette.White,
            gridRow: FigmaPalette.White,
            gridRowAlt: FigmaPalette.Gray50,
            gridHeader: FigmaPalette.TableHeaderTint,
            gridSelection: FigmaPalette.PrimarySoft,
            gridSelectionFore: FigmaPalette.Gray900,
            gridLines: FigmaPalette.Gray200,
            secondaryButton: FigmaPalette.Gray100,
            secondaryButtonHover: FigmaPalette.Gray200,
            borderSubtle: FigmaPalette.PrimarySoft,
            topBarButtonHover: FigmaPalette.Gray100
        );
    }

    public static class ThemeManager
    {
        private static bool _isLight;

        public static bool IsLight => _isLight;

        public static UiColorScheme Current => _isLight ? UiColorScheme.Light : UiColorScheme.Dark;

        public static event EventHandler ThemeChanged;

        public static void LoadFromSettings()
        {
            try
            {
                _isLight = Settings.Default.UseLightTheme;
            }
            catch
            {
                _isLight = true;
            }
        }

        public static void Save()
        {
            try
            {
                Settings.Default.UseLightTheme = _isLight;
                Settings.Default.Save();
            }
            catch { }
        }

        public static void SetLight(bool light)
        {
            if (_isLight == light) return;
            _isLight = light;
            ThemeChanged?.Invoke(null, EventArgs.Empty);
            Save();
        }

        public static void Toggle()
        {
            SetLight(!_isLight);
        }

        public static float BrandingOpacity(bool isDashboard = false)
        {
            if (_isLight) return isDashboard ? 0.35f : 0.12f;
            return isDashboard ? 0.55f : 0.22f;
        }

        public static void StyleDataGridView(DataGridView dgv, UiColorScheme s)
        {
            if (dgv == null) return;
            dgv.BackgroundColor = s.GridBackground;
            dgv.GridColor = s.GridLines;
            dgv.DefaultCellStyle.BackColor = s.GridRow;
            dgv.DefaultCellStyle.ForeColor = s.TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = s.GridSelection;
            dgv.DefaultCellStyle.SelectionForeColor = s.GridSelectionFore;
            // RowsDefaultCellStyle is set (dark) in the designers and takes
            // precedence over DefaultCellStyle for non-alternating rows; override
            // it so light mode does not show dark row striping.
            dgv.RowsDefaultCellStyle.BackColor = s.GridRow;
            dgv.RowsDefaultCellStyle.ForeColor = s.TextPrimary;
            dgv.RowsDefaultCellStyle.SelectionBackColor = s.GridSelection;
            dgv.RowsDefaultCellStyle.SelectionForeColor = s.GridSelectionFore;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = s.GridHeader;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = IsLight ? s.TextMuted : Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = s.GridHeader;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgv.ColumnHeadersDefaultCellStyle.ForeColor;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = s.GridRowAlt;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = s.TextPrimary;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = s.GridSelection;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = s.GridSelectionFore;
            dgv.EnableHeadersVisualStyles = false;
        }
    }
}
