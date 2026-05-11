using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class DietPlanForm : Form
    {
        // ── Feeding plan model ──
        private class FeedingPlan
        {
            public string Name    { get; set; }
            public string PdfPath { get; set; }
            public override string ToString() => Name;
        }

        // ── Mock member store (shared with MembersForm mock data) ──
        private static readonly List<(string Name, string Phone, string Plan)> MockMembers =
            new List<(string, string, string)>
            {
                ("أحمد محمد",    "0501234567", "Basic Plan"),
                ("سارة علي",     "0559876543", "Pro Plan"),
                ("خالد إبراهيم", "0561112233", "Annual Plan"),
                ("نورة حسن",     "0547778899", "Basic Plan"),
                ("عمر فاروق",    "0533334455", "Pro Plan"),
                ("ليلى أحمد",    "0522225566", "Basic Plan"),
                ("يوسف كمال",    "0511116677", "Annual Plan"),
                ("فاطمة سعيد",   "0588889900", "Basic Plan"),
                ("محمود عادل",   "0577771122", "Pro Plan"),
                ("هند محمود",    "0566662233", "Annual Plan"),
            };

        // ── Feeding plans catalog ──
        private readonly List<FeedingPlan> _feedingPlans = new List<FeedingPlan>
        {
            new FeedingPlan { Name = "خطة التنشيف",       PdfPath = @"C:\Plans\cutting_plan.pdf" },
            new FeedingPlan { Name = "خطة التضخم",        PdfPath = @"C:\Plans\bulking_plan.pdf" },
            new FeedingPlan { Name = "خطة الحفاظ",        PdfPath = @"C:\Plans\maintenance_plan.pdf" },
            new FeedingPlan { Name = "خطة نباتية",        PdfPath = @"C:\Plans\vegan_plan.pdf" },
            new FeedingPlan { Name = "خطة كيتو",          PdfPath = @"C:\Plans\keto_plan.pdf" },
        };

        // ── Currently found member ──
        private string _currentMemberName  = "";
        private string _currentMemberPhone = "";

        public DietPlanForm()
        {
            InitializeComponent();
            RefreshPlanCombo();
            WireEvents();
        }

        // ═══════════════════════════════════════════
        //  INIT
        // ═══════════════════════════════════════════
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
            btnSearchPhone.Click              += BtnSearchPhone_Click;
            txtSearchPhone.KeyDown            += (s, e) => { if (e.KeyCode == Keys.Enter) BtnSearchPhone_Click(s, e); };
            btnBrowsePlanPdf.Click            += BtnBrowsePlanPdf_Click;
            btnSavePlan.Click                 += BtnSavePlan_Click;
            cmbSelectPlan.SelectedIndexChanged += CmbSelectPlan_Changed;
            btnSendPlan.Click                 += BtnSendPlan_Click;
        }

        // ═══════════════════════════════════════════
        //  SEARCH BY PHONE
        // ═══════════════════════════════════════════
        private void BtnSearchPhone_Click(object sender, EventArgs e)
        {
            string phone = txtSearchPhone.Text.Trim();
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("الرجاء إدخال رقم الهاتف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var found = MockMembers.FirstOrDefault(m => m.Phone.Contains(phone));

            if (found == default)
            {
                lblFoundName.Text  = "❌  لم يُعثر على عضو بهذا الرقم";
                lblFoundName.ForeColor = Color.FromArgb(220, 53, 69);
                lblFoundPhone.Text = "";
                lblFoundPlan.Text  = "";
                _currentMemberName  = "";
                _currentMemberPhone = "";
            }
            else
            {
                _currentMemberName  = found.Name;
                _currentMemberPhone = found.Phone;

                lblFoundName.Text  = "👤  " + found.Name;
                lblFoundName.ForeColor = Color.White;
                lblFoundPhone.Text = "📞  " + found.Phone;
                lblFoundPlan.Text  = "📋  " + found.Plan;
            }
        }

        // ═══════════════════════════════════════════
        //  CREATE FEEDING PLAN
        // ═══════════════════════════════════════════
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
            if (string.IsNullOrEmpty(pdf))
            {
                MessageBox.Show("الرجاء اختيار ملف PDF للخطة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for duplicate name
            if (_feedingPlans.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("توجد خطة بهذا الاسم بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newPlan = new FeedingPlan { Name = name, PdfPath = pdf };
            _feedingPlans.Add(newPlan);
            RefreshPlanCombo();

            // Auto-select the new plan
            cmbSelectPlan.SelectedItem = newPlan;

            txtPlanName.Text = "";
            txtPlanPdf.Text  = "";

            MessageBox.Show("تم حفظ الخطة: " + name, "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ═══════════════════════════════════════════
        //  SELECT PLAN → AUTO-FILL PDF PATH
        // ═══════════════════════════════════════════
        private void CmbSelectPlan_Changed(object sender, EventArgs e)
        {
            if (cmbSelectPlan.SelectedItem is FeedingPlan plan)
                txtSelectedPlanPdf.Text = plan.PdfPath;
            else
                txtSelectedPlanPdf.Text = "";
        }

        // ═══════════════════════════════════════════
        //  SEND PLAN
        // ═══════════════════════════════════════════
        private void BtnSendPlan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentMemberName))
            {
                MessageBox.Show("الرجاء البحث عن عضو أولاً برقم الهاتف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cmbSelectPlan.SelectedItem is FeedingPlan plan))
            {
                MessageBox.Show("الرجاء اختيار خطة تغذية", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string logEntry  = $"[{timestamp}]  {_currentMemberName} ({_currentMemberPhone})  ←  {plan.Name}";

            listHistory.Items.Insert(0, logEntry);

            string msg =
                $"مرحباً {_currentMemberName}،\n\nنرفق لك خطة التغذية: {plan.Name}\nملف PDF: {plan.PdfPath}\n\nمع تحيات فريق Glory Gym";
            WhatsAppWeb.OpenChat(_currentMemberPhone, msg);

            MessageBox.Show(
                "تم فتح واتساب في المتصفح لإرسال الرسالة. أكمل الإرسال من هناك إن لزم.",
                "واتساب", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
