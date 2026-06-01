namespace gym_mangment_system
{
    partial class MembersForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private Guna.UI2.WinForms.Guna2GradientButton btnAddMember;
        private Guna.UI2.WinForms.Guna2DataGridView gridMembers;
        private System.Windows.Forms.Panel pnlActions;
        private Guna.UI2.WinForms.Guna2Button btnEdit;
        private Guna.UI2.WinForms.Guna2Button btnWhatsApp;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private System.Windows.Forms.Label lblMemberCount;

        // ── Add/Edit inline panel ──
        private Guna.UI2.WinForms.Guna2Panel pnlForm;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFName;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.Label lblFPhone;
        private System.Windows.Forms.TextBox txtFPhone;
        private System.Windows.Forms.Label lblFGender;
        private System.Windows.Forms.ComboBox cmbFGender;
        private System.Windows.Forms.Label lblFPlan;
        private System.Windows.Forms.ComboBox cmbFPlan;
        private System.Windows.Forms.Label lblFPlanPrice;
        private System.Windows.Forms.TextBox txtFPlanPrice;
        private System.Windows.Forms.Label lblFPlanMonths;
        private System.Windows.Forms.TextBox txtFPlanMonths;
        private Guna.UI2.WinForms.Guna2Button btnFormSave;
        private Guna.UI2.WinForms.Guna2Button btnFormCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle       = new System.Windows.Forms.Label();
            this.pnlSearch      = new System.Windows.Forms.Panel();
            this.btnAddMember   = new Guna.UI2.WinForms.Guna2GradientButton();
            this.txtSearch      = new System.Windows.Forms.TextBox();
            this.lblSearch      = new System.Windows.Forms.Label();
            this.gridMembers    = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlActions     = new System.Windows.Forms.Panel();
            this.lblMemberCount = new System.Windows.Forms.Label();
            this.btnDelete      = new Guna.UI2.WinForms.Guna2Button();
            this.btnWhatsApp    = new Guna.UI2.WinForms.Guna2Button();
            this.btnEdit        = new Guna.UI2.WinForms.Guna2Button();

            this.pnlForm        = new Guna.UI2.WinForms.Guna2Panel();
            this.btnFormCancel  = new Guna.UI2.WinForms.Guna2Button();
            this.btnFormSave    = new Guna.UI2.WinForms.Guna2Button();
            this.cmbFPlan       = new System.Windows.Forms.ComboBox();
            this.lblFPlan       = new System.Windows.Forms.Label();
            this.txtFPlanPrice  = new System.Windows.Forms.TextBox();
            this.lblFPlanPrice  = new System.Windows.Forms.Label();
            this.txtFPlanMonths = new System.Windows.Forms.TextBox();
            this.lblFPlanMonths = new System.Windows.Forms.Label();
            this.cmbFGender     = new System.Windows.Forms.ComboBox();
            this.lblFGender     = new System.Windows.Forms.Label();
            this.txtFPhone      = new System.Windows.Forms.TextBox();
            this.lblFPhone      = new System.Windows.Forms.Label();
            this.txtFName       = new System.Windows.Forms.TextBox();
            this.lblFName       = new System.Windows.Forms.Label();
            this.lblFormTitle   = new System.Windows.Forms.Label();

            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMembers)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.Padding   = new System.Windows.Forms.Padding(15, 8, 15, 0);
            this.lblTitle.Size      = new System.Drawing.Size(950, 50);
            this.lblTitle.TabIndex  = 0;
            this.lblTitle.Text      = "👥  إدارة الأعضاء";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // pnlSearch
            this.pnlSearch.Controls.Add(this.btnAddMember);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Dock     = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Name     = "pnlSearch";
            this.pnlSearch.Padding  = new System.Windows.Forms.Padding(20, 5, 20, 5);
            this.pnlSearch.Size     = new System.Drawing.Size(950, 55);
            this.pnlSearch.TabIndex = 1;

            this.lblSearch.AutoSize  = true;
            this.lblSearch.Dock      = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font      = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(150, 150, 160);
            this.lblSearch.Name      = "lblSearch";
            this.lblSearch.Padding   = new System.Windows.Forms.Padding(0, 10, 5, 0);
            this.lblSearch.TabIndex  = 0;
            this.lblSearch.Text      = "🔍 بحث:";

            this.txtSearch.Anchor      = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.txtSearch.BackColor   = System.Drawing.Color.FromArgb(38, 38, 45);
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font        = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearch.ForeColor   = System.Drawing.Color.White;
            this.txtSearch.Location    = new System.Drawing.Point(530, 12);
            this.txtSearch.Name        = "txtSearch";
            this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSearch.Size        = new System.Drawing.Size(320, 29);
            this.txtSearch.TabIndex    = 1;

            this.btnAddMember.FillColor  = System.Drawing.Color.FromArgb(152, 16, 250);
            this.btnAddMember.FillColor2 = System.Drawing.Color.FromArgb(230, 0, 118);
            this.btnAddMember.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnAddMember.BorderRadius = 12;
            this.btnAddMember.Font       = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddMember.ForeColor  = System.Drawing.Color.White;
            this.btnAddMember.Location   = new System.Drawing.Point(25, 10);
            this.btnAddMember.Name       = "btnAddMember";
            this.btnAddMember.Size       = new System.Drawing.Size(180, 35);
            this.btnAddMember.TabIndex   = 2;
            this.btnAddMember.Text       = "➕ إضافة عضو جديد";

            // gridMembers
            this.gridMembers.AllowUserToAddRows    = false;
            this.gridMembers.AllowUserToDeleteRows = false;
            this.gridMembers.BackgroundColor       = System.Drawing.Color.FromArgb(24, 24, 30);
            this.gridMembers.BorderStyle           = System.Windows.Forms.BorderStyle.None;
            this.gridMembers.CellBorderStyle       = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridMembers.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(38, 38, 45), ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = System.Drawing.Color.FromArgb(38, 38, 45), SelectionForeColor = System.Drawing.Color.White
            };
            this.gridMembers.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(26, 26, 32), ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                SelectionBackColor = System.Drawing.Color.FromArgb(50, 50, 60), SelectionForeColor = System.Drawing.Color.White,
                Padding = new System.Windows.Forms.Padding(5, 3, 5, 3)
            };
            this.gridMembers.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            { BackColor = System.Drawing.Color.FromArgb(30, 30, 36) };
            this.gridMembers.ColumnHeadersHeight         = 48;
            this.gridMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridMembers.Dock                        = System.Windows.Forms.DockStyle.Fill;
            this.gridMembers.EnableHeadersVisualStyles   = false;
            this.gridMembers.GridColor                   = System.Drawing.Color.FromArgb(40, 40, 48);
            this.gridMembers.Name                        = "gridMembers";
            this.gridMembers.ReadOnly                    = true;
            this.gridMembers.RightToLeft                 = System.Windows.Forms.RightToLeft.Yes;
            this.gridMembers.RowHeadersVisible           = false;
            this.gridMembers.RowTemplate.Height          = 44;
            this.gridMembers.SelectionMode               = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMembers.TabIndex                    = 2;

            // pnlActions
            this.pnlActions.Controls.Add(this.lblMemberCount);
            this.pnlActions.Controls.Add(this.btnDelete);
            this.pnlActions.Controls.Add(this.btnWhatsApp);
            this.pnlActions.Controls.Add(this.btnEdit);
            this.pnlActions.Dock     = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Name     = "pnlActions";
            this.pnlActions.Padding  = new System.Windows.Forms.Padding(20, 8, 20, 8);
            this.pnlActions.Size     = new System.Drawing.Size(950, 55);
            this.pnlActions.TabIndex = 3;

            this.btnWhatsApp.FillColor  = System.Drawing.Color.FromArgb(37, 211, 102);
            this.btnWhatsApp.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnWhatsApp.Dock       = System.Windows.Forms.DockStyle.Left;
            this.btnWhatsApp.BorderRadius = 8;
            this.btnWhatsApp.Font       = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnWhatsApp.ForeColor  = System.Drawing.Color.White;
            this.btnWhatsApp.Margin     = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnWhatsApp.Name       = "btnWhatsApp";
            this.btnWhatsApp.Size       = new System.Drawing.Size(170, 39);
            this.btnWhatsApp.TabIndex   = 2;
            this.btnWhatsApp.Text       = "📱 واتساب";

            this.btnEdit.FillColor  = System.Drawing.Color.FromArgb(43, 127, 255);
            this.btnEdit.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Dock       = System.Windows.Forms.DockStyle.Left;
            this.btnEdit.BorderRadius = 8;
            this.btnEdit.Font       = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor  = System.Drawing.Color.White;
            this.btnEdit.Name       = "btnEdit";
            this.btnEdit.Size       = new System.Drawing.Size(160, 39);
            this.btnEdit.TabIndex   = 4;
            this.btnEdit.Text       = "✏️ تعديل العضو";

            this.btnDelete.FillColor  = System.Drawing.Color.FromArgb(231, 0, 11);
            this.btnDelete.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Dock       = System.Windows.Forms.DockStyle.Left;
            this.btnDelete.BorderRadius = 8;
            this.btnDelete.Font       = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor  = System.Drawing.Color.White;
            this.btnDelete.Margin     = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDelete.Name       = "btnDelete";
            this.btnDelete.Size       = new System.Drawing.Size(160, 39);
            this.btnDelete.TabIndex   = 3;
            this.btnDelete.Text       = "🗑️  حذف العضو";

            this.lblMemberCount.Dock      = System.Windows.Forms.DockStyle.Right;
            this.lblMemberCount.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblMemberCount.ForeColor = System.Drawing.Color.FromArgb(140, 140, 150);
            this.lblMemberCount.Name      = "lblMemberCount";
            this.lblMemberCount.Size      = new System.Drawing.Size(200, 39);
            this.lblMemberCount.TabIndex  = 5;
            this.lblMemberCount.Text      = "إجمالي الأعضاء: 0";
            this.lblMemberCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── pnlForm (overlay) ──
            this.pnlForm.FillColor = System.Drawing.Color.FromArgb(32, 32, 40);
            this.pnlForm.BorderRadius = 14;
            this.pnlForm.ShadowDecoration.Enabled = true;
            this.pnlForm.Controls.Add(this.btnFormCancel);
            this.pnlForm.Controls.Add(this.btnFormSave);
            this.pnlForm.Controls.Add(this.txtFPlanMonths);
            this.pnlForm.Controls.Add(this.lblFPlanMonths);
            this.pnlForm.Controls.Add(this.txtFPlanPrice);
            this.pnlForm.Controls.Add(this.lblFPlanPrice);
            this.pnlForm.Controls.Add(this.cmbFPlan);
            this.pnlForm.Controls.Add(this.lblFPlan);
            this.pnlForm.Controls.Add(this.cmbFGender);
            this.pnlForm.Controls.Add(this.lblFGender);
            this.pnlForm.Controls.Add(this.txtFPhone);
            this.pnlForm.Controls.Add(this.lblFPhone);
            this.pnlForm.Controls.Add(this.txtFName);
            this.pnlForm.Controls.Add(this.lblFName);
            this.pnlForm.Controls.Add(this.lblFormTitle);
            this.pnlForm.Location = new System.Drawing.Point(220, 100);
            this.pnlForm.Name     = "pnlForm";
            this.pnlForm.Padding  = new System.Windows.Forms.Padding(25);
            this.pnlForm.Size     = new System.Drawing.Size(500, 430);
            this.pnlForm.TabIndex = 10;
            this.pnlForm.Visible  = false;

            // lblFormTitle
            this.lblFormTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblFormTitle.Font      = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Name      = "lblFormTitle";
            this.lblFormTitle.Size      = new System.Drawing.Size(450, 40);
            this.lblFormTitle.TabIndex  = 0;
            this.lblFormTitle.Text      = "➕  إضافة عضو جديد";
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Name
            this.lblFName.AutoSize  = true;
            this.lblFName.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFName.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.lblFName.Location  = new System.Drawing.Point(395, 58);
            this.lblFName.Name      = "lblFName";
            this.lblFName.TabIndex  = 1;
            this.lblFName.Text      = "الاسم الكامل:";

            this.txtFName.BackColor   = System.Drawing.Color.FromArgb(45, 45, 55);
            this.txtFName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFName.Font        = new System.Drawing.Font("Segoe UI", 11F);
            this.txtFName.ForeColor   = System.Drawing.Color.White;
            this.txtFName.Location    = new System.Drawing.Point(35, 80);
            this.txtFName.Name        = "txtFName";
            this.txtFName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFName.Size        = new System.Drawing.Size(435, 27);
            this.txtFName.TabIndex    = 2;

            // Phone
            this.lblFPhone.AutoSize  = true;
            this.lblFPhone.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFPhone.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.lblFPhone.Location  = new System.Drawing.Point(400, 118);
            this.lblFPhone.Name      = "lblFPhone";
            this.lblFPhone.TabIndex  = 3;
            this.lblFPhone.Text      = "رقم الهاتف:";

            this.txtFPhone.BackColor   = System.Drawing.Color.FromArgb(45, 45, 55);
            this.txtFPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFPhone.Font        = new System.Drawing.Font("Segoe UI", 11F);
            this.txtFPhone.ForeColor   = System.Drawing.Color.White;
            this.txtFPhone.Location    = new System.Drawing.Point(35, 140);
            this.txtFPhone.Name        = "txtFPhone";
            this.txtFPhone.Size        = new System.Drawing.Size(435, 27);
            this.txtFPhone.TabIndex    = 4;

            // Gender
            this.lblFGender.AutoSize  = true;
            this.lblFGender.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFGender.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.lblFGender.Location  = new System.Drawing.Point(415, 178);
            this.lblFGender.Name      = "lblFGender";
            this.lblFGender.TabIndex  = 5;
            this.lblFGender.Text      = "الجنس:";

            this.cmbFGender.BackColor     = System.Drawing.Color.FromArgb(45, 45, 55);
            this.cmbFGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFGender.FlatStyle     = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFGender.Font          = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbFGender.ForeColor     = System.Drawing.Color.White;
            this.cmbFGender.Items.AddRange(new object[] { "ذكر", "أنثى" });
            this.cmbFGender.Location      = new System.Drawing.Point(35, 200);
            this.cmbFGender.Name          = "cmbFGender";
            this.cmbFGender.RightToLeft   = System.Windows.Forms.RightToLeft.Yes;
            this.cmbFGender.Size          = new System.Drawing.Size(435, 28);
            this.cmbFGender.TabIndex      = 6;

            // Plan
            this.lblFPlan.AutoSize  = true;
            this.lblFPlan.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFPlan.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.lblFPlan.Location  = new System.Drawing.Point(392, 238);
            this.lblFPlan.Name      = "lblFPlan";
            this.lblFPlan.TabIndex  = 7;
            this.lblFPlan.Text      = "نوع الاشتراك:";

            this.cmbFPlan.BackColor     = System.Drawing.Color.FromArgb(45, 45, 55);
            this.cmbFPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFPlan.FlatStyle     = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFPlan.Font          = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbFPlan.ForeColor     = System.Drawing.Color.White;
            this.cmbFPlan.Location      = new System.Drawing.Point(35, 260);
            this.cmbFPlan.Name          = "cmbFPlan";
            this.cmbFPlan.RightToLeft   = System.Windows.Forms.RightToLeft.Yes;
            this.cmbFPlan.Size          = new System.Drawing.Size(435, 28);
            this.cmbFPlan.TabIndex      = 8;

            // Price (read-only, auto-filled)
            this.lblFPlanPrice.AutoSize  = true;
            this.lblFPlanPrice.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFPlanPrice.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.lblFPlanPrice.Location  = new System.Drawing.Point(415, 300);
            this.lblFPlanPrice.Name      = "lblFPlanPrice";
            this.lblFPlanPrice.TabIndex  = 9;
            this.lblFPlanPrice.Text      = "السعر:";

            this.txtFPlanPrice.BackColor   = System.Drawing.Color.FromArgb(30, 30, 38);
            this.txtFPlanPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFPlanPrice.Font        = new System.Drawing.Font("Segoe UI", 11F);
            this.txtFPlanPrice.ForeColor   = System.Drawing.Color.FromArgb(0, 166, 62);
            this.txtFPlanPrice.Location    = new System.Drawing.Point(235, 320);
            this.txtFPlanPrice.Name        = "txtFPlanPrice";
            this.txtFPlanPrice.ReadOnly    = true;
            this.txtFPlanPrice.Size        = new System.Drawing.Size(235, 27);
            this.txtFPlanPrice.TabIndex    = 10;

            // Duration (read-only, auto-filled)
            this.lblFPlanMonths.AutoSize  = true;
            this.lblFPlanMonths.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFPlanMonths.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.lblFPlanMonths.Location  = new System.Drawing.Point(178, 300);
            this.lblFPlanMonths.Name      = "lblFPlanMonths";
            this.lblFPlanMonths.TabIndex  = 11;
            this.lblFPlanMonths.Text      = "المدة:";

            this.txtFPlanMonths.BackColor   = System.Drawing.Color.FromArgb(30, 30, 38);
            this.txtFPlanMonths.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFPlanMonths.Font        = new System.Drawing.Font("Segoe UI", 11F);
            this.txtFPlanMonths.ForeColor   = System.Drawing.Color.FromArgb(43, 127, 255);
            this.txtFPlanMonths.Location    = new System.Drawing.Point(35, 320);
            this.txtFPlanMonths.Name        = "txtFPlanMonths";
            this.txtFPlanMonths.ReadOnly    = true;
            this.txtFPlanMonths.Size        = new System.Drawing.Size(190, 27);
            this.txtFPlanMonths.TabIndex    = 12;

            // Save / Cancel
            this.btnFormSave.FillColor  = System.Drawing.Color.FromArgb(0, 166, 62);
            this.btnFormSave.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnFormSave.BorderRadius = 8;
            this.btnFormSave.Font       = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFormSave.ForeColor  = System.Drawing.Color.White;
            this.btnFormSave.Location   = new System.Drawing.Point(245, 370);
            this.btnFormSave.Name       = "btnFormSave";
            this.btnFormSave.Size       = new System.Drawing.Size(225, 40);
            this.btnFormSave.TabIndex   = 13;
            this.btnFormSave.Text       = "✓  حفظ";

            this.btnFormCancel.FillColor  = System.Drawing.Color.FromArgb(55, 55, 65);
            this.btnFormCancel.Cursor     = System.Windows.Forms.Cursors.Hand;
            this.btnFormCancel.BorderRadius = 8;
            this.btnFormCancel.Font       = new System.Drawing.Font("Segoe UI", 12F);
            this.btnFormCancel.ForeColor  = System.Drawing.Color.LightGray;
            this.btnFormCancel.Location   = new System.Drawing.Point(35, 370);
            this.btnFormCancel.Name       = "btnFormCancel";
            this.btnFormCancel.Size       = new System.Drawing.Size(200, 40);
            this.btnFormCancel.TabIndex   = 14;
            this.btnFormCancel.Text       = "إلغاء";

            // MembersForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.FromArgb(18, 18, 22);
            this.ClientSize          = new System.Drawing.Size(950, 660);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.gridMembers);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.lblTitle);
            this.Name        = "MembersForm";
            this.Padding     = new System.Windows.Forms.Padding(0);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text        = "إدارة الأعضاء";

            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMembers)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
