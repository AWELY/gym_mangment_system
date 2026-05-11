namespace gym_mangment_system
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tlpRevCards;
        private System.Windows.Forms.Panel pnlRev;
        private System.Windows.Forms.Label lblRevVal;
        private System.Windows.Forms.Label lblRevT;
        private System.Windows.Forms.Panel pnlSubs;
        private System.Windows.Forms.Label lblSubsVal;
        private System.Windows.Forms.Label lblSubsT;
        private System.Windows.Forms.Panel pnlStore;
        private System.Windows.Forms.Label lblStoreVal;
        private System.Windows.Forms.Label lblStoreT;
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
            this.tlpRevCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRev = new System.Windows.Forms.Panel();
            this.lblRevVal = new System.Windows.Forms.Label();
            this.lblRevT = new System.Windows.Forms.Label();
            this.pnlSubs = new System.Windows.Forms.Panel();
            this.lblSubsVal = new System.Windows.Forms.Label();
            this.lblSubsT = new System.Windows.Forms.Label();
            this.pnlStore = new System.Windows.Forms.Panel();
            this.lblStoreVal = new System.Windows.Forms.Label();
            this.lblStoreT = new System.Windows.Forms.Label();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.tlpRevCards.SuspendLayout();
            this.pnlRev.SuspendLayout();
            this.pnlSubs.SuspendLayout();
            this.pnlStore.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(15, 10, 15, 5);
            this.lblTitle.Size = new System.Drawing.Size(910, 55);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📊  المالية";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tlpRevCards
            // 
            this.tlpRevCards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpRevCards.ColumnCount = 3;
            this.tlpRevCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpRevCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpRevCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpRevCards.Controls.Add(this.pnlRev, 2, 0);
            this.tlpRevCards.Controls.Add(this.pnlSubs, 1, 0);
            this.tlpRevCards.Controls.Add(this.pnlStore, 0, 0);
            this.tlpRevCards.Location = new System.Drawing.Point(23, 85);
            this.tlpRevCards.Name = "tlpRevCards";
            this.tlpRevCards.RowCount = 1;
            this.tlpRevCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRevCards.Size = new System.Drawing.Size(904, 120);
            this.tlpRevCards.TabIndex = 1;
            // 
            // pnlRev
            // 
            this.pnlRev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pnlRev.Controls.Add(this.lblRevVal);
            this.pnlRev.Controls.Add(this.lblRevT);
            this.pnlRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRev.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.pnlRev.Name = "pnlRev";
            this.pnlRev.Size = new System.Drawing.Size(283, 114);
            this.pnlRev.TabIndex = 0;
            // 
            // lblRevT
            // 
            this.lblRevT.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRevT.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblRevT.ForeColor = System.Drawing.Color.LightGray;
            this.lblRevT.Location = new System.Drawing.Point(0, 0);
            this.lblRevT.Name = "lblRevT";
            this.lblRevT.Size = new System.Drawing.Size(283, 35);
            this.lblRevT.TabIndex = 0;
            this.lblRevT.Text = "💰 إجمالي الإيرادات";
            this.lblRevT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRevVal
            // 
            this.lblRevVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRevVal.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblRevVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.lblRevVal.Location = new System.Drawing.Point(0, 35);
            this.lblRevVal.Name = "lblRevVal";
            this.lblRevVal.Size = new System.Drawing.Size(283, 79);
            this.lblRevVal.TabIndex = 1;
            this.lblRevVal.Text = "12,450 $";
            this.lblRevVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSubs
            // 
            this.pnlSubs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pnlSubs.Controls.Add(this.lblSubsVal);
            this.pnlSubs.Controls.Add(this.lblSubsT);
            this.pnlSubs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSubs.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.pnlSubs.Name = "pnlSubs";
            this.pnlSubs.Size = new System.Drawing.Size(283, 114);
            this.pnlSubs.TabIndex = 1;
            // 
            // lblSubsT
            // 
            this.lblSubsT.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubsT.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubsT.ForeColor = System.Drawing.Color.LightGray;
            this.lblSubsT.Location = new System.Drawing.Point(0, 0);
            this.lblSubsT.Name = "lblSubsT";
            this.lblSubsT.Size = new System.Drawing.Size(283, 35);
            this.lblSubsT.TabIndex = 0;
            this.lblSubsT.Text = "👥 إيرادات الاشتراكات";
            this.lblSubsT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSubsVal
            // 
            this.lblSubsVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubsVal.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblSubsVal.ForeColor = System.Drawing.Color.White;
            this.lblSubsVal.Location = new System.Drawing.Point(0, 35);
            this.lblSubsVal.Name = "lblSubsVal";
            this.lblSubsVal.Size = new System.Drawing.Size(283, 79);
            this.lblSubsVal.TabIndex = 1;
            this.lblSubsVal.Text = "9,200 $";
            this.lblSubsVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlStore
            // 
            this.pnlStore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pnlStore.Controls.Add(this.lblStoreVal);
            this.pnlStore.Controls.Add(this.lblStoreT);
            this.pnlStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStore.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.pnlStore.Name = "pnlStore";
            this.pnlStore.Size = new System.Drawing.Size(283, 114);
            this.pnlStore.TabIndex = 2;
            // 
            // lblStoreT
            // 
            this.lblStoreT.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStoreT.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblStoreT.ForeColor = System.Drawing.Color.LightGray;
            this.lblStoreT.Location = new System.Drawing.Point(0, 0);
            this.lblStoreT.Name = "lblStoreT";
            this.lblStoreT.Size = new System.Drawing.Size(283, 35);
            this.lblStoreT.TabIndex = 0;
            this.lblStoreT.Text = "🛒 إيرادات المتجر";
            this.lblStoreT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStoreVal
            // 
            this.lblStoreVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStoreVal.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStoreVal.ForeColor = System.Drawing.Color.White;
            this.lblStoreVal.Location = new System.Drawing.Point(0, 35);
            this.lblStoreVal.Name = "lblStoreVal";
            this.lblStoreVal.Size = new System.Drawing.Size(283, 79);
            this.lblStoreVal.TabIndex = 1;
            this.lblStoreVal.Text = "3,250 $";
            this.lblStoreVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlChart
            // 
            this.pnlChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pnlChart.Location = new System.Drawing.Point(23, 225);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(904, 340);
            this.pnlChart.TabIndex = 2;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(100)))), ((int)(((byte)(200)))));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(23, 585);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(904, 50);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "📄  تصدير التقرير (PDF)";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(950, 660);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.pnlChart);
            this.Controls.Add(this.tlpRevCards);
            this.Controls.Add(this.lblTitle);
            this.Name = "ReportsForm";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "المالية";
            this.tlpRevCards.ResumeLayout(false);
            this.pnlRev.ResumeLayout(false);
            this.pnlSubs.ResumeLayout(false);
            this.pnlStore.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
