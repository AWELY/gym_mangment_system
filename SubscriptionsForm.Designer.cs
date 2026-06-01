namespace gym_mangment_system
{
    partial class SubscriptionsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView gridSubs;
        private System.Windows.Forms.Panel pnlEditor;
        private System.Windows.Forms.Label lblMember;
        private System.Windows.Forms.ComboBox cmbMember;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.NumericUpDown numDuration;
        private System.Windows.Forms.ComboBox cmbDurationUnit;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.NumericUpDown numPrice;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Button btnSaveSubscription;
        private System.Windows.Forms.Button btnClearForm;
        private System.Windows.Forms.Button btnDeleteSubscription;
        private System.Windows.Forms.Label lblHint;

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
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnDeleteSubscription = new System.Windows.Forms.Button();
            this.btnClearForm = new System.Windows.Forms.Button();
            this.btnSaveSubscription = new System.Windows.Forms.Button();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.lblPrice = new System.Windows.Forms.Label();
            this.cmbDurationUnit = new System.Windows.Forms.ComboBox();
            this.numDuration = new System.Windows.Forms.NumericUpDown();
            this.lblDuration = new System.Windows.Forms.Label();
            this.txtType = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbMember = new System.Windows.Forms.ComboBox();
            this.lblMember = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridSubs)).BeginInit();
            this.pnlEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuration)).BeginInit();
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
            this.lblTitle.Text = "👥  إدارة الاشتراكات";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gridSubs
            // 
            this.gridSubs.AllowUserToAddRows = false;
            this.gridSubs.AllowUserToDeleteRows = false;
            this.gridSubs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.gridSubs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridSubs.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { 
                BackColor = System.Drawing.Color.FromArgb(45, 45, 45),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            };
            this.gridSubs.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle {
                BackColor = System.Drawing.Color.FromArgb(30, 30, 30),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                SelectionBackColor = System.Drawing.Color.FromArgb(60, 60, 60),
                SelectionForeColor = System.Drawing.Color.White
            };
            this.gridSubs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSubs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSubs.EnableHeadersVisualStyles = false;
            this.gridSubs.GridColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.gridSubs.Location = new System.Drawing.Point(20, 235);
            this.gridSubs.Name = "gridSubs";
            this.gridSubs.ReadOnly = true;
            this.gridSubs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridSubs.RowHeadersVisible = false;
            this.gridSubs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSubs.Size = new System.Drawing.Size(910, 345);
            this.gridSubs.TabIndex = 1;
            // 
            // pnlEditor
            // 
            this.pnlEditor.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.pnlEditor.Controls.Add(this.lblHint);
            this.pnlEditor.Controls.Add(this.btnDeleteSubscription);
            this.pnlEditor.Controls.Add(this.btnClearForm);
            this.pnlEditor.Controls.Add(this.btnSaveSubscription);
            this.pnlEditor.Controls.Add(this.dtpStartDate);
            this.pnlEditor.Controls.Add(this.lblStartDate);
            this.pnlEditor.Controls.Add(this.numPrice);
            this.pnlEditor.Controls.Add(this.lblPrice);
            this.pnlEditor.Controls.Add(this.cmbDurationUnit);
            this.pnlEditor.Controls.Add(this.numDuration);
            this.pnlEditor.Controls.Add(this.lblDuration);
            this.pnlEditor.Controls.Add(this.txtType);
            this.pnlEditor.Controls.Add(this.lblType);
            this.pnlEditor.Controls.Add(this.cmbMember);
            this.pnlEditor.Controls.Add(this.lblMember);
            this.pnlEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEditor.Location = new System.Drawing.Point(20, 75);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Padding = new System.Windows.Forms.Padding(15);
            this.pnlEditor.Size = new System.Drawing.Size(910, 160);
            this.pnlEditor.TabIndex = 2;
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.Color.Silver;
            this.lblHint.Location = new System.Drawing.Point(535, 128);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(358, 13);
            this.lblHint.TabIndex = 14;
            this.lblHint.Text = "اختر عضو، اكتب نوع الاشتراك، المدة، والسعر ثم اضغط حفظ. للتعديل اختر صف من الجدول.";
            // 
            // btnDeleteSubscription
            // 
            this.btnDeleteSubscription.BackColor = System.Drawing.Color.FromArgb(231, 0, 11);
            this.btnDeleteSubscription.FlatAppearance.BorderSize = 0;
            this.btnDeleteSubscription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteSubscription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteSubscription.ForeColor = System.Drawing.Color.White;
            this.btnDeleteSubscription.Location = new System.Drawing.Point(15, 111);
            this.btnDeleteSubscription.Name = "btnDeleteSubscription";
            this.btnDeleteSubscription.Size = new System.Drawing.Size(130, 34);
            this.btnDeleteSubscription.TabIndex = 13;
            this.btnDeleteSubscription.Text = "🗑 حذف المحدد";
            this.btnDeleteSubscription.UseVisualStyleBackColor = false;
            // 
            // btnClearForm
            // 
            this.btnClearForm.BackColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.btnClearForm.FlatAppearance.BorderSize = 0;
            this.btnClearForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearForm.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClearForm.ForeColor = System.Drawing.Color.White;
            this.btnClearForm.Location = new System.Drawing.Point(151, 111);
            this.btnClearForm.Name = "btnClearForm";
            this.btnClearForm.Size = new System.Drawing.Size(120, 34);
            this.btnClearForm.TabIndex = 12;
            this.btnClearForm.Text = "تفريغ";
            this.btnClearForm.UseVisualStyleBackColor = false;
            // 
            // btnSaveSubscription
            // 
            this.btnSaveSubscription.BackColor = System.Drawing.Color.FromArgb(0, 166, 62);
            this.btnSaveSubscription.FlatAppearance.BorderSize = 0;
            this.btnSaveSubscription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSubscription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveSubscription.ForeColor = System.Drawing.Color.White;
            this.btnSaveSubscription.Location = new System.Drawing.Point(277, 111);
            this.btnSaveSubscription.Name = "btnSaveSubscription";
            this.btnSaveSubscription.Size = new System.Drawing.Size(150, 34);
            this.btnSaveSubscription.TabIndex = 11;
            this.btnSaveSubscription.Text = "💾 حفظ الاشتراك";
            this.btnSaveSubscription.UseVisualStyleBackColor = false;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CalendarForeColor = System.Drawing.Color.White;
            this.dtpStartDate.CalendarMonthBackground = System.Drawing.Color.FromArgb(45, 45, 45);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(15, 73);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(190, 20);
            this.dtpStartDate.TabIndex = 10;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStartDate.ForeColor = System.Drawing.Color.White;
            this.lblStartDate.Location = new System.Drawing.Point(121, 51);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(84, 19);
            this.lblStartDate.TabIndex = 9;
            this.lblStartDate.Text = "تاريخ البداية";
            // 
            // numPrice
            // 
            this.numPrice.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.ForeColor = System.Drawing.Color.White;
            this.numPrice.Location = new System.Drawing.Point(227, 73);
            this.numPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(200, 20);
            this.numPrice.TabIndex = 8;
            this.numPrice.ThousandsSeparator = true;
            this.numPrice.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrice.ForeColor = System.Drawing.Color.White;
            this.lblPrice.Location = new System.Drawing.Point(376, 51);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(51, 19);
            this.lblPrice.TabIndex = 7;
            this.lblPrice.Text = "السعر $";
            // 
            // cmbDurationUnit
            // 
            this.cmbDurationUnit.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.cmbDurationUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDurationUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDurationUnit.ForeColor = System.Drawing.Color.White;
            this.cmbDurationUnit.FormattingEnabled = true;
            this.cmbDurationUnit.Items.AddRange(new object[] {
            "شهر",
            "سنة"});
            this.cmbDurationUnit.Location = new System.Drawing.Point(433, 73);
            this.cmbDurationUnit.Name = "cmbDurationUnit";
            this.cmbDurationUnit.Size = new System.Drawing.Size(100, 21);
            this.cmbDurationUnit.TabIndex = 6;
            // 
            // numDuration
            // 
            this.numDuration.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.numDuration.ForeColor = System.Drawing.Color.White;
            this.numDuration.Location = new System.Drawing.Point(539, 73);
            this.numDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDuration.Name = "numDuration";
            this.numDuration.Size = new System.Drawing.Size(93, 20);
            this.numDuration.TabIndex = 5;
            this.numDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDuration.ForeColor = System.Drawing.Color.White;
            this.lblDuration.Location = new System.Drawing.Point(566, 51);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(66, 19);
            this.lblDuration.TabIndex = 4;
            this.lblDuration.Text = "مدة الاشتراك";
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.txtType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtType.ForeColor = System.Drawing.Color.White;
            this.txtType.Location = new System.Drawing.Point(638, 73);
            this.txtType.Name = "txtType";
            this.txtType.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtType.Size = new System.Drawing.Size(121, 20);
            this.txtType.TabIndex = 3;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblType.ForeColor = System.Drawing.Color.White;
            this.lblType.Location = new System.Drawing.Point(682, 51);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(77, 19);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "نوع الاشتراك";
            // 
            // cmbMember
            // 
            this.cmbMember.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.cmbMember.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbMember.ForeColor = System.Drawing.Color.White;
            this.cmbMember.FormattingEnabled = true;
            this.cmbMember.Location = new System.Drawing.Point(765, 73);
            this.cmbMember.Name = "cmbMember";
            this.cmbMember.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbMember.Size = new System.Drawing.Size(125, 21);
            this.cmbMember.TabIndex = 1;
            // 
            // lblMember
            // 
            this.lblMember.AutoSize = true;
            this.lblMember.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMember.ForeColor = System.Drawing.Color.White;
            this.lblMember.Location = new System.Drawing.Point(840, 51);
            this.lblMember.Name = "lblMember";
            this.lblMember.Size = new System.Drawing.Size(50, 19);
            this.lblMember.TabIndex = 0;
            this.lblMember.Text = "العضو";
            // 
            // SubscriptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(950, 660);
            this.Controls.Add(this.gridSubs);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.lblTitle);
            this.Name = "SubscriptionsForm";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "إدارة الاشتراكات";
            ((System.ComponentModel.ISupportInitialize)(this.gridSubs)).EndInit();
            this.pnlEditor.ResumeLayout(false);
            this.pnlEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuration)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
