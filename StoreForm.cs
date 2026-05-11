using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class StoreForm : Form
    {
        // Cart model
        private class CartItem
        {
            public string Name  { get; set; }
            public decimal Price { get; set; }
            public int Qty      { get; set; }
        }

        // Product model
        private class Product
        {
            public string Name      { get; set; }
            public decimal Price    { get; set; }
            public string Category  { get; set; }
            public string Emoji     { get; set; }
            public int StockQty     { get; set; }
            public DateTime Expiry  { get; set; }
            /// <summary>Optional product photo; owned by this product (dispose when product removed).</summary>
            public Image Photo      { get; set; }
        }

        private readonly List<CartItem> _cart = new List<CartItem>();
        private readonly List<Product>  _products = new List<Product>
        {
            new Product { Name="واي بروتين",      Price=50.00m, Category="بروتين",      Emoji="💪", StockQty=20, Expiry=DateTime.Now.AddMonths(8) },
            new Product { Name="كرياتين مونو",    Price=25.00m, Category="كرياتين",     Emoji="⚡", StockQty=15, Expiry=DateTime.Now.AddMonths(12) },
            new Product { Name="BCAA أحماض",      Price=30.00m, Category="بروتين",      Emoji="🧬", StockQty=10, Expiry=DateTime.Now.AddMonths(6) },
            new Product { Name="فيتامين D3",      Price=12.00m, Category="فيتامينات",   Emoji="☀️", StockQty=30, Expiry=DateTime.Now.AddMonths(18) },
            new Product { Name="أوميغا 3",        Price=18.00m, Category="فيتامينات",   Emoji="🐟", StockQty=25, Expiry=DateTime.Now.AddMonths(10) },
            new Product { Name="مشروب طاقة",      Price=5.00m,  Category="مشروبات طاقة",Emoji="🥤", StockQty=50, Expiry=DateTime.Now.AddMonths(4) },
            new Product { Name="حزام رفع أثقال", Price=35.00m, Category="معدات",       Emoji="🏋️", StockQty=8,  Expiry=DateTime.Now.AddYears(3) },
            new Product { Name="قفازات تمرين",   Price=15.00m, Category="معدات",       Emoji="🧤", StockQty=12, Expiry=DateTime.Now.AddYears(3) },
            new Product { Name="شيكر بروتين",    Price=8.00m,  Category="معدات",       Emoji="🥤", StockQty=18, Expiry=DateTime.Now.AddYears(2) },
            new Product { Name="بروتين بار",     Price=3.50m,  Category="بروتين",      Emoji="🍫", StockQty=40, Expiry=DateTime.Now.AddMonths(3) },
            new Product { Name="جلوتامين",       Price=22.00m, Category="بروتين",      Emoji="💊", StockQty=14, Expiry=DateTime.Now.AddMonths(9) },
            new Product { Name="ZMA مكمل",       Price=16.00m, Category="فيتامينات",   Emoji="💤", StockQty=11, Expiry=DateTime.Now.AddMonths(15) },
        };

        public StoreForm()
        {
            InitializeComponent();
            ApplyBackgroundBranding();
            PopulateProducts();
            WireEvents();
            FormClosed += StoreForm_FormClosed;
        }

        private void StoreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var p in _products)
                p.Photo?.Dispose();
        }

        private void ApplyBackgroundBranding()
        {
            Image faded = ImageAssets.TryLoadToughBackground("store", 0.22f);
            if (faded == null) return;
            this.BackgroundImage = faded;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            pnlProducts.BackgroundImage = faded;
            pnlProducts.BackgroundImageLayout = ImageLayout.Stretch;
            pnlCart.BackgroundImage = faded;
            pnlCart.BackgroundImageLayout = ImageLayout.Stretch;
        }

        // ═══════════════════════════════════════════
        //  PRODUCT GRID
        // ═══════════════════════════════════════════
        private void PopulateProducts()
        {
            flowProducts.Controls.Clear();
            foreach (var p in _products)
                flowProducts.Controls.Add(CreateProductCard(p));
        }

        private Panel CreateProductCard(Product product)
        {
            bool lowStock  = product.StockQty <= 5;
            bool nearExpiry = (product.Expiry - DateTime.Now).TotalDays <= 30;

            Panel card = new Panel
            {
                Size      = new Size(170, 175),
                BackColor = Color.FromArgb(35, 35, 35),
                Margin    = new Padding(6),
                Cursor    = Cursors.Hand,
                Tag       = product
            };

            Control visual;
            if (product.Photo != null)
            {
                visual = new PictureBox
                {
                    Size     = new Size(170, 48),
                    Location = new Point(0, 6),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image    = product.Photo,
                    BackColor = Color.Transparent
                };
            }
            else
            {
                visual = new Label { Text = product.Emoji, Font = new Font("Segoe UI", 26F), Size = new Size(170, 48), Location = new Point(0, 6), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            }
            Label name  = new Label { Text = product.Name,  Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.White, Size = new Size(170, 20), Location = new Point(0, 56), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            Label price = new Label { Text = product.Price.ToString("0.00") + " $", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(76, 175, 80), Size = new Size(170, 24), Location = new Point(0, 78), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };

            // Stock badge
            Color stockColor = lowStock ? Color.FromArgb(220, 53, 69) : Color.FromArgb(76, 175, 80);
            Label stock = new Label { Text = "كمية: " + product.StockQty, Font = new Font("Segoe UI", 8F), ForeColor = stockColor, Size = new Size(170, 18), Location = new Point(0, 104), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };

            // Expiry badge
            Color expiryColor = nearExpiry ? Color.FromArgb(255, 152, 0) : Color.FromArgb(120, 120, 130);
            Label expiry = new Label { Text = "ينتهي: " + product.Expiry.ToString("yyyy-MM-dd"), Font = new Font("Segoe UI", 7.5F), ForeColor = expiryColor, Size = new Size(170, 16), Location = new Point(0, 124), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };

            Label cat = new Label { Text = product.Category, Font = new Font("Segoe UI", 7.5F), ForeColor = Color.Gray, Size = new Size(170, 14), Location = new Point(0, 142), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(50, 50, 50);
            card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(35, 35, 35);

            EventHandler click = (s, e) => AddToCart(product);
            card.Click += click;
            foreach (Control c in new Control[] { visual, name, price, stock, expiry, cat })
            {
                c.Click += click;
                c.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(50, 50, 50);
                c.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(35, 35, 35);
                c.Cursor = Cursors.Hand;
            }

            card.Controls.Add(visual);
            card.Controls.Add(name);
            card.Controls.Add(price);
            card.Controls.Add(stock);
            card.Controls.Add(expiry);
            card.Controls.Add(cat);
            return card;
        }

        // ═══════════════════════════════════════════
        //  CART LOGIC
        // ═══════════════════════════════════════════
        private void AddToCart(Product product)
        {
            if (product.StockQty <= 0)
            {
                MessageBox.Show("هذا المنتج غير متوفر في المخزون", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var existing = _cart.Find(c => c.Name == product.Name);
            if (existing != null) existing.Qty++;
            else _cart.Add(new CartItem { Name = product.Name, Price = product.Price, Qty = 1 });
            RefreshCart();
        }

        private void RefreshCart()
        {
            flowCartItems.Controls.Clear();
            decimal total = 0; int totalItems = 0;

            foreach (var item in _cart)
            {
                decimal line = item.Price * item.Qty;
                total += line; totalItems += item.Qty;

                Panel row = new Panel { Size = new Size(320, 55), BackColor = Color.FromArgb(40, 40, 40), Margin = new Padding(2, 3, 2, 3) };

                Label lblN = new Label { Text = item.Name, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(165, 5), Size = new Size(150, 22), TextAlign = ContentAlignment.MiddleRight };
                Label lblD = new Label { Text = item.Qty + " × " + item.Price.ToString("0.00") + " $ = " + line.ToString("0.00") + " $", Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(76, 175, 80), Location = new Point(100, 30), Size = new Size(215, 18), TextAlign = ContentAlignment.MiddleRight };

                Button btnRm = new Button { Text = "✕", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(220, 53, 69), BackColor = Color.FromArgb(40, 40, 40), FlatStyle = FlatStyle.Flat, Size = new Size(30, 30), Location = new Point(8, 12), Cursor = Cursors.Hand, Tag = item };
                btnRm.FlatAppearance.BorderSize = 0;
                btnRm.Click += (s, e) => { _cart.Remove((CartItem)((Button)s).Tag); RefreshCart(); };

                Button btnP = new Button { Text = "+", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.FromArgb(55, 55, 55), FlatStyle = FlatStyle.Flat, Size = new Size(28, 28), Location = new Point(42, 13), Cursor = Cursors.Hand, Tag = item };
                btnP.FlatAppearance.BorderSize = 0;
                btnP.Click += (s, e) => { ((CartItem)((Button)s).Tag).Qty++; RefreshCart(); };

                Button btnM = new Button { Text = "-", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.FromArgb(55, 55, 55), FlatStyle = FlatStyle.Flat, Size = new Size(28, 28), Location = new Point(72, 13), Cursor = Cursors.Hand, Tag = item };
                btnM.FlatAppearance.BorderSize = 0;
                btnM.Click += (s, e) => { var ci = (CartItem)((Button)s).Tag; if (--ci.Qty <= 0) _cart.Remove(ci); RefreshCart(); };

                row.Controls.Add(lblN); row.Controls.Add(lblD);
                row.Controls.Add(btnRm); row.Controls.Add(btnP); row.Controls.Add(btnM);
                flowCartItems.Controls.Add(row);
            }

            lblTotalValue.Text = total.ToString("0.00") + " $";
            lblCartCount.Text  = totalItems + " عنصر في السلة";
        }

        // ═══════════════════════════════════════════
        //  EVENTS
        // ═══════════════════════════════════════════
        private void WireEvents()
        {
            btnCheckout.Click   += BtnCheckout_Click;
            btnClearCart.Click  += BtnClearCart_Click;
            btnAddNewItem.Click += BtnAddNewItem_Click;
            btnConfirmAdd.Click += BtnConfirmAdd_Click;
            btnCancelAdd.Click  += BtnCancelAdd_Click;
            btnBrowseImage.Click += BtnBrowseImage_Click;
        }

        private void BtnBrowseImage_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "صور|*.png;*.jpg;*.jpeg;*.gif;*.bmp|كل الملفات|*.*";
                dlg.Title = "اختر صورة المنتج";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                try
                {
                    picNewImage.Image?.Dispose();
                    picNewImage.Image = Image.FromFile(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("تعذر تحميل الصورة:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0) { MessageBox.Show("السلة فارغة!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            decimal total = 0;
            foreach (var item in _cart) total += item.Price * item.Qty;
            MessageBox.Show("تم إصدار الفاتورة بنجاح!\n\nالمجموع: " + total.ToString("0.00") + " $\nعدد المنتجات: " + _cart.Count,
                "✅ فاتورة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _cart.Clear(); RefreshCart();
        }

        private void BtnClearCart_Click(object sender, EventArgs e) { _cart.Clear(); RefreshCart(); }

        private void BtnAddNewItem_Click(object sender, EventArgs e)
        {
            pnlAddItem.Visible = true;
            pnlAddItem.BringToFront();
            txtNewName.Text   = "";
            numNewPrice.Value = 10;
            numNewStock.Value = 1;
            dtpNewExpiry.Value = DateTime.Now.AddMonths(6);
            if (cmbNewCategory.Items.Count > 0) cmbNewCategory.SelectedIndex = 0;
            picNewImage.Image?.Dispose();
            picNewImage.Image = null;
            txtNewName.Focus();
        }

        private void BtnConfirmAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewName.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم المنتج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cat   = cmbNewCategory.SelectedItem?.ToString() ?? "أخرى";
            string emoji = "📦";
            if (cat == "بروتين")         emoji = "💪";
            else if (cat == "كرياتين")   emoji = "⚡";
            else if (cat == "فيتامينات") emoji = "💊";
            else if (cat == "مشروبات طاقة") emoji = "🥤";
            else if (cat == "معدات")     emoji = "🏋️";

            Image photoCopy = null;
            if (picNewImage.Image != null)
                photoCopy = (Image)picNewImage.Image.Clone();

            var p = new Product
            {
                Name     = txtNewName.Text.Trim(),
                Price    = numNewPrice.Value,
                Category = cat,
                Emoji    = emoji,
                StockQty = (int)numNewStock.Value,
                Expiry   = dtpNewExpiry.Value,
                Photo    = photoCopy
            };

            _products.Add(p);
            flowProducts.Controls.Add(CreateProductCard(p));
            pnlAddItem.Visible = false;
            picNewImage.Image?.Dispose();
            picNewImage.Image = null;

            MessageBox.Show("تمت إضافة المنتج: " + p.Name + "\nالكمية: " + p.StockQty + " | ينتهي: " + p.Expiry.ToString("yyyy-MM-dd"),
                "✅ تمت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCancelAdd_Click(object sender, EventArgs e)
        {
            pnlAddItem.Visible = false;
            picNewImage.Image?.Dispose();
            picNewImage.Image = null;
        }

        private void StoreForm_Load(object sender, EventArgs e) { }
    }
}
