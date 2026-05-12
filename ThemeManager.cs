using System;
using System.Drawing;
using System.Windows.Forms;
using gym_mangment_system.Properties;

namespace gym_mangment_system
{
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
            formBackground: Color.FromArgb(18, 18, 22),
            sidebar: Color.FromArgb(22, 22, 26),
            sidebarNav: Color.FromArgb(22, 22, 26),
            sidebarNavActive: Color.FromArgb(40, 40, 48),
            topBar: Color.FromArgb(26, 26, 30),
            statusBar: Color.FromArgb(18, 18, 22),
            contentHost: Color.FromArgb(18, 18, 22),
            textPrimary: Color.White,
            textMuted: Color.FromArgb(160, 160, 170),
            textOnAccent: Color.White,
            panel: Color.FromArgb(32, 32, 38),
            panelElevated: Color.FromArgb(38, 38, 45),
            card: Color.FromArgb(32, 32, 40),
            cardHover: Color.FromArgb(42, 42, 52),
            notifCard: Color.FromArgb(38, 38, 45),
            notifCardHover: Color.FromArgb(48, 48, 55),
            quickPanel: Color.FromArgb(22, 22, 28),
            tableHeaderBand: Color.FromArgb(70, 70, 82),
            inputBackground: Color.FromArgb(38, 38, 45),
            inputForeground: Color.White,
            listBackground: Color.FromArgb(30, 30, 36),
            listForeground: Color.White,
            gridBackground: Color.FromArgb(12, 12, 14),
            gridRow: Color.FromArgb(18, 18, 24),
            gridRowAlt: Color.FromArgb(24, 24, 30),
            gridHeader: Color.FromArgb(30, 30, 38),
            gridSelection: Color.FromArgb(40, 40, 52),
            gridSelectionFore: Color.White,
            gridLines: Color.FromArgb(50, 50, 50),
            secondaryButton: Color.FromArgb(55, 55, 65),
            secondaryButtonHover: Color.FromArgb(75, 75, 88),
            borderSubtle: Color.FromArgb(50, 50, 58),
            topBarButtonHover: Color.FromArgb(45, 45, 50)
        );

        public static UiColorScheme Light { get; } = new UiColorScheme(
            formBackground: Color.FromArgb(245, 246, 250),
            sidebar: Color.FromArgb(255, 255, 255),
            sidebarNav: Color.FromArgb(248, 249, 252),
            sidebarNavActive: Color.FromArgb(230, 235, 245),
            topBar: Color.FromArgb(252, 252, 254),
            statusBar: Color.FromArgb(240, 242, 247),
            contentHost: Color.FromArgb(245, 246, 250),
            textPrimary: Color.FromArgb(28, 32, 40),
            textMuted: Color.FromArgb(95, 100, 115),
            textOnAccent: Color.White,
            panel: Color.FromArgb(255, 255, 255),
            panelElevated: Color.FromArgb(248, 249, 252),
            card: Color.FromArgb(255, 255, 255),
            cardHover: Color.FromArgb(236, 240, 248),
            notifCard: Color.FromArgb(255, 255, 255),
            notifCardHover: Color.FromArgb(236, 240, 248),
            quickPanel: Color.FromArgb(248, 249, 252),
            tableHeaderBand: Color.FromArgb(220, 226, 238),
            inputBackground: Color.FromArgb(255, 255, 255),
            inputForeground: Color.FromArgb(28, 32, 40),
            listBackground: Color.FromArgb(255, 255, 255),
            listForeground: Color.FromArgb(28, 32, 40),
            gridBackground: Color.FromArgb(252, 252, 254),
            gridRow: Color.FromArgb(255, 255, 255),
            gridRowAlt: Color.FromArgb(248, 249, 252),
            gridHeader: Color.FromArgb(228, 232, 240),
            gridSelection: Color.FromArgb(187, 212, 255),
            gridSelectionFore: Color.FromArgb(20, 40, 90),
            gridLines: Color.FromArgb(210, 216, 225),
            secondaryButton: Color.FromArgb(210, 218, 232),
            secondaryButtonHover: Color.FromArgb(195, 205, 225),
            borderSubtle: Color.FromArgb(210, 216, 225),
            topBarButtonHover: Color.FromArgb(236, 240, 248)
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
                _isLight = false;
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
            dgv.ColumnHeadersDefaultCellStyle.BackColor = s.GridHeader;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = IsLight ? s.TextPrimary : Color.White;
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
