using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace gym_mangment_system
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
            AssignNavEvents();
        }

        private void AssignNavEvents()
        {
            this.btnNavDiet.Click += (s, e) => { new DietPlanForm().ShowDialog(this); };
            this.btnNavSubs.Click += (s, e) => { new SubscriptionsForm().ShowDialog(this); };
            this.btnNavStore.Click += (s, e) => { new StoreForm().ShowDialog(this); };
            this.btnNavReports.Click += (s, e) => { new ReportsForm().ShowDialog(this); };
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnNutrition_Click(object sender, EventArgs e)
        {
            DietPlanForm dForm = new DietPlanForm();
            dForm.Show(this);
        }

        private void ChartPanel_Paint(object sender, PaintEventArgs e)
        {
            // Draw a mock chart (a red line with filled gradient)
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen gridPen = new Pen(Color.FromArgb(50, 50, 50));
            // draw horizontal grid lines
            for (int i = 50; i < 300; i += 50)
            {
                g.DrawLine(gridPen, 50, i, 580, i);
            }

            // Draw line graph
            Point[] points = {
                new Point(50, 250),
                new Point(100, 200),
                new Point(150, 210),
                new Point(200, 150),
                new Point(250, 140),
                new Point(300, 180),
                new Point(350, 120),
                new Point(400, 100),
                new Point(450, 130),
                new Point(500, 80),
                new Point(550, 60)
            };

            Pen linePen = new Pen(Color.FromArgb(220, 53, 69), 3);
            g.DrawLines(linePen, points);

            // Fill area under curve
            GraphicsPath path = new GraphicsPath();
            path.AddLines(points);
            path.AddLine(550, 60, 550, 300);
            path.AddLine(550, 300, 50, 300);
            path.CloseFigure();

            using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, 600, 300), Color.FromArgb(100, 220, 53, 69), Color.Transparent, LinearGradientMode.Vertical))
            {
                g.FillPath(brush, path);
            }

            // Draw points
            foreach (Point p in points)
            {
                g.FillEllipse(Brushes.White, p.X - 4, p.Y - 4, 8, 8);
            }
        }
    }
}
