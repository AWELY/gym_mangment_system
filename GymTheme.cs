using System.Drawing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    internal enum GymThemeMode
    {
        Dark,
        Light
    }

    internal sealed class GymThemePalette
    {
        public Color Background { get; set; }
        public Color Surface { get; set; }
        public Color Panel { get; set; }
        public Color Card { get; set; }
        public Color CardHover { get; set; }
        public Color Sidebar { get; set; }
        public Color TopBar { get; set; }
        public Color StatusBar { get; set; }
        public Color Text { get; set; }
        public Color MutedText { get; set; }
        public Color ButtonBack { get; set; }
        public Color ButtonHover { get; set; }
        public Color ButtonText { get; set; }
        public Color InputBack { get; set; }
        public Color InputText { get; set; }
        public Color Primary { get; set; }
        public Color Accent { get; set; }
        public Color Success { get; set; }
        public Color Warning { get; set; }
        public Color NavNormal { get; set; }
        public Color NavActive { get; set; }
    }

    internal static class GymTheme
    {
        public static GymThemeMode Mode { get; private set; } = GymThemeMode.Dark;

        public static GymThemePalette Current => Mode == GymThemeMode.Dark ? Dark : Light;

        public static bool IsDark => Mode == GymThemeMode.Dark;

        public static void Toggle()
        {
            Mode = Mode == GymThemeMode.Dark ? GymThemeMode.Light : GymThemeMode.Dark;
        }

        private static readonly GymThemePalette Dark = new GymThemePalette
        {
            Background = Color.FromArgb(18, 18, 22),
            Surface = Color.FromArgb(22, 22, 28),
            Panel = Color.FromArgb(26, 26, 32),
            Card = Color.FromArgb(32, 32, 40),
            CardHover = Color.FromArgb(42, 42, 52),
            Sidebar = Color.FromArgb(22, 22, 26),
            TopBar = Color.FromArgb(26, 26, 30),
            StatusBar = Color.FromArgb(18, 18, 22),
            Text = Color.White,
            MutedText = Color.FromArgb(160, 160, 175),
            ButtonBack = Color.FromArgb(55, 55, 65),
            ButtonHover = Color.FromArgb(75, 75, 88),
            ButtonText = Color.White,
            InputBack = Color.FromArgb(30, 30, 36),
            InputText = Color.White,
            Primary = Color.FromArgb(33, 150, 243),
            Accent = Color.FromArgb(220, 53, 69),
            Success = Color.FromArgb(76, 175, 80),
            Warning = Color.FromArgb(255, 193, 7),
            NavNormal = Color.FromArgb(160, 160, 170),
            NavActive = Color.White
        };

        private static readonly GymThemePalette Light = new GymThemePalette
        {
            Background = Color.FromArgb(235, 243, 255),
            Surface = Color.FromArgb(255, 255, 255),
            Panel = Color.FromArgb(222, 236, 255),
            Card = Color.FromArgb(255, 255, 255),
            CardHover = Color.FromArgb(214, 231, 255),
            Sidebar = Color.FromArgb(13, 71, 161),
            TopBar = Color.FromArgb(21, 101, 192),
            StatusBar = Color.FromArgb(198, 40, 40),
            Text = Color.FromArgb(20, 35, 55),
            MutedText = Color.FromArgb(76, 91, 115),
            ButtonBack = Color.FromArgb(198, 40, 40),
            ButtonHover = Color.FromArgb(229, 57, 53),
            ButtonText = Color.White,
            InputBack = Color.White,
            InputText = Color.FromArgb(20, 35, 55),
            Primary = Color.FromArgb(21, 101, 192),
            Accent = Color.FromArgb(198, 40, 40),
            Success = Color.FromArgb(46, 125, 50),
            Warning = Color.FromArgb(245, 124, 0),
            NavNormal = Color.FromArgb(210, 226, 255),
            NavActive = Color.White
        };

        public static void ApplyTo(Control root)
        {
            if (root == null) return;

            ApplyControl(root, Current);

            foreach (Control child in root.Controls)
                ApplyTo(child);
        }

        private static void ApplyControl(Control control, GymThemePalette palette)
        {
            if (control is DataGridView grid)
            {
                ApplyGrid(grid, palette);
                return;
            }

            if (control is Button button)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = palette.ButtonBack;
                button.ForeColor = palette.ButtonText;
                button.UseVisualStyleBackColor = false;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = palette.ButtonHover;
                return;
            }

            if (control is TextBox || control is ComboBox || control is ListBox ||
                control is NumericUpDown || control is DateTimePicker)
            {
                control.BackColor = palette.InputBack;
                control.ForeColor = palette.InputText;
                return;
            }

            if (control is Label)
            {
                control.ForeColor = palette.Text;
                if (control.BackColor != Color.Transparent)
                    control.BackColor = palette.Panel;
                return;
            }

            if (control is Form)
            {
                control.BackColor = palette.Background;
                control.ForeColor = palette.Text;
                return;
            }

            if (control is Panel || control is FlowLayoutPanel || control is TableLayoutPanel || control is GroupBox)
            {
                control.BackColor = palette.Panel;
                control.ForeColor = palette.Text;
            }
        }

        private static void ApplyGrid(DataGridView grid, GymThemePalette palette)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = palette.Background;
            grid.GridColor = palette.Panel;
            grid.DefaultCellStyle.BackColor = palette.Surface;
            grid.DefaultCellStyle.ForeColor = palette.Text;
            grid.DefaultCellStyle.SelectionBackColor = palette.Primary;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = palette.Panel;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = palette.Text;
            grid.ColumnHeadersDefaultCellStyle.BackColor = palette.Primary;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.RowHeadersDefaultCellStyle.BackColor = palette.Panel;
            grid.RowHeadersDefaultCellStyle.ForeColor = palette.Text;
        }
    }
}
