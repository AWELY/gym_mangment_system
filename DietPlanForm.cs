using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class DietPlanForm : Form
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

        public DietPlanForm()
        {
            InitializeComponent();
            ReloadFeedingPlansFromStore();
            ReloadHistoryFromStore();
            RefreshPlanCombo();
            WireEvents();
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
            btnSearchPhone.Click              += BtnSearchPhone_Click;
            txtSearchPhone.KeyDown            += (s, e) => { if (e.KeyCode == Keys.Enter) BtnSearchPhone_Click(s, e); };
            btnBrowsePlanPdf.Click            += BtnBrowsePlanPdf_Click;
            btnSavePlan.Click                 += BtnSavePlan_Click;
            cmbSelectPlan.SelectedIndexChanged += CmbSelectPlan_Changed;
            btnSendPlan.Click                 += BtnSendPlan_Click;
        }

        private void BtnSearchPhone_Click(object sender, EventArgs e)
        {
            string phone = txtSearchPhone.Text.Trim();
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("الرجاء إدخال رقم الهاتف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var found = GymDataStore.Data.Members.FirstOrDefault(m =>
                !string.IsNullOrEmpty(m.Phone) && m.Phone.Contains(phone));

            if (found == null)
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
                _currentMemberName  = found.FullName;
                _currentMemberPhone = found.Phone;

                lblFoundName.Text  = "👤  " + found.FullName;
                lblFoundName.ForeColor = Color.White;
                lblFoundPhone.Text = "📞  " + found.Phone;
                lblFoundPlan.Text  = "📋  " + found.PlanName;
            }
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
            if (string.IsNullOrEmpty(pdf))
            {
                MessageBox.Show("الرجاء اختيار ملف PDF للخطة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_feedingPlans.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ||
                GymDataStore.Data.FeedingPlans.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("توجد خطة بهذا الاسم بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GymDataStore.Data.FeedingPlans.Add(new FeedingPlanRecord { Name = name, PdfPath = pdf });
            GymDataStore.Save();

            var newPlan = new FeedingPlan { Name = name, PdfPath = pdf };
            _feedingPlans.Add(newPlan);
            RefreshPlanCombo();
            cmbSelectPlan.SelectedItem = newPlan;

            txtPlanName.Text = "";
            txtPlanPdf.Text  = "";

            MessageBox.Show("تم حفظ الخطة: " + name, "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CmbSelectPlan_Changed(object sender, EventArgs e)
        {
            if (cmbSelectPlan.SelectedItem is FeedingPlan plan)
                txtSelectedPlanPdf.Text = plan.PdfPath;
            else
                txtSelectedPlanPdf.Text = "";
        }

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

            GymDataStore.Data.DietSendHistory.Insert(0, logEntry);
            GymDataStore.Save();
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
