namespace gym_mangment_system
{
    partial class DietPlanForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblIngTitle;
        private System.Windows.Forms.ListBox listIng;
        private System.Windows.Forms.Label lblFood;
        private System.Windows.Forms.TextBox txtFood;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Button btnWhatsApp;
        private System.Windows.Forms.Label lblHistory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblIngTitle = new System.Windows.Forms.Label();
            this.listIng = new System.Windows.Forms.ListBox();
            this.lblFood = new System.Windows.Forms.Label();
            this.txtFood = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.btnWhatsApp = new System.Windows.Forms.Button();
            this.lblHistory = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblIngTitle
            this.lblIngTitle.Text = "Ingredients / Food";
            this.lblIngTitle.Location = new System.Drawing.Point(20, 20);
            this.lblIngTitle.AutoSize = true;
            
            // listIng
            this.listIng.Location = new System.Drawing.Point(20, 50);
            this.listIng.Size = new System.Drawing.Size(180, 300);
            this.listIng.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.listIng.ForeColor = System.Drawing.Color.White;
            this.listIng.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listIng.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listIng.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listIng.Items.AddRange(new object[] { "شوفان", "حليب خالي الدسم", "بروتين مصل اللبن", "تفاح", "موز", "لوز", "زبدة فول سوداني", "دجاج مشوي", "تونا", "أرز أسمر", "بطاطا حلوة", "بروكلي", "زبادي يوناني" });

            // lblFood
            this.lblFood.Text = "Food";
            this.lblFood.Location = new System.Drawing.Point(220, 20);
            this.lblFood.AutoSize = true;

            // txtFood
            this.txtFood.Location = new System.Drawing.Point(220, 50);
            this.txtFood.Size = new System.Drawing.Size(250, 25);
            this.txtFood.Multiline = true;
            this.txtFood.Height = 60;
            this.txtFood.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.txtFood.ForeColor = System.Drawing.Color.White;
            this.txtFood.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // lblDesc
            this.lblDesc.Text = "Comments";
            this.lblDesc.Location = new System.Drawing.Point(220, 120);
            this.lblDesc.AutoSize = true;

            // txtComments
            this.txtComments.Location = new System.Drawing.Point(220, 150);
            this.txtComments.Size = new System.Drawing.Size(250, 80);
            this.txtComments.Multiline = true;
            this.txtComments.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.txtComments.ForeColor = System.Drawing.Color.White;
            this.txtComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // btnWhatsApp
            this.btnWhatsApp.Text = "إرسال عبر WhatsApp";
            this.btnWhatsApp.BackColor = System.Drawing.Color.FromArgb(18, 140, 126); 
            this.btnWhatsApp.ForeColor = System.Drawing.Color.White;
            this.btnWhatsApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWhatsApp.FlatAppearance.BorderSize = 0;
            this.btnWhatsApp.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnWhatsApp.Size = new System.Drawing.Size(250, 45);
            this.btnWhatsApp.Location = new System.Drawing.Point(220, 250);
            this.btnWhatsApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWhatsApp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnWhatsApp.Click += new System.EventHandler(this.BtnWhatsApp_Click);

            // lblHistory
            this.lblHistory.Text = "إرسال عبر المدير\nالاشتراكات: بروتين مصل اللبن\nإرسال عبر WhatsApp";
            this.lblHistory.Location = new System.Drawing.Point(220, 310);
            this.lblHistory.Size = new System.Drawing.Size(250, 60);
            this.lblHistory.ForeColor = System.Drawing.Color.LightGray;
            this.lblHistory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

            // DietPlanForm
            this.Text = "diet plan creator";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.ForeColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblIngTitle);
            this.Controls.Add(this.listIng);
            this.Controls.Add(this.lblFood);
            this.Controls.Add(this.txtFood);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.btnWhatsApp);
            this.Controls.Add(this.lblHistory);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
