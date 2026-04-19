namespace gym_mangment_system
{
    partial class SubscriptionsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView gridSubs;
        private System.Windows.Forms.Button btnAddMonth;
        private System.Windows.Forms.Button btnAddYear;
        private System.Windows.Forms.Button btnAddVIP;

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
            this.gridSubs = new System.Windows.Forms.DataGridView();
            this.btnAddMonth = new System.Windows.Forms.Button();
            this.btnAddYear = new System.Windows.Forms.Button();
            this.btnAddVIP = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridSubs)).BeginInit();
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
            this.lblTitle.Size = new System.Drawing.Size(201, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "إدارة الاشتراكات";
            // 
            // gridSubs
            // 
            this.gridSubs.AllowUserToAddRows = false;
            this.gridSubs.AllowUserToDeleteRows = false;
            this.gridSubs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.gridSubs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSubs.Location = new System.Drawing.Point(20, 70);
            this.gridSubs.Name = "gridSubs";
            this.gridSubs.ReadOnly = true;
            this.gridSubs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridSubs.Size = new System.Drawing.Size(540, 300);
            this.gridSubs.TabIndex = 1;
            // 
            // btnAddMonth
            // 
            this.btnAddMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnAddMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMonth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddMonth.ForeColor = System.Drawing.Color.White;
            this.btnAddMonth.Location = new System.Drawing.Point(20, 390);
            this.btnAddMonth.Name = "btnAddMonth";
            this.btnAddMonth.Size = new System.Drawing.Size(150, 40);
            this.btnAddMonth.TabIndex = 2;
            this.btnAddMonth.Text = "اشتراك شهري +";
            this.btnAddMonth.UseVisualStyleBackColor = false;
            // 
            // btnAddYear
            // 
            this.btnAddYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnAddYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddYear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddYear.ForeColor = System.Drawing.Color.White;
            this.btnAddYear.Location = new System.Drawing.Point(215, 390);
            this.btnAddYear.Name = "btnAddYear";
            this.btnAddYear.Size = new System.Drawing.Size(150, 40);
            this.btnAddYear.TabIndex = 3;
            this.btnAddYear.Text = "اشتراك سنوي +";
            this.btnAddYear.UseVisualStyleBackColor = false;
            // 
            // btnAddVIP
            // 
            this.btnAddVIP.BackColor = System.Drawing.Color.Gold;
            this.btnAddVIP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddVIP.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddVIP.ForeColor = System.Drawing.Color.Black;
            this.btnAddVIP.Location = new System.Drawing.Point(410, 390);
            this.btnAddVIP.Name = "btnAddVIP";
            this.btnAddVIP.Size = new System.Drawing.Size(150, 40);
            this.btnAddVIP.TabIndex = 4;
            this.btnAddVIP.Text = "اشتراك VIP +";
            this.btnAddVIP.UseVisualStyleBackColor = false;
            // 
            // SubscriptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.btnAddVIP);
            this.Controls.Add(this.btnAddYear);
            this.Controls.Add(this.btnAddMonth);
            this.Controls.Add(this.gridSubs);
            this.Controls.Add(this.lblTitle);
            this.Name = "SubscriptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "إدارة الاشتراكات";
            ((System.ComponentModel.ISupportInitialize)(this.gridSubs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
