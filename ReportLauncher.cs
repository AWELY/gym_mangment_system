using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace gym_mangment_system
{
    /// <summary>
    /// Opens a Crystal report (.rpt) file with the system's default handler.
    /// The report files are embedded in the assembly, so they are extracted to a
    /// temporary file first. This keeps working when the app is published with
    /// ClickOnce, where the executable runs from a deployment folder that does not
    /// contain the loose report files.
    /// </summary>
    internal static class ReportLauncher
    {
        internal static void OpenEmbeddedReport(string fileName, string friendlyName)
        {
            try
            {
                string path = ResolveReportPath(fileName);
                if (path == null)
                {
                    GunaUi.Show(
                        "لم يتم العثور على ملف التقرير: " + fileName,
                        "خطأ في الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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

        private static string ResolveReportPath(string fileName)
        {
            // 1) Prefer a loose file next to the executable (normal build / xcopy run).
            string local = Path.Combine(Application.StartupPath, fileName);
            if (File.Exists(local))
                return local;

            // 2) Fall back to the copy embedded in the assembly (ClickOnce / published).
            Assembly asm = Assembly.GetExecutingAssembly();
            string resourceName = null;
            foreach (string name in asm.GetManifestResourceNames())
            {
                if (name.Equals(fileName, StringComparison.OrdinalIgnoreCase) ||
                    name.EndsWith("." + fileName, StringComparison.OrdinalIgnoreCase))
                {
                    resourceName = name;
                    break;
                }
            }
            if (resourceName == null)
                return null;

            string target = Path.Combine(Path.GetTempPath(), fileName);
            using (Stream src = asm.GetManifestResourceStream(resourceName))
            {
                if (src == null) return null;
                using (FileStream dst = File.Create(target))
                    src.CopyTo(dst);
            }
            return target;
        }
    }
}
