namespace gym_mangment_system
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlRev;
        private System.Windows.Forms.Label lblRevVal;
        private System.Windows.Forms.Label lblRevT;
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Button btnExport;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlRev = new System.Windows.Forms.Panel();
            this.lblRevVal = new System.Windows.Forms.Label();
            this.lblRevT = new System.Windows.Forms.Label();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlRev.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTitle.Size = new System.Drawing.Size(183, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "التقارير المالية";
            // 
            // pnlRev
            // 
            this.pnlRev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.pnlRev.Controls.Add(this.lblRevVal);
            this.pnlRev.Controls.Add(this.lblRevT);
            this.pnlRev.Location = new System.Drawing.Point(20, 70);
            this.pnlRev.Name = "pnlRev";
            this.pnlRev.Size = new System.Drawing.Size(540, 100);
            this.pnlRev.TabIndex = 1;
            // 
            // lblRevVal
            // 
            this.lblRevVal.AutoSize = true;
            this.lblRevVal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblRevVal.ForeColor = System.Drawing.Color.LightGreen;
            this.lblRevVal.Location = new System.Drawing.Point(200, 40);
            this.lblRevVal.Name = "lblRevVal";
            this.lblRevVal.Size = new System.Drawing.Size(130, 45);
            this.lblRevVal.TabIndex = 1;
            this.lblRevVal.Text = "12,450 $";
            // 
            // lblRevT
            // 
            this.lblRevT.AutoSize = true;
            this.lblRevT.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblRevT.ForeColor = System.Drawing.Color.LightGray;
            this.lblRevT.Location = new System.Drawing.Point(170, 10);
            this.lblRevT.Name = "lblRevT";
            this.lblRevT.Size = new System.Drawing.Size(200, 21);
            this.lblRevT.TabIndex = 0;
            this.lblRevT.Text = "إجمالي الإيرادات (الاشتراكات + متجر)";
            // 
            // pnlChart
            // 
            this.pnlChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlChart.Location = new System.Drawing.Point(20, 190);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(540, 200);
            this.pnlChart.TabIndex = 2;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(100)))), ((int)(((byte)(200)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(20, 410);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(540, 40);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "تصدير التقرير (PDF)";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(584, 471);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.pnlChart);
            this.Controls.Add(this.pnlRev);
            this.Controls.Add(this.lblTitle);
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "التقارير المالية";
            this.pnlRev.ResumeLayout(false);
            this.pnlRev.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
