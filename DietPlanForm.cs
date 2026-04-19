using System;
using System.Drawing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class DietPlanForm : Form
    {
        public DietPlanForm()
        {
            InitializeComponent();
        }

        private void BtnWhatsApp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("تم إرسال الخطة بنجاح عبر WhatsApp!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
