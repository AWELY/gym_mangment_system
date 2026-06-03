using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public static class ImageAssets
    {
        public static string BgGym = "bg_gym.jpg";
        public static string LogoHeartDumbbell = "logo_heart_dumbbell.jpg";
        public static string LogoHeartbeat = "logo_heartbeat.jpg";
        public static string AppLogo = "glory_gym_logo.png";

        private static Icon _appIcon;

        /// <summary>The application/brand icon (extracted from the published EXE icon).</summary>
        public static Icon AppIcon
        {
            get
            {
                if (_appIcon == null)
                {
                    try { _appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); }
                    catch { /* icon unavailable -> leave null */ }
                }
                return _appIcon;
            }
        }

        /// <summary>Assigns the brand icon to a top-level window (title bar + taskbar).</summary>
        public static void ApplyAppIcon(Form form)
        {
            Icon ic = AppIcon;
            if (form != null && ic != null)
                form.Icon = ic;
        }

        public static Image TryLoad(string fileName)
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "Resources", fileName);
                if (File.Exists(path))
                    return Image.FromFile(path);
            }
            catch { }
            return null;
        }

        public static Image CreateWithOpacity(Image image, float opacity)
        {
            if (image == null) return null;
            try
            {
                Bitmap bmp = new Bitmap(image.Width, image.Height);
                using (Graphics gfx = Graphics.FromImage(bmp))
                {
                    ColorMatrix matrix = new ColorMatrix { Matrix33 = opacity };
                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch
            {
                return image;
            }
        }

        public static Image TryLoadToughBackground(string context, float opacity)
        {
            // Figma design is a clean, flat light surface — no photographic
            // backdrop. Skip the gym background entirely in light mode so every
            // page matches the Figma look.
            if (ThemeManager.IsLight)
                return null;

            // The context parameter was intended for future different backgrounds,
            // For now, always use the main background image
            Image bg = TryLoad(BgGym);
            if (bg != null)
                return CreateWithOpacity(bg, opacity);
            return null;
        }
    }
}
