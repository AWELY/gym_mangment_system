using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using gym_mangment_system.Properties;

namespace gym_mangment_system
{
    /// <summary>
    /// Settings page ("الإعدادات"): edit/test the MSSQL connection string and
    /// take database backups into a chosen folder.
    /// </summary>
    public partial class SettingsForm : Form, IThemeAware
    {
        private Label _lblTitle;
        private Panel _body;

        private Guna2Panel _connCard;
        private Label _lblConnTitle, _lblConnHint;
        private Guna2TextBox _txtConn;
        private Guna2Button _btnTest, _btnSaveConn;

        private Guna2Panel _backupCard;
        private Label _lblBackupTitle, _lblBackupHint;
        private Guna2TextBox _txtFolder;
        private Guna2Button _btnBrowse, _btnBackupNow;

        public SettingsForm()
        {
            InitializeComponent();
            RightToLeft = RightToLeft.Yes;
            BuildLayout();
            ApplyTheme(ThemeManager.Current);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 660);
            Name = "SettingsForm";
            Text = "الإعدادات";
            ResumeLayout(false);
        }

        private void BuildLayout()
        {
            _lblTitle = new Label
            {
                Dock      = DockStyle.Top,
                Height    = 56,
                Font      = new Font("Segoe UI", 20F, FontStyle.Bold),
                Padding   = new Padding(20, 10, 20, 0),
                TextAlign = ContentAlignment.MiddleRight,
                Text      = "⚙️  الإعدادات"
            };

            _body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 12, 24, 24), AutoScroll = true };

            // ── connection card ──
            _connCard = GunaUi.Card(620, 240, ThemeManager.Current.Card, ThemeManager.Current.BorderSubtle);
            _connCard.Location = new Point(24, 16);

            _lblConnTitle = new Label
            {
                Text = "🗄️  اتصال قاعدة البيانات (MSSQL Server)",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Location = new Point(18, 16), Size = new Size(584, 28),
                TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent
            };
            _lblConnHint = new Label
            {
                Text = "سلسلة الاتصال. اتركها فارغة لاستخدام الإعداد الافتراضي من ملف App.config.",
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(18, 48), Size = new Size(584, 22),
                TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent
            };
            _txtConn = new Guna2TextBox
            {
                Location = new Point(18, 78), Size = new Size(584, 80),
                Multiline = true, BorderRadius = 8, Font = new Font("Segoe UI", 10F),
                Text = CurrentConnectionDisplay()
            };
            _btnTest = GunaUi.Button("اختبار الاتصال", FigmaPalette.BlueBtn, new Point(322, 176), new Size(140, 40));
            _btnSaveConn = GunaUi.Button("حفظ", FigmaPalette.GreenBtn, new Point(176, 176), new Size(140, 40));
            _btnTest.Click     += BtnTest_Click;
            _btnSaveConn.Click += BtnSaveConn_Click;

            _connCard.Controls.Add(_lblConnTitle);
            _connCard.Controls.Add(_lblConnHint);
            _connCard.Controls.Add(_txtConn);
            _connCard.Controls.Add(_btnTest);
            _connCard.Controls.Add(_btnSaveConn);

            // ── backup card ──
            _backupCard = GunaUi.Card(620, 200, ThemeManager.Current.Card, ThemeManager.Current.BorderSubtle);
            _backupCard.Location = new Point(24, 272);

            _lblBackupTitle = new Label
            {
                Text = "💾  النسخ الاحتياطي لقاعدة البيانات",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Location = new Point(18, 16), Size = new Size(584, 28),
                TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent
            };
            _lblBackupHint = new Label
            {
                Text = "اختر المجلد الذي تريد حفظ النسخة الاحتياطية فيه. يجب أن يملك خادم SQL صلاحية الكتابة عليه.",
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(18, 48), Size = new Size(584, 22),
                TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent
            };
            _txtFolder = new Guna2TextBox
            {
                Location = new Point(18, 78), Size = new Size(584, 40),
                BorderRadius = 8, ReadOnly = true, Font = new Font("Segoe UI", 10F),
                PlaceholderText = "لم يتم تحديد مجلد بعد",
                Text = Settings.Default.BackupFolder ?? string.Empty
            };
            _btnBrowse = GunaUi.Button("استعراض...", FigmaPalette.BlueBtn, new Point(322, 130), new Size(140, 40));
            _btnBackupNow = GunaUi.Button("نسخ احتياطي الآن", FigmaPalette.GreenBtn, new Point(140, 130), new Size(176, 40));
            _btnBrowse.Click    += BtnBrowse_Click;
            _btnBackupNow.Click += (s, e) => BackupInteractive(this);

            _backupCard.Controls.Add(_lblBackupTitle);
            _backupCard.Controls.Add(_lblBackupHint);
            _backupCard.Controls.Add(_txtFolder);
            _backupCard.Controls.Add(_btnBrowse);
            _backupCard.Controls.Add(_btnBackupNow);

            _body.Controls.Add(_connCard);
            _body.Controls.Add(_backupCard);

            Controls.Add(_body);
            Controls.Add(_lblTitle);
        }

        private static string CurrentConnectionDisplay()
        {
            // Show the saved override if any; otherwise leave blank (config default in use).
            string ovr = Settings.Default.SqlConnectionString;
            return string.IsNullOrWhiteSpace(ovr) ? string.Empty : ovr;
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            string cs = string.IsNullOrWhiteSpace(_txtConn.Text) ? Db.ConnectionString : _txtConn.Text.Trim();
            try
            {
                using (var conn = new SqlConnection(cs))
                    conn.Open();
                GunaUi.Show("تم الاتصال بقاعدة البيانات بنجاح ✅", "اختبار الاتصال",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GunaUi.Show("فشل الاتصال:\n\n" + ex.Message, "اختبار الاتصال",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveConn_Click(object sender, EventArgs e)
        {
            Settings.Default.SqlConnectionString = _txtConn.Text?.Trim() ?? string.Empty;
            Settings.Default.Save();
            GunaUi.Show(
                "تم حفظ إعدادات الاتصال.\nسيتم استخدامها عند إعادة تشغيل النظام.",
                "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog { Description = "اختر مجلد النسخ الاحتياطي" })
            {
                if (!string.IsNullOrWhiteSpace(_txtFolder.Text))
                    dlg.SelectedPath = _txtFolder.Text;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _txtFolder.Text = dlg.SelectedPath;
                    Settings.Default.BackupFolder = dlg.SelectedPath;
                    Settings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Performs an interactive database backup: resolves the configured folder
        /// (prompting if none is set), runs the backup and reports the result.
        /// Reused by the Settings button and the on-exit prompt. Returns true on success.
        /// </summary>
        public static bool BackupInteractive(IWin32Window owner)
        {
            string folder = Settings.Default.BackupFolder;
            if (string.IsNullOrWhiteSpace(folder))
            {
                using (var dlg = new FolderBrowserDialog { Description = "اختر مجلد النسخ الاحتياطي" })
                {
                    if (dlg.ShowDialog(owner) != DialogResult.OK)
                        return false;
                    folder = dlg.SelectedPath;
                    Settings.Default.BackupFolder = folder;
                    Settings.Default.Save();
                }
            }

            try
            {
                string file = Db.BackupTo(folder);
                GunaUi.Show(owner,
                    "تم إنشاء النسخة الاحتياطية بنجاح ✅\n\n" + file,
                    "نسخ احتياطي", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                // The most common cause is that the SQL Server *service account*
                // (not the Windows user) cannot write to the chosen folder, e.g. the
                // Desktop -> "Operating system error 5 (Access is denied.)". Offer to
                // save into SQL Server's own default backup directory instead, which
                // the service account can always write to.
                string defaultDir = null;
                try { defaultDir = Db.GetDefaultBackupDirectory(); } catch { /* ignore */ }

                if (!string.IsNullOrWhiteSpace(defaultDir) &&
                    !string.Equals(defaultDir.TrimEnd('\\'), (folder ?? "").TrimEnd('\\'), StringComparison.OrdinalIgnoreCase))
                {
                    var retry = GunaUi.Show(owner,
                        "تعذر إنشاء النسخة الاحتياطية في المجلد المحدد، غالباً لأن حساب خدمة " +
                        "SQL Server لا يملك صلاحية الكتابة عليه (مثل سطح المكتب).\n\n" +
                        "هل تريد حفظ النسخة في مجلد SQL Server الافتراضي؟\n\n" + defaultDir,
                        "نسخ احتياطي", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (retry == DialogResult.Yes)
                    {
                        try
                        {
                            string file2 = Db.BackupTo(defaultDir);
                            Settings.Default.BackupFolder = defaultDir;
                            Settings.Default.Save();
                            GunaUi.Show(owner,
                                "تم إنشاء النسخة الاحتياطية بنجاح ✅\n\n" + file2,
                                "نسخ احتياطي", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        catch (Exception ex2)
                        {
                            ex = ex2;
                        }
                    }
                }

                GunaUi.Show(owner,
                    "تعذر إنشاء النسخة الاحتياطية:\n\n" + ex.Message +
                    "\n\nملاحظة: يجب أن يملك حساب خدمة SQL Server صلاحية الكتابة على المجلد المحدد." +
                    "\nاختر مجلداً مثل C:\\GymBackups وامنح حساب الخدمة صلاحية الكتابة عليه، " +
                    "أو استخدم مجلد SQL Server الافتراضي.",
                    "نسخ احتياطي", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            _body.BackColor = s.ContentHost;
            _lblTitle.ForeColor = s.TextPrimary;

            StyleCard(_connCard, s);
            _lblConnTitle.ForeColor = s.TextPrimary;
            _lblConnHint.ForeColor = s.TextMuted;
            StyleTextBox(_txtConn, s);

            StyleCard(_backupCard, s);
            _lblBackupTitle.ForeColor = s.TextPrimary;
            _lblBackupHint.ForeColor = s.TextMuted;
            StyleTextBox(_txtFolder, s);
        }

        private static void StyleCard(Guna2Panel card, UiColorScheme s)
        {
            card.FillColor = s.Card;
            card.BorderColor = s.BorderSubtle;
        }

        private static void StyleTextBox(Guna2TextBox tb, UiColorScheme s)
        {
            tb.FillColor = s.InputBackground;
            tb.ForeColor = s.InputForeground;
            tb.BorderColor = s.BorderSubtle;
            tb.FocusedState.BorderColor = FigmaPalette.Primary;
            tb.HoverState.BorderColor = FigmaPalette.Primary;
            tb.PlaceholderForeColor = s.TextMuted;
        }
    }
}
