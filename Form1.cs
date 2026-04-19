using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace gym_mangment_system
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Mock Login -> Open Dashboard Form
            DashboardForm dashboard = new DashboardForm();
            this.Hide();
            dashboard.ShowDialog();
            this.Close(); // Close the application after the dashboard gets closed
        }
    }
}
