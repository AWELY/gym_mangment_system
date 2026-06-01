namespace gym_mangment_system
{
    partial class StoreForm
    {
        private System.ComponentModel.IContainer components = null;

        // Left: Products
        private System.Windows.Forms.Panel pnlProducts;
        private System.Windows.Forms.Label lblProductsTitle;
        private System.Windows.Forms.FlowLayoutPanel flowProducts;
        private System.Windows.Forms.Button btnAddNewItem;

        // Right: Cart
        private System.Windows.Forms.Panel pnlCart;
        private System.Windows.Forms.Label lblCartTitle;
        private System.Windows.Forms.Label lblCartCount;
        private System.Windows.Forms.Panel pnlCartItems;
        private System.Windows.Forms.FlowLayoutPanel flowCartItems;
        private System.Windows.Forms.Panel pnlCartFooter;
        private System.Windows.Forms.Label lblTotalLabel;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.Button btnClearCart;

        // Add Item Dialog
        private System.Windows.Forms.Panel pnlAddItem;
        private System.Windows.Forms.Label lblAddItemTitle;
        private System.Windows.Forms.Label lblNewName;
        private System.Windows.Forms.TextBox txtNewName;
        private System.Windows.Forms.Label lblNewPrice;
        private System.Windows.Forms.NumericUpDown numNewPrice;
        private System.Windows.Forms.Label lblNewCategory;
        private System.Windows.Forms.ComboBox cmbNewCategory;
        private System.Windows.Forms.Label lblNewStock;
        private System.Windows.Forms.NumericUpDown numNewStock;
        private System.Windows.Forms.Label lblNewExpiry;
        private System.Windows.Forms.DateTimePicker dtpNewExpiry;
        private System.Windows.Forms.Label lblNewImage;
        private System.Windows.Forms.PictureBox picNewImage;
        private System.Windows.Forms.Button btnBrowseImage;
        private System.Windows.Forms.Button btnConfirmAdd;
        private System.Windows.Forms.Button btnCancelAdd;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlProducts    = new System.Windows.Forms.Panel();
            this.flowProducts   = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddNewItem  = new System.Windows.Forms.Button();
            this.lblProductsTitle = new System.Windows.Forms.Label();
            this.pnlCart        = new System.Windows.Forms.Panel();
            this.pnlCartItems   = new System.Windows.Forms.Panel();
            this.flowCartItems  = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCartFooter  = new System.Windows.Forms.Panel();
            this.btnClearCart   = new System.Windows.Forms.Button();
            this.btnCheckout    = new System.Windows.Forms.Button();
            this.lblTotalValue  = new System.Windows.Forms.Label();
            this.lblTotalLabel  = new System.Windows.Forms.Label();
            this.lblCartCount   = new System.Windows.Forms.Label();
            this.lblCartTitle   = new System.Windows.Forms.Label();
            this.pnlAddItem     = new System.Windows.Forms.Panel();
            this.btnCancelAdd   = new System.Windows.Forms.Button();
            this.btnConfirmAdd  = new System.Windows.Forms.Button();
            this.cmbNewCategory = new System.Windows.Forms.ComboBox();
            this.lblNewCategory = new System.Windows.Forms.Label();
            this.numNewPrice    = new System.Windows.Forms.NumericUpDown();
            this.lblNewPrice    = new System.Windows.Forms.Label();
            this.numNewStock    = new System.Windows.Forms.NumericUpDown();
            this.lblNewStock    = new System.Windows.Forms.Label();
            this.dtpNewExpiry   = new System.Windows.Forms.DateTimePicker();
            this.lblNewExpiry   = new System.Windows.Forms.Label();
            this.txtNewName     = new System.Windows.Forms.TextBox();
            this.lblNewName     = new System.Windows.Forms.Label();
            this.lblAddItemTitle = new System.Windows.Forms.Label();
            this.lblNewImage    = new System.Windows.Forms.Label();
            this.picNewImage    = new System.Windows.Forms.PictureBox();
            this.btnBrowseImage = new System.Windows.Forms.Button();

            this.pnlProducts.SuspendLayout();
            this.pnlCart.SuspendLayout();
            this.pnlCartItems.SuspendLayout();
            this.pnlCartFooter.SuspendLayout();
            this.pnlAddItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewImage)).BeginInit();
            this.SuspendLayout();

            // pnlProducts
            this.pnlProducts.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.pnlProducts.Controls.Add(this.flowProducts);
            this.pnlProducts.Controls.Add(this.btnAddNewItem);
            this.pnlProducts.Controls.Add(this.lblProductsTitle);
            this.pnlProducts.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.pnlProducts.Name     = "pnlProducts";
            this.pnlProducts.Padding  = new System.Windows.Forms.Padding(15);
            this.pnlProducts.TabIndex = 0;

            this.flowProducts.AutoScroll = true;
            this.flowProducts.BackColor  = System.Drawing.Color.FromArgb(20, 20, 20);
            this.flowProducts.Dock       = System.Windows.Forms.DockStyle.Fill;
            this.flowProducts.Name       = "flowProducts";
            this.flowProducts.Padding    = new System.Windows.Forms.Padding(5);
            this.flowProducts.TabIndex   = 2;

            this.btnAddNewItem.BackColor = System.Drawing.Color.FromArgb(79, 57, 246);
            this.btnAddNewItem.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnAddNewItem.Dock      = System.Windows.Forms.DockStyle.Bottom;
            this.btnAddNewItem.FlatAppearance.BorderSize = 0;
            this.btnAddNewItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewItem.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnAddNewItem.ForeColor = System.Drawing.Color.White;
            this.btnAddNewItem.Name      = "btnAddNewItem";
            this.btnAddNewItem.Size      = new System.Drawing.Size(560, 50);
            this.btnAddNewItem.TabIndex  = 1;
            this.btnAddNewItem.Text      = "➕  إضافة منتج جديد";
            this.btnAddNewItem.UseVisualStyleBackColor = false;

            this.lblProductsTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblProductsTitle.Font      = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblProductsTitle.ForeColor = System.Drawing.Color.White;
            this.lblProductsTitle.Name      = "lblProductsTitle";
            this.lblProductsTitle.Size      = new System.Drawing.Size(560, 45);
            this.lblProductsTitle.TabIndex  = 0;
            this.lblProductsTitle.Text      = "🛒  المنتجات";
            this.lblProductsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // pnlCart
            this.pnlCart.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.pnlCart.Controls.Add(this.pnlCartItems);
            this.pnlCart.Controls.Add(this.pnlCartFooter);
            this.pnlCart.Controls.Add(this.lblCartCount);
            this.pnlCart.Controls.Add(this.lblCartTitle);
            this.pnlCart.Dock     = System.Windows.Forms.DockStyle.Right;
            this.pnlCart.Name     = "pnlCart";
            this.pnlCart.Padding  = new System.Windows.Forms.Padding(12);
            this.pnlCart.Size     = new System.Drawing.Size(360, 660);
            this.pnlCart.TabIndex = 1;

            this.pnlCartItems.Controls.Add(this.flowCartItems);
            this.pnlCartItems.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.pnlCartItems.Name     = "pnlCartItems";
            this.pnlCartItems.TabIndex = 3;

            this.flowCartItems.AutoScroll    = true;
            this.flowCartItems.BackColor     = System.Drawing.Color.FromArgb(30, 30, 30);
            this.flowCartItems.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.flowCartItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowCartItems.Name          = "flowCartItems";
            this.flowCartItems.TabIndex      = 0;
            this.flowCartItems.WrapContents  = false;

            this.pnlCartFooter.Controls.Add(this.btnClearCart);
            this.pnlCartFooter.Controls.Add(this.btnCheckout);
            this.pnlCartFooter.Controls.Add(this.lblTotalValue);
            this.pnlCartFooter.Controls.Add(this.lblTotalLabel);
            this.pnlCartFooter.Dock     = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCartFooter.Name     = "pnlCartFooter";
            this.pnlCartFooter.Size     = new System.Drawing.Size(336, 150);
            this.pnlCartFooter.TabIndex = 2;

            this.btnClearCart.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnClearCart.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnClearCart.FlatAppearance.BorderSize = 0;
            this.btnClearCart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearCart.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.btnClearCart.ForeColor = System.Drawing.Color.LightGray;
            this.btnClearCart.Location  = new System.Drawing.Point(0, 130);
            this.btnClearCart.Name      = "btnClearCart";
            this.btnClearCart.Size      = new System.Drawing.Size(336, 20);
            this.btnClearCart.TabIndex  = 3;
            this.btnClearCart.Text      = "مسح السلة";
            this.btnClearCart.UseVisualStyleBackColor = false;

            this.btnCheckout.BackColor = System.Drawing.Color.FromArgb(0, 166, 62);
            this.btnCheckout.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnCheckout.FlatAppearance.BorderSize = 0;
            this.btnCheckout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckout.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnCheckout.ForeColor = System.Drawing.Color.White;
            this.btnCheckout.Location  = new System.Drawing.Point(0, 82);
            this.btnCheckout.Name      = "btnCheckout";
            this.btnCheckout.Size      = new System.Drawing.Size(336, 45);
            this.btnCheckout.TabIndex  = 2;
            this.btnCheckout.Text      = "✅  إصدار الفاتورة";
            this.btnCheckout.UseVisualStyleBackColor = false;

            this.lblTotalValue.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblTotalValue.Font      = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.ForeColor = System.Drawing.Color.FromArgb(0, 166, 62);
            this.lblTotalValue.Location  = new System.Drawing.Point(0, 25);
            this.lblTotalValue.Name      = "lblTotalValue";
            this.lblTotalValue.Size      = new System.Drawing.Size(336, 50);
            this.lblTotalValue.TabIndex  = 1;
            this.lblTotalValue.Text      = "0.00 $";
            this.lblTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblTotalLabel.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblTotalLabel.Font      = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTotalLabel.ForeColor = System.Drawing.Color.LightGray;
            this.lblTotalLabel.Name      = "lblTotalLabel";
            this.lblTotalLabel.Size      = new System.Drawing.Size(336, 25);
            this.lblTotalLabel.TabIndex  = 0;
            this.lblTotalLabel.Text      = "المجموع الكلي:";
            this.lblTotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblCartCount.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblCartCount.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCartCount.ForeColor = System.Drawing.Color.Gray;
            this.lblCartCount.Name      = "lblCartCount";
            this.lblCartCount.Size      = new System.Drawing.Size(336, 22);
            this.lblCartCount.TabIndex  = 1;
            this.lblCartCount.Text      = "0 عنصر في السلة";
            this.lblCartCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblCartTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblCartTitle.Font      = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblCartTitle.ForeColor = System.Drawing.Color.White;
            this.lblCartTitle.Name      = "lblCartTitle";
            this.lblCartTitle.Size      = new System.Drawing.Size(336, 42);
            this.lblCartTitle.TabIndex  = 0;
            this.lblCartTitle.Text      = "🧾  السلة";
            this.lblCartTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── Add Item Dialog (now taller to fit extra fields) ──
            this.pnlAddItem.BackColor = System.Drawing.Color.FromArgb(35, 35, 35);
            this.pnlAddItem.Controls.Add(this.btnCancelAdd);
            this.pnlAddItem.Controls.Add(this.btnConfirmAdd);
            this.pnlAddItem.Controls.Add(this.picNewImage);
            this.pnlAddItem.Controls.Add(this.btnBrowseImage);
            this.pnlAddItem.Controls.Add(this.lblNewImage);
            this.pnlAddItem.Controls.Add(this.dtpNewExpiry);
            this.pnlAddItem.Controls.Add(this.lblNewExpiry);
            this.pnlAddItem.Controls.Add(this.numNewStock);
            this.pnlAddItem.Controls.Add(this.lblNewStock);
            this.pnlAddItem.Controls.Add(this.cmbNewCategory);
            this.pnlAddItem.Controls.Add(this.lblNewCategory);
            this.pnlAddItem.Controls.Add(this.numNewPrice);
            this.pnlAddItem.Controls.Add(this.lblNewPrice);
            this.pnlAddItem.Controls.Add(this.txtNewName);
            this.pnlAddItem.Controls.Add(this.lblNewName);
            this.pnlAddItem.Controls.Add(this.lblAddItemTitle);
            this.pnlAddItem.Location = new System.Drawing.Point(80, 50);
            this.pnlAddItem.Name     = "pnlAddItem";
            this.pnlAddItem.Padding  = new System.Windows.Forms.Padding(20);
            this.pnlAddItem.Size     = new System.Drawing.Size(420, 560);
            this.pnlAddItem.TabIndex = 10;
            this.pnlAddItem.Visible  = false;

            this.lblAddItemTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblAddItemTitle.Font      = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAddItemTitle.ForeColor = System.Drawing.Color.White;
            this.lblAddItemTitle.Name      = "lblAddItemTitle";
            this.lblAddItemTitle.Size      = new System.Drawing.Size(380, 40);
            this.lblAddItemTitle.TabIndex  = 0;
            this.lblAddItemTitle.Text      = "➕  إضافة منتج جديد";
            this.lblAddItemTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Name
            this.lblNewName.AutoSize  = true;
            this.lblNewName.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNewName.ForeColor = System.Drawing.Color.LightGray;
            this.lblNewName.Location  = new System.Drawing.Point(300, 60);
            this.lblNewName.Name      = "lblNewName";
            this.lblNewName.TabIndex  = 1;
            this.lblNewName.Text      = "اسم المنتج:";

            this.txtNewName.BackColor   = System.Drawing.Color.FromArgb(50, 50, 50);
            this.txtNewName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewName.Font        = new System.Drawing.Font("Segoe UI", 12F);
            this.txtNewName.ForeColor   = System.Drawing.Color.White;
            this.txtNewName.Location    = new System.Drawing.Point(30, 82);
            this.txtNewName.Name        = "txtNewName";
            this.txtNewName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtNewName.Size        = new System.Drawing.Size(360, 29);
            this.txtNewName.TabIndex    = 2;

            // Price
            this.lblNewPrice.AutoSize  = true;
            this.lblNewPrice.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNewPrice.ForeColor = System.Drawing.Color.LightGray;
            this.lblNewPrice.Location  = new System.Drawing.Point(300, 122);
            this.lblNewPrice.Name      = "lblNewPrice";
            this.lblNewPrice.TabIndex  = 3;
            this.lblNewPrice.Text      = "السعر $:";

            this.numNewPrice.BackColor     = System.Drawing.Color.FromArgb(50, 50, 50);
            this.numNewPrice.DecimalPlaces = 2;
            this.numNewPrice.Font          = new System.Drawing.Font("Segoe UI", 12F);
            this.numNewPrice.ForeColor     = System.Drawing.Color.White;
            this.numNewPrice.Location      = new System.Drawing.Point(30, 144);
            this.numNewPrice.Maximum       = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numNewPrice.Minimum       = new decimal(new int[] { 0, 0, 0, 0 });
            this.numNewPrice.Name          = "numNewPrice";
            this.numNewPrice.Size          = new System.Drawing.Size(360, 29);
            this.numNewPrice.TabIndex      = 4;
            this.numNewPrice.Value         = new decimal(new int[] { 10, 0, 0, 0 });

            // Stock Qty
            this.lblNewStock.AutoSize  = true;
            this.lblNewStock.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNewStock.ForeColor = System.Drawing.Color.LightGray;
            this.lblNewStock.Location  = new System.Drawing.Point(300, 184);
            this.lblNewStock.Name      = "lblNewStock";
            this.lblNewStock.TabIndex  = 5;
            this.lblNewStock.Text      = "الكمية (Stock):";

            this.numNewStock.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.numNewStock.Font      = new System.Drawing.Font("Segoe UI", 12F);
            this.numNewStock.ForeColor = System.Drawing.Color.White;
            this.numNewStock.Location  = new System.Drawing.Point(30, 206);
            this.numNewStock.Maximum   = new decimal(new int[] { 9999, 0, 0, 0 });
            this.numNewStock.Minimum   = new decimal(new int[] { 0, 0, 0, 0 });
            this.numNewStock.Name      = "numNewStock";
            this.numNewStock.Size      = new System.Drawing.Size(360, 29);
            this.numNewStock.TabIndex  = 6;
            this.numNewStock.Value     = new decimal(new int[] { 1, 0, 0, 0 });

            // Expiry Date
            this.lblNewExpiry.AutoSize  = true;
            this.lblNewExpiry.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNewExpiry.ForeColor = System.Drawing.Color.LightGray;
            this.lblNewExpiry.Location  = new System.Drawing.Point(300, 246);
            this.lblNewExpiry.Name      = "lblNewExpiry";
            this.lblNewExpiry.TabIndex  = 7;
            this.lblNewExpiry.Text      = "تاريخ الانتهاء:";

            this.dtpNewExpiry.CalendarForeColor    = System.Drawing.Color.White;
            this.dtpNewExpiry.CalendarMonthBackground = System.Drawing.Color.FromArgb(40, 40, 40);
            this.dtpNewExpiry.Font      = new System.Drawing.Font("Segoe UI", 12F);
            this.dtpNewExpiry.Format    = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNewExpiry.Location  = new System.Drawing.Point(30, 268);
            this.dtpNewExpiry.Name      = "dtpNewExpiry";
            this.dtpNewExpiry.Size      = new System.Drawing.Size(360, 29);
            this.dtpNewExpiry.TabIndex  = 8;
            this.dtpNewExpiry.Value     = System.DateTime.Now.AddMonths(6);

            // Category
            this.lblNewCategory.AutoSize  = true;
            this.lblNewCategory.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNewCategory.ForeColor = System.Drawing.Color.LightGray;
            this.lblNewCategory.Location  = new System.Drawing.Point(300, 308);
            this.lblNewCategory.Name      = "lblNewCategory";
            this.lblNewCategory.TabIndex  = 9;
            this.lblNewCategory.Text      = "التصنيف:";

            this.cmbNewCategory.BackColor     = System.Drawing.Color.FromArgb(50, 50, 50);
            this.cmbNewCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewCategory.FlatStyle     = System.Windows.Forms.FlatStyle.Flat;
            this.cmbNewCategory.Font          = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbNewCategory.ForeColor     = System.Drawing.Color.White;
            this.cmbNewCategory.Items.AddRange(new object[] { "بروتين","كرياتين","فيتامينات","مشروبات طاقة","معدات","أخرى" });
            this.cmbNewCategory.Location      = new System.Drawing.Point(30, 330);
            this.cmbNewCategory.Name          = "cmbNewCategory";
            this.cmbNewCategory.RightToLeft   = System.Windows.Forms.RightToLeft.Yes;
            this.cmbNewCategory.Size          = new System.Drawing.Size(360, 29);
            this.cmbNewCategory.TabIndex      = 10;

            // Image
            this.lblNewImage.AutoSize  = true;
            this.lblNewImage.Font      = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNewImage.ForeColor = System.Drawing.Color.LightGray;
            this.lblNewImage.Location  = new System.Drawing.Point(300, 370);
            this.lblNewImage.Name      = "lblNewImage";
            this.lblNewImage.TabIndex  = 13;
            this.lblNewImage.Text      = "صورة المنتج:";

            this.picNewImage.BackColor   = System.Drawing.Color.FromArgb(50, 50, 50);
            this.picNewImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picNewImage.Location    = new System.Drawing.Point(130, 390);
            this.picNewImage.Name        = "picNewImage";
            this.picNewImage.Size        = new System.Drawing.Size(100, 100);
            this.picNewImage.SizeMode    = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picNewImage.TabIndex    = 14;

            this.btnBrowseImage.BackColor  = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnBrowseImage.FlatStyle  = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseImage.ForeColor  = System.Drawing.Color.White;
            this.btnBrowseImage.Location   = new System.Drawing.Point(30, 420);
            this.btnBrowseImage.Name       = "btnBrowseImage";
            this.btnBrowseImage.Size       = new System.Drawing.Size(80, 30);
            this.btnBrowseImage.TabIndex   = 15;
            this.btnBrowseImage.Text       = "استعراض";
            this.btnBrowseImage.UseVisualStyleBackColor = false;

            // Buttons
            this.btnConfirmAdd.BackColor = System.Drawing.Color.FromArgb(0, 166, 62);
            this.btnConfirmAdd.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmAdd.FlatAppearance.BorderSize = 0;
            this.btnConfirmAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmAdd.Font      = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnConfirmAdd.ForeColor = System.Drawing.Color.White;
            this.btnConfirmAdd.Location  = new System.Drawing.Point(215, 500);
            this.btnConfirmAdd.Name      = "btnConfirmAdd";
            this.btnConfirmAdd.Size      = new System.Drawing.Size(175, 40);
            this.btnConfirmAdd.TabIndex  = 11;
            this.btnConfirmAdd.Text      = "✓  إضافة";
            this.btnConfirmAdd.UseVisualStyleBackColor = false;

            this.btnCancelAdd.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnCancelAdd.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnCancelAdd.FlatAppearance.BorderSize = 0;
            this.btnCancelAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelAdd.Font      = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCancelAdd.ForeColor = System.Drawing.Color.LightGray;
            this.btnCancelAdd.Location  = new System.Drawing.Point(30, 500);
            this.btnCancelAdd.Name      = "btnCancelAdd";
            this.btnCancelAdd.Size      = new System.Drawing.Size(175, 40);
            this.btnCancelAdd.TabIndex  = 12;
            this.btnCancelAdd.Text      = "إلغاء";
            this.btnCancelAdd.UseVisualStyleBackColor = false;

            // StoreForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.FromArgb(20, 20, 20);
            this.ClientSize          = new System.Drawing.Size(950, 660);
            this.Controls.Add(this.pnlAddItem);
            this.Controls.Add(this.pnlProducts);
            this.Controls.Add(this.pnlCart);
            this.Name        = "StoreForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text        = "متجر المكملات الغذائية";
            this.Load       += new System.EventHandler(this.StoreForm_Load);

            this.pnlProducts.ResumeLayout(false);
            this.pnlCart.ResumeLayout(false);
            this.pnlCartItems.ResumeLayout(false);
            this.pnlCartFooter.ResumeLayout(false);
            this.pnlAddItem.ResumeLayout(false);
            this.pnlAddItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewImage)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
