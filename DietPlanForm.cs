using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace gym_mangment_system
{
    public partial class DietPlanForm : Form, IThemeAware
    {
        private sealed class FeedingPlan
        {
            public string Name    { get; set; }
            public string PdfPath { get; set; }
            public override string ToString() => Name;
        }

        private readonly List<FeedingPlan> _feedingPlans = new List<FeedingPlan>();

        private string _currentMemberName  = "";
        private string _currentMemberPhone = "";

        // Figma card layout
        private FlowLayoutPanel _cardHost;
        private Panel _root;
        private Guna2GradientButton _btnAddPlan;
        private Guna2Button _btnCancelPlan;
        private string _editingPlanName = null;

        public DietPlanForm()
        {
            InitializeComponent();
            ReloadFeedingPlansFromStore();
            ReloadHistoryFromStore();
            RefreshPlanCombo();
            SetupPhoneAutoComplete();
            WireEvents();
            BuildDietLayout();
            ApplyTheme(ThemeManager.Current);
            this.Resize += (_, __) => { if (pnlCreatePlan.Visible) RecenterCreatePlan(); };
        }

        private void RecenterCreatePlan()
        {
            int w = Math.Min(pnlCreatePlan.Width, ClientSize.Width);
            int h = Math.Min(pnlCreatePlan.Height, ClientSize.Height);
            pnlCreatePlan.Location = new Point(Math.Max(0, (ClientSize.Width - w) / 2),
                                               Math.Max(0, (ClientSize.Height - h) / 2));
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            ForeColor = s.TextPrimary;
            lblTitle.ForeColor = s.TextPrimary;

            // overlay editor card
            pnlCreatePlan.FillColor = s.Card;
            pnlCreatePlan.BorderColor = s.BorderSubtle;
            lblCreateTitle.ForeColor = s.TextPrimary;
            lblPlanName.ForeColor = s.TextMuted;
            lblPlanPdf.ForeColor = s.TextMuted;
            txtPlanName.BackColor = s.InputBackground;
            txtPlanName.ForeColor = s.InputForeground;
            txtPlanPdf.BackColor = s.InputBackground;
            txtPlanPdf.ForeColor = s.InputForeground;

            if (_root != null) _root.BackColor = s.ContentHost;
            if (_btnCancelPlan != null)
            {
                _btnCancelPlan.FillColor = s.SecondaryButton;
                _btnCancelPlan.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.White;
            }
            if (_cardHost != null)
            {
                _cardHost.BackColor = s.ContentHost;
                BuildPlanCards();
            }
        }

        // ── Figma layout: toolbar + plan cards + send log ──
        private void BuildDietLayout()
        {
            pnlLeft.Visible  = false;
            pnlRight.Visible = false;

            // History is still recorded in the background, but the on-screen
            // "سجل الإرسال" section is removed per request — keep the list hidden.
            listHistory.Visible = false;
            lblSectionHistory.Visible = false;
            pnlRight.Controls.Remove(pnlCreatePlan);

            // overlay editor
            Controls.Add(pnlCreatePlan);
            pnlCreatePlan.Dock        = DockStyle.None;
            pnlCreatePlan.Size        = new Size(400, 250);
            pnlCreatePlan.Visible     = false;
            btnSavePlan.Size     = new Size(178, 38);
            btnSavePlan.Location = new Point(210, 200);
            btnSavePlan.Text     = "💾 حفظ";
            _btnCancelPlan = GunaUi.Button("إلغاء", ThemeManager.Current.SecondaryButton, new Point(16, 200), new Size(178, 38));
            _btnCancelPlan.Click += (_, __) => pnlCreatePlan.Visible = false;
            pnlCreatePlan.Controls.Add(_btnCancelPlan);

            _root = new Panel { Dock = DockStyle.Fill };
            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var toolbar = new Panel { Dock = DockStyle.Fill };
            _btnAddPlan = GunaUi.ToolbarButton("➕ إضافة خطة", FigmaPalette.Primary, new Point(18, 9));
            _btnAddPlan.Click += (_, __) => ShowPlanOverlay(null);
            toolbar.Controls.Add(_btnAddPlan);

            _cardHost = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                AutoScroll    = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents  = true,
                Padding       = new Padding(14)
            };

            tlp.Controls.Add(toolbar, 0, 0);
            tlp.Controls.Add(_cardHost, 0, 1);
            _root.Controls.Add(tlp);

            Controls.Add(_root);
            _root.BringToFront();
        }

        private void BuildPlanCards()
        {
            if (_cardHost == null) return;
            UiColorScheme s = ThemeManager.Current;
            _cardHost.SuspendLayout();
            foreach (Control c in _cardHost.Controls) c.Dispose();
            _cardHost.Controls.Clear();
            foreach (var p in _feedingPlans)
                _cardHost.Controls.Add(BuildPlanCard(p, s));
            _cardHost.ResumeLayout();
        }

        private Guna2Panel BuildPlanCard(FeedingPlan p, UiColorScheme s)
        {
            int w = 330, h = 188, pad = 18, innerW = w - pad * 2;
            Guna2Panel card = GunaUi.Card(w, h, s.Card, s.BorderSubtle);

            Label name = new Label { Text = p.Name, Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = s.TextPrimary, Location = new Point(pad, 16), Size = new Size(innerW, 28), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            string pdfText = string.IsNullOrWhiteSpace(p.PdfPath) ? "—" : "📄 " + System.IO.Path.GetFileName(p.PdfPath);
            Label pdf = new Label { Text = pdfText, Font = new Font("Segoe UI", 9.5F), ForeColor = FigmaPalette.Primary, Location = new Point(pad, 48), Size = new Size(innerW, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };

            Guna2Button send = GunaUi.Button("📱  إرسال", FigmaPalette.GreenBtn, new Point(pad, 84), new Size(innerW, 36));
            int btnW = (innerW - 10) / 2;
            Guna2Button del = GunaUi.Button("🗑  حذف", FigmaPalette.Red, new Point(pad, 130), new Size(btnW, 34));
            Guna2Button edit = GunaUi.Button("✎  تعديل", FigmaPalette.BlueBtn, new Point(pad + btnW + 10, 130), new Size(btnW, 34));

            string nm = p.Name, path = p.PdfPath;
            send.Click += async (_, __) => await SendPlanAsync(new FeedingPlan { Name = nm, PdfPath = path });
            edit.Click += (_, __) => ShowPlanOverlay(nm);
            del.Click  += (_, __) => DeletePlan(nm);

            card.Controls.Add(name); card.Controls.Add(pdf);
            card.Controls.Add(send); card.Controls.Add(del); card.Controls.Add(edit);
            return card;
        }

        private void ShowPlanOverlay(string editName)
        {
            _editingPlanName = editName;
            if (editName == null)
            {
                lblCreateTitle.Text = "➕  إنشاء خطة جديدة";
                txtPlanName.Text = "";
                txtPlanPdf.Text  = "";
            }
            else
            {
                var p = _feedingPlans.FirstOrDefault(x => x.Name == editName);
                lblCreateTitle.Text = "✏️  تعديل الخطة";
                txtPlanName.Text = p?.Name ?? "";
                txtPlanPdf.Text  = p?.PdfPath ?? "";
            }
            RecenterCreatePlan();
            pnlCreatePlan.Visible = true;
            pnlCreatePlan.BringToFront();
            txtPlanName.Focus();
        }

        private void DeletePlan(string name)
        {
            if (MessageBox.Show("حذف خطة التغذية: " + name + "؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            GymDataStore.Data.FeedingPlans.RemoveAll(x => x.Name == name);
            GymDataStore.Save();
            _feedingPlans.RemoveAll(x => x.Name == name);
            RefreshPlanCombo();
            BuildPlanCards();
        }

        private void SetupPhoneAutoComplete()
        {
            var phones = new AutoCompleteStringCollection();
            foreach (var m in GymDataStore.Data.Members)
            {
                if (!string.IsNullOrWhiteSpace(m.Phone))
                    phones.Add(m.Phone.Trim());
            }

            txtSearchPhone.AutoCompleteCustomSource = phones;
            txtSearchPhone.AutoCompleteSource       = AutoCompleteSource.CustomSource;
            txtSearchPhone.AutoCompleteMode         = AutoCompleteMode.SuggestAppend;
        }

        private void ReloadFeedingPlansFromStore()
        {
            _feedingPlans.Clear();
            foreach (var r in GymDataStore.Data.FeedingPlans)
                _feedingPlans.Add(new FeedingPlan { Name = r.Name, PdfPath = r.PdfPath });
        }

        private void ReloadHistoryFromStore()
        {
            listHistory.Items.Clear();
            foreach (var line in Enumerable.Reverse(GymDataStore.Data.DietSendHistory))
                listHistory.Items.Add(line);
        }

        private void RefreshPlanCombo()
        {
            cmbSelectPlan.Items.Clear();
            foreach (var plan in _feedingPlans)
                cmbSelectPlan.Items.Add(plan);

            if (cmbSelectPlan.Items.Count > 0)
                cmbSelectPlan.SelectedIndex = 0;
        }

        private void WireEvents()
        {
            btnBrowsePlanPdf.Click += BtnBrowsePlanPdf_Click;
            btnSavePlan.Click      += BtnSavePlan_Click;
        }

        private void BtnBrowsePlanPdf_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title  = "اختر ملف PDF للخطة";
                dlg.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtPlanPdf.Text = dlg.FileName;
            }
        }

        private void BtnSavePlan_Click(object sender, EventArgs e)
        {
            string name = txtPlanName.Text.Trim();
            string pdf  = txtPlanPdf.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("الرجاء إدخال اسم الخطة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool nameTakenByOther =
                GymDataStore.Data.FeedingPlans.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && p.Name != _editingPlanName);
            if (nameTakenByOther)
            {
                MessageBox.Show("توجد خطة بهذا الاسم بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_editingPlanName == null)
            {
                GymDataStore.Data.FeedingPlans.Add(new FeedingPlanRecord { Name = name, PdfPath = pdf });
                _feedingPlans.Add(new FeedingPlan { Name = name, PdfPath = pdf });
            }
            else
            {
                var rec = GymDataStore.Data.FeedingPlans.FirstOrDefault(p => p.Name == _editingPlanName);
                if (rec != null) { rec.Name = name; rec.PdfPath = pdf; }
                var local = _feedingPlans.FirstOrDefault(p => p.Name == _editingPlanName);
                if (local != null) { local.Name = name; local.PdfPath = pdf; }
            }

            GymDataStore.Save();
            RefreshPlanCombo();
            BuildPlanCards();
            pnlCreatePlan.Visible = false;
        }

        private async Task SendPlanAsync(FeedingPlan plan)
        {
            string phone = Microsoft.VisualBasic.Interaction.InputBox(
                "أدخل رقم هاتف العضو لإرسال خطة: " + plan.Name, "إرسال الخطة", "");
            if (string.IsNullOrWhiteSpace(phone)) return;

            var found = GymDataStore.Data.Members.FirstOrDefault(m =>
                !string.IsNullOrEmpty(m.Phone) && m.Phone.Contains(phone.Trim()));
            if (found == null)
            {
                MessageBox.Show("لم يُعثر على عضو بهذا الرقم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _currentMemberName  = found.FullName;
            _currentMemberPhone = found.Phone;
            await SendCoreAsync(plan);
        }

        private async Task SendCoreAsync(FeedingPlan plan)
        {
            if (string.IsNullOrEmpty(_currentMemberName) || plan == null) return;

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string logEntry  = $"[{timestamp}]  {_currentMemberName} ({_currentMemberPhone})  ←  {plan.Name}";

            GymDataStore.Data.DietSendHistory.Insert(0, logEntry);
            GymDataStore.Save();
            listHistory.Items.Insert(0, logEntry);

            string defaultCc = QonvoWhatsAppClient.ReadSetting("QonvoDefaultCountryCode", "218");
            string recipient = QonvoPhone.ToE164(_currentMemberPhone, defaultCc);
            string senderPhone = QonvoWhatsAppClient.ReadSetting("QonvoSenderWhatsAppPhone", "").Trim();
            if (!string.IsNullOrEmpty(senderPhone) && !senderPhone.StartsWith("+", StringComparison.Ordinal))
                senderPhone = "+" + new string(senderPhone.Where(char.IsDigit).ToArray());

            string msgForMember =
                $"مرحباً {_currentMemberName}،\n\nخطة التغذية: {plan.Name}\n\nمع تحيات فريق Glory Gym";

            string baseUrl = QonvoWhatsAppClient.ReadSetting("QonvoBaseUrl", "https://backup.qonvo.ly/api");
            string token   = QonvoWhatsAppClient.ReadSetting("QonvoApiToken", "");

            bool sentAsDocument = !string.IsNullOrWhiteSpace(plan.PdfPath) && File.Exists(plan.PdfPath.Trim());
            QonvoSendResult result;

            try
            {
                Cursor = Cursors.WaitCursor;

                using (var client = new QonvoWhatsAppClient())
                {
                    if (sentAsDocument)
                    {
                        result = await client.SendDocumentFromFileAsync(
                                baseUrl,
                                token,
                                senderPhone,
                                recipient,
                                plan.PdfPath.Trim(),
                                msgForMember)
                            .ConfigureAwait(true);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(plan.PdfPath))
                        {
                            MessageBox.Show(
                                "ملف PDF غير موجود في المسار المحفوظ. سيتم إرسال رسالة نصية فقط.",
                                "تنبيه",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }

                        result = await client.SendTextAsync(baseUrl, token, senderPhone, recipient, msgForMember)
                            .ConfigureAwait(true);
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            if (result.Ok)
            {
                MessageBox.Show(
                    sentAsDocument
                        ? "تم جدولة إرسال خطة التغذية (ملف PDF) عبر واتساب (Qonvo)."
                        : "تم جدولة إرسال الرسالة عبر واتساب (Qonvo).",
                    "واتساب",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var openBrowser = MessageBox.Show(
                result.Detail + "\n\nهل تريد فتح واتساب في المتصفح كبديل؟",
                "فشل الإرسال عبر الـ API",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (openBrowser == DialogResult.Yes)
            {
                string msgWithPdf =
                    $"مرحباً {_currentMemberName}،\n\nنرفق لك خطة التغذية: {plan.Name}\nملف PDF: {plan.PdfPath}\n\nمع تحيات فريق Glory Gym";
                WhatsAppWeb.OpenChat(_currentMemberPhone, msgWithPdf);
            }
        }

        private void DietPlanForm_Load(object sender, EventArgs e)
        {

        }
    }
}
