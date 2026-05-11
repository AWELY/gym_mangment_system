using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    /// <summary>
    /// Opens WhatsApp Web / app via https://wa.me with an optional pre-filled message.
    /// </summary>
    public static class WhatsAppWeb
    {
        /// <summary>
        /// Keeps digits only; if the number looks like a Saudi local mobile (05xxxxxxxx), prefixes 966.
        /// </summary>
        public static string NormalizePhoneForWaMe(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            string d = new string(raw.Where(char.IsDigit).ToArray());
            if (d.Length == 0) return "";

            if (d.StartsWith("966", StringComparison.Ordinal)) return d;
            if (d.StartsWith("0", StringComparison.Ordinal) && d.Length >= 10)
                return "966" + d.Substring(1);
            if (d.Length == 9)
                return "966" + d;

            return d;
        }

        public static void OpenChat(string phoneRaw, string message)
        {
            string n = NormalizePhoneForWaMe(phoneRaw);
            if (string.IsNullOrEmpty(n))
            {
                MessageBox.Show("رقم الهاتف غير صالح لإرسال واتساب.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string text = message ?? "";
            string url = "https://wa.me/" + n + "?text=" + Uri.EscapeDataString(text);

            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر فتح المتصفح:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
