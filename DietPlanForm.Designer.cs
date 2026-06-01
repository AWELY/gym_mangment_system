namespace gym_mangment_system
{
    partial class DietPlanForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Top title ──
        private System.Windows.Forms.Label lblTitle;

        // ── Left panel: member search ──
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblSearchPhone;
        private System.Windows.Forms.TextBox txtSearchPhone;
        private System.Windows.Forms.Button btnSearchPhone;
        private System.Windows.Forms.Panel pnlMemberInfo;
        private System.Windows.Forms.Label lblFoundName;
        private System.Windows.Forms.Label lblFoundPhone;
        private System.Windows.Forms.Label lblFoundPlan;
        private System.Windows.Forms.Label lblSectionHistory;
        private System.Windows.Forms.ListBox listHistory;

        // ── Right panel: feeding plans ──
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblRightTitle;

        // Create new plan group
        private Guna.UI2.WinForms.Guna2Panel pnlCreatePlan;
        private System.Windows.Forms.Label lblCreateTitle;
        private System.Windows.Forms.Label lblPlanName;
        private System.Windows.Forms.TextBox txtPlanName;
        private System.Windows.Forms.Label lblPlanPdf;
        private System.Windows.Forms.TextBox txtPlanPdf;
        private Guna.UI2.WinForms.Guna2Button btnBrowsePlanPdf;
        private Guna.UI2.WinForms.Guna2Button btnSavePlan;

        // Select & send plan group
        private System.Windows.Forms.Label lblSelectPlan;
        private System.Windows.Forms.ComboBox cmbSelectPlan;
        private System.Windows.Forms.TextBox txtSelectedPlanPdf;
        private System.Windows.Forms.Button btnSendPlan;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.listHistory = new System.Windows.Forms.ListBox();
            this.lblSectionHistory = new System.Windows.Forms.Label();
            this.pnlMemberInfo = new System.Windows.Forms.Panel();
            this.lblFoundPlan = new System.Windows.Forms.Label();
            this.lblFoundPhone = new System.Windows.Forms.Label();
            this.lblFoundName = new System.Windows.Forms.Label();
            this.btnSearchPhone = new System.Windows.Forms.Button();
            this.txtSearchPhone = new System.Windows.Forms.TextBox();
            this.lblSearchPhone = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.btnSendPlan = new System.Windows.Forms.Button();
            this.txtSelectedPlanPdf = new System.Windows.Forms.TextBox();
            this.cmbSelectPlan = new System.Windows.Forms.ComboBox();
            this.lblSelectPlan = new System.Windows.Forms.Label();
            this.pnlCreatePlan = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSavePlan = new Guna.UI2.WinForms.Guna2Button();
            this.btnBrowsePlanPdf = new Guna.UI2.WinForms.Guna2Button();
            this.txtPlanPdf = new System.Windows.Forms.TextBox();
            this.lblPlanPdf = new System.Windows.Forms.Label();
            this.txtPlanName = new System.Windows.Forms.TextBox();
            this.lblPlanName = new System.Windows.Forms.Label();
            this.lblCreateTitle = new System.Windows.Forms.Label();
            this.lblRightTitle = new System.Windows.Forms.Label();
            this.pnlLeft.SuspendLayout();
            this.pnlMemberInfo.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlCreatePlan.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(15, 10, 15, 5);
            this.lblTitle.Size = new System.Drawing.Size(950, 55);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🍏  إدارة خطط التغذية";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(32)))));
            this.pnlLeft.Controls.Add(this.listHistory);
            this.pnlLeft.Controls.Add(this.lblSectionHistory);
            this.pnlLeft.Controls.Add(this.pnlMemberInfo);
            this.pnlLeft.Controls.Add(this.btnSearchPhone);
            this.pnlLeft.Controls.Add(this.txtSearchPhone);
            this.pnlLeft.Controls.Add(this.lblSearchPhone);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 55);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.pnlLeft.Size = new System.Drawing.Size(530, 605);
            this.pnlLeft.TabIndex = 1;
            // 
            // listHistory
            // 
            this.listHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.listHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listHistory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listHistory.ForeColor = System.Drawing.Color.White;
            this.listHistory.ItemHeight = 17;
            this.listHistory.Location = new System.Drawing.Point(20, 234);
            this.listHistory.Name = "listHistory";
            this.listHistory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listHistory.Size = new System.Drawing.Size(458, 340);
            this.listHistory.TabIndex = 5;
            // 
            // lblSectionHistory
            // 
            this.lblSectionHistory.AutoSize = true;
            this.lblSectionHistory.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSectionHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(190)))));
            this.lblSectionHistory.Location = new System.Drawing.Point(20, 208);
            this.lblSectionHistory.Name = "lblSectionHistory";
            this.lblSectionHistory.Size = new System.Drawing.Size(124, 20);
            this.lblSectionHistory.TabIndex = 4;
            this.lblSectionHistory.Text = "📋  سجل الإرسال:";
            // 
            // pnlMemberInfo
            // 
            this.pnlMemberInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(44)))));
            this.pnlMemberInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMemberInfo.Controls.Add(this.lblFoundPlan);
            this.pnlMemberInfo.Controls.Add(this.lblFoundPhone);
            this.pnlMemberInfo.Controls.Add(this.lblFoundName);
            this.pnlMemberInfo.Location = new System.Drawing.Point(20, 90);
            this.pnlMemberInfo.Name = "pnlMemberInfo";
            this.pnlMemberInfo.Padding = new System.Windows.Forms.Padding(12);
            this.pnlMemberInfo.Size = new System.Drawing.Size(458, 100);
            this.pnlMemberInfo.TabIndex = 3;
            // 
            // lblFoundPlan
            // 
            this.lblFoundPlan.AutoSize = true;
            this.lblFoundPlan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFoundPlan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.lblFoundPlan.Location = new System.Drawing.Point(12, 64);
            this.lblFoundPlan.Name = "lblFoundPlan";
            this.lblFoundPlan.Size = new System.Drawing.Size(0, 19);
            this.lblFoundPlan.TabIndex = 2;
            // 
            // lblFoundPhone
            // 
            this.lblFoundPhone.AutoSize = true;
            this.lblFoundPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFoundPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(165)))));
            this.lblFoundPhone.Location = new System.Drawing.Point(12, 42);
            this.lblFoundPhone.Name = "lblFoundPhone";
            this.lblFoundPhone.Size = new System.Drawing.Size(0, 19);
            this.lblFoundPhone.TabIndex = 1;
            // 
            // lblFoundName
            // 
            this.lblFoundName.AutoSize = true;
            this.lblFoundName.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFoundName.ForeColor = System.Drawing.Color.White;
            this.lblFoundName.Location = new System.Drawing.Point(12, 12);
            this.lblFoundName.Name = "lblFoundName";
            this.lblFoundName.Size = new System.Drawing.Size(166, 25);
            this.lblFoundName.TabIndex = 0;
            this.lblFoundName.Text = "—  لم يتم البحث بعد";
            // 
            // btnSearchPhone
            // 
            this.btnSearchPhone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnSearchPhone.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearchPhone.FlatAppearance.BorderSize = 0;
            this.btnSearchPhone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchPhone.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSearchPhone.ForeColor = System.Drawing.Color.White;
            this.btnSearchPhone.Location = new System.Drawing.Point(368, 42);
            this.btnSearchPhone.Name = "btnSearchPhone";
            this.btnSearchPhone.Size = new System.Drawing.Size(110, 32);
            this.btnSearchPhone.TabIndex = 2;
            this.btnSearchPhone.Text = "بحث";
            this.btnSearchPhone.UseVisualStyleBackColor = false;
            // 
            // txtSearchPhone
            // 
            this.txtSearchPhone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(48)))));
            this.txtSearchPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchPhone.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtSearchPhone.ForeColor = System.Drawing.Color.White;
            this.txtSearchPhone.Location = new System.Drawing.Point(20, 42);
            this.txtSearchPhone.Name = "txtSearchPhone";
            this.txtSearchPhone.Size = new System.Drawing.Size(340, 31);
            this.txtSearchPhone.TabIndex = 1;
            // 
            // lblSearchPhone
            // 
            this.lblSearchPhone.AutoSize = true;
            this.lblSearchPhone.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSearchPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(190)))));
            this.lblSearchPhone.Location = new System.Drawing.Point(20, 15);
            this.lblSearchPhone.Name = "lblSearchPhone";
            this.lblSearchPhone.Size = new System.Drawing.Size(156, 20);
            this.lblSearchPhone.TabIndex = 0;
            this.lblSearchPhone.Text = "🔍  البحث برقم الهاتف:";
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlRight.Controls.Add(this.btnSendPlan);
            this.pnlRight.Controls.Add(this.txtSelectedPlanPdf);
            this.pnlRight.Controls.Add(this.cmbSelectPlan);
            this.pnlRight.Controls.Add(this.lblSelectPlan);
            this.pnlRight.Controls.Add(this.pnlCreatePlan);
            this.pnlRight.Controls.Add(this.lblRightTitle);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(530, 55);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(15, 12, 15, 12);
            this.pnlRight.Size = new System.Drawing.Size(420, 605);
            this.pnlRight.TabIndex = 2;
            // 
            // btnSendPlan
            // 
            this.btnSendPlan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(140)))), ((int)(((byte)(126)))));
            this.btnSendPlan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSendPlan.FlatAppearance.BorderSize = 0;
            this.btnSendPlan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendPlan.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnSendPlan.ForeColor = System.Drawing.Color.White;
            this.btnSendPlan.Location = new System.Drawing.Point(15, 392);
            this.btnSendPlan.Name = "btnSendPlan";
            this.btnSendPlan.Size = new System.Drawing.Size(388, 50);
            this.btnSendPlan.TabIndex = 5;
            this.btnSendPlan.Text = "📱  إرسال الخطة عبر WhatsApp";
            this.btnSendPlan.UseVisualStyleBackColor = false;
            // 
            // txtSelectedPlanPdf
            // 
            this.txtSelectedPlanPdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.txtSelectedPlanPdf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSelectedPlanPdf.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSelectedPlanPdf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.txtSelectedPlanPdf.Location = new System.Drawing.Point(15, 354);
            this.txtSelectedPlanPdf.Name = "txtSelectedPlanPdf";
            this.txtSelectedPlanPdf.ReadOnly = true;
            this.txtSelectedPlanPdf.Size = new System.Drawing.Size(388, 24);
            this.txtSelectedPlanPdf.TabIndex = 4;
            this.txtSelectedPlanPdf.Text = "المسار يظهر هنا تلقائياً عند الاختيار";
            // 
            // cmbSelectPlan
            // 
            this.cmbSelectPlan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(55)))));
            this.cmbSelectPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectPlan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSelectPlan.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbSelectPlan.ForeColor = System.Drawing.Color.White;
            this.cmbSelectPlan.Location = new System.Drawing.Point(15, 316);
            this.cmbSelectPlan.Name = "cmbSelectPlan";
            this.cmbSelectPlan.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbSelectPlan.Size = new System.Drawing.Size(388, 28);
            this.cmbSelectPlan.TabIndex = 3;
            // 
            // lblSelectPlan
            // 
            this.lblSelectPlan.AutoSize = true;
            this.lblSelectPlan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSelectPlan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(190)))));
            this.lblSelectPlan.Location = new System.Drawing.Point(15, 288);
            this.lblSelectPlan.Name = "lblSelectPlan";
            this.lblSelectPlan.Size = new System.Drawing.Size(160, 20);
            this.lblSelectPlan.TabIndex = 2;
            this.lblSelectPlan.Text = "📋  اختر الخطة للإرسال:";
            // 
            // pnlCreatePlan
            // 
            this.pnlCreatePlan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(48)))));
            this.pnlCreatePlan.BorderRadius = 14;
            this.pnlCreatePlan.ShadowDecoration.Enabled = true;
            this.pnlCreatePlan.Controls.Add(this.btnSavePlan);
            this.pnlCreatePlan.Controls.Add(this.btnBrowsePlanPdf);
            this.pnlCreatePlan.Controls.Add(this.txtPlanPdf);
            this.pnlCreatePlan.Controls.Add(this.lblPlanPdf);
            this.pnlCreatePlan.Controls.Add(this.txtPlanName);
            this.pnlCreatePlan.Controls.Add(this.lblPlanName);
            this.pnlCreatePlan.Controls.Add(this.lblCreateTitle);
            this.pnlCreatePlan.Location = new System.Drawing.Point(15, 48);
            this.pnlCreatePlan.Name = "pnlCreatePlan";
            this.pnlCreatePlan.Padding = new System.Windows.Forms.Padding(12);
            this.pnlCreatePlan.Size = new System.Drawing.Size(388, 225);
            this.pnlCreatePlan.TabIndex = 1;
            // 
            // btnSavePlan
            // 
            this.btnSavePlan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnSavePlan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSavePlan.BorderRadius = 8;
            this.btnSavePlan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSavePlan.ForeColor = System.Drawing.Color.White;
            this.btnSavePlan.Location = new System.Drawing.Point(12, 168);
            this.btnSavePlan.Name = "btnSavePlan";
            this.btnSavePlan.Size = new System.Drawing.Size(362, 40);
            this.btnSavePlan.TabIndex = 6;
            this.btnSavePlan.Text = "✓  حفظ الخطة";
            // 
            // btnBrowsePlanPdf
            // 
            this.btnBrowsePlanPdf.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.btnBrowsePlanPdf.BorderRadius = 6;
            this.btnBrowsePlanPdf.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBrowsePlanPdf.ForeColor = System.Drawing.Color.White;
            this.btnBrowsePlanPdf.Location = new System.Drawing.Point(288, 124);
            this.btnBrowsePlanPdf.Name = "btnBrowsePlanPdf";
            this.btnBrowsePlanPdf.Size = new System.Drawing.Size(86, 25);
            this.btnBrowsePlanPdf.TabIndex = 5;
            this.btnBrowsePlanPdf.Text = "📁 اختيار";
            // 
            // txtPlanPdf
            // 
            this.txtPlanPdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(58)))));
            this.txtPlanPdf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPlanPdf.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPlanPdf.ForeColor = System.Drawing.Color.White;
            this.txtPlanPdf.Location = new System.Drawing.Point(12, 124);
            this.txtPlanPdf.Name = "txtPlanPdf";
            this.txtPlanPdf.ReadOnly = true;
            this.txtPlanPdf.Size = new System.Drawing.Size(270, 25);
            this.txtPlanPdf.TabIndex = 4;
            // 
            // lblPlanPdf
            // 
            this.lblPlanPdf.AutoSize = true;
            this.lblPlanPdf.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPlanPdf.ForeColor = System.Drawing.Color.LightGray;
            this.lblPlanPdf.Location = new System.Drawing.Point(12, 102);
            this.lblPlanPdf.Name = "lblPlanPdf";
            this.lblPlanPdf.Size = new System.Drawing.Size(102, 19);
            this.lblPlanPdf.TabIndex = 3;
            this.lblPlanPdf.Text = "مسار ملف PDF:";
            // 
            // txtPlanName
            // 
            this.txtPlanName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(58)))));
            this.txtPlanName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPlanName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPlanName.ForeColor = System.Drawing.Color.White;
            this.txtPlanName.Location = new System.Drawing.Point(12, 64);
            this.txtPlanName.Name = "txtPlanName";
            this.txtPlanName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtPlanName.Size = new System.Drawing.Size(362, 27);
            this.txtPlanName.TabIndex = 2;
            // 
            // lblPlanName
            // 
            this.lblPlanName.AutoSize = true;
            this.lblPlanName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPlanName.ForeColor = System.Drawing.Color.LightGray;
            this.lblPlanName.Location = new System.Drawing.Point(12, 42);
            this.lblPlanName.Name = "lblPlanName";
            this.lblPlanName.Size = new System.Drawing.Size(76, 19);
            this.lblPlanName.TabIndex = 1;
            this.lblPlanName.Text = "اسم الخطة:";
            // 
            // lblCreateTitle
            // 
            this.lblCreateTitle.AutoSize = true;
            this.lblCreateTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCreateTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.lblCreateTitle.Location = new System.Drawing.Point(12, 12);
            this.lblCreateTitle.Name = "lblCreateTitle";
            this.lblCreateTitle.Size = new System.Drawing.Size(147, 20);
            this.lblCreateTitle.TabIndex = 0;
            this.lblCreateTitle.Text = "➕  إنشاء خطة جديدة";
            // 
            // lblRightTitle
            // 
            this.lblRightTitle.AutoSize = true;
            this.lblRightTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblRightTitle.ForeColor = System.Drawing.Color.White;
            this.lblRightTitle.Location = new System.Drawing.Point(15, 12);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(162, 28);
            this.lblRightTitle.TabIndex = 0;
            this.lblRightTitle.Text = "📂  خطط التغذية";
            // 
            // DietPlanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(950, 660);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.lblTitle);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "DietPlanForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "إدارة خطط التغذية";
            this.Load += new System.EventHandler(this.DietPlanForm_Load);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlMemberInfo.ResumeLayout(false);
            this.pnlMemberInfo.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlCreatePlan.ResumeLayout(false);
            this.pnlCreatePlan.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
