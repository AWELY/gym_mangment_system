using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gym_mangment_system
{
    public partial class StoreForm : Form, IThemeAware
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
            public int Id           { get; set; }
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
        private readonly List<Product>  _products = new List<Product>();
        private readonly bool _startAddMode;

        public StoreForm()
            : this(false) { }

        public StoreForm(bool startAddMode)
        {
            _startAddMode = startAddMode;
            InitializeComponent();
            ApplyBackgroundBranding();
            LoadProductsFromStore();
            WireEvents();
            ApplyTheme(ThemeManager.Current);
            FormClosed += StoreForm_FormClosed;

            if (_startAddMode)
                BtnAddNewItem_Click(this, EventArgs.Empty);
        }

        private void LoadProductsFromStore()
        {
            foreach (var p in _products)
                p.Photo?.Dispose();
            _products.Clear();
            foreach (var r in GymDataStore.Data.StoreProducts.OrderBy(x => x.Id))
                _products.Add(ProductFromRecord(r));
        }

        private static Product ProductFromRecord(StoreProductRecord r)
        {
            Image img = null;
            if (!string.IsNullOrEmpty(r.PhotoBase64))
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(r.PhotoBase64);
                    using (var ms = new MemoryStream(bytes))
                        img = Image.FromStream(ms);
                }
                catch { /* ignore bad image */ }
            }

            DateTime exp = DateTime.TryParse(r.Expiry, out var e) ? e : DateTime.Now.AddMonths(6);
            return new Product
            {
                Id       = r.Id,
                Name     = r.Name ?? "",
                Price    = r.Price,
                Category = r.Category ?? "",
                Emoji    = string.IsNullOrEmpty(r.Emoji) ? "📦" : r.Emoji,
                StockQty = r.StockQty,
                Expiry   = exp,
                Photo    = img
            };
        }

        private static StoreProductRecord ToRecord(Product p)
        {
            string b64 = null;
            if (p.Photo != null)
            {
                using (var ms = new MemoryStream())
                {
                    p.Photo.Save(ms, ImageFormat.Png);
                    b64 = Convert.ToBase64String(ms.ToArray());
                }
            }

            return new StoreProductRecord
            {
                Id          = p.Id,
                Name        = p.Name,
                Price       = p.Price,
                Category    = p.Category,
                Emoji       = p.Emoji,
                StockQty    = p.StockQty,
                Expiry      = p.Expiry.ToString("yyyy-MM-dd"),
                PhotoBase64 = b64
            };
        }

        private void PersistAllProducts()
        {
            GymDataStore.Data.StoreProducts.Clear();
            foreach (var p in _products.OrderBy(x => x.Id))
                GymDataStore.Data.StoreProducts.Add(ToRecord(p));
            GymDataStore.Save();
        }

        private void StoreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PersistAllProducts();
            foreach (var p in _products)
                p.Photo?.Dispose();
            _products.Clear();
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            pnlProducts.BackColor = s.Panel;
            flowProducts.BackColor = s.Panel;
            lblProductsTitle.ForeColor = s.TextPrimary;

            pnlCart.BackColor = s.PanelElevated;
            pnlCartItems.BackColor = s.PanelElevated;
            flowCartItems.BackColor = s.PanelElevated;
            pnlCartFooter.BackColor = s.PanelElevated;
            lblCartTitle.ForeColor = s.TextPrimary;
            lblCartCount.ForeColor = s.TextMuted;
            lblTotalLabel.ForeColor = s.TextMuted;

            btnClearCart.FillColor = s.SecondaryButton;
            btnClearCart.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.LightGray;
            btnCheckout.FillColor = FigmaPalette.GreenBtn;
            GunaUi.ApplyBrandGradient(btnAddNewItem);

            pnlAddItem.FillColor = s.Card;
            pnlAddItem.BorderColor = s.BorderSubtle;
            lblAddItemTitle.ForeColor = s.TextPrimary;
            foreach (Label lb in new[] { lblNewName, lblNewPrice, lblNewStock, lblNewExpiry, lblNewCategory, lblNewImage })
            {
                lb.ForeColor = s.TextMuted;
            }
            txtNewName.BackColor = s.InputBackground;
            txtNewName.ForeColor = s.InputForeground;
            numNewPrice.BackColor = s.InputBackground;
            numNewPrice.ForeColor = s.InputForeground;
            numNewStock.BackColor = s.InputBackground;
            numNewStock.ForeColor = s.InputForeground;
            cmbNewCategory.BackColor = s.InputBackground;
            cmbNewCategory.ForeColor = s.InputForeground;
            dtpNewExpiry.CalendarMonthBackground = s.InputBackground;
            dtpNewExpiry.CalendarForeColor = s.InputForeground;
            picNewImage.BackColor = s.InputBackground;
            btnBrowseImage.FillColor = s.SecondaryButton;
            btnBrowseImage.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.White;
            btnConfirmAdd.FillColor = FigmaPalette.GreenBtn;
            btnCancelAdd.FillColor = s.SecondaryButton;
            btnCancelAdd.ForeColor = ThemeManager.IsLight ? s.TextPrimary : Color.LightGray;

            PopulateProducts();
            RefreshCart();
        }

        private void ApplyBackgroundBranding()
        {
            float op = ThemeManager.BrandingOpacity();
            Image faded = ImageAssets.TryLoadToughBackground("store", op);
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
            UiColorScheme s = ThemeManager.Current;
            bool lowStock  = product.StockQty <= 5;
            bool nearExpiry = (product.Expiry - DateTime.Now).TotalDays <= 30;

            Panel card = new Panel
            {
                Size      = new Size(170, 175),
                BackColor = s.PanelElevated,
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
            Label name  = new Label { Text = product.Name,  Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = s.TextPrimary, Size = new Size(170, 20), Location = new Point(0, 56), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            Label price = new Label { Text = product.Price.ToString("0.00") + " ريال", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = FigmaPalette.Primary, Size = new Size(170, 24), Location = new Point(0, 78), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };

            Color stockColor = lowStock ? Color.FromArgb(231, 0, 11) : Color.FromArgb(0, 166, 62);
            Label stock = new Label { Text = "كمية: " + product.StockQty, Font = new Font("Segoe UI", 8F), ForeColor = stockColor, Size = new Size(170, 18), Location = new Point(0, 104), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };

            Color expiryColor = nearExpiry ? Color.FromArgb(255, 105, 0) : s.TextMuted;
            Label expiry = new Label { Text = "ينتهي: " + product.Expiry.ToString("yyyy-MM-dd"), Font = new Font("Segoe UI", 7.5F), ForeColor = expiryColor, Size = new Size(170, 16), Location = new Point(0, 124), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };

            Label cat = new Label { Text = product.Category, Font = new Font("Segoe UI", 7.5F), ForeColor = s.TextMuted, Size = new Size(170, 14), Location = new Point(0, 142), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };

            Color hover = s.CardHover;
            Color normal = s.PanelElevated;
            card.MouseEnter += (ev, e) => card.BackColor = hover;
            card.MouseLeave += (ev, e) => card.BackColor = normal;

            EventHandler click = (ev, e) => AddToCart(product);
            card.Click += click;
            foreach (Control c in new Control[] { visual, name, price, stock, expiry, cat })
            {
                c.Click += click;
                c.MouseEnter += (ev, e) => card.BackColor = hover;
                c.MouseLeave += (ev, e) => card.BackColor = normal;
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
        private static void ShowStockExceededWarning(string productName, int requestedQty, int availableQty)
        {
            MessageBox.Show(
                "الكمية المطلوبة أكبر من المتوفر في المخزون.\n\n" +
                "المنتج: " + productName + "\n" +
                "المطلوب: " + requestedQty + "\n" +
                "المتوفر: " + availableQty,
                "تنبيه المخزون", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void AddToCart(Product product)
        {
            if (product.StockQty <= 0)
            {
                MessageBox.Show("هذا المنتج غير متوفر في المخزون", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var existing = _cart.Find(c => c.Name == product.Name);
            if (existing != null)
            {
                int nextQty = existing.Qty + 1;
                if (nextQty > product.StockQty)
                {
                    ShowStockExceededWarning(product.Name, nextQty, product.StockQty);
                    return;
                }
                existing.Qty++;
            }
            else
                _cart.Add(new CartItem { Name = product.Name, Price = product.Price, Qty = 1 });
            RefreshCart();
        }

        private void RefreshCart()
        {
            UiColorScheme s = ThemeManager.Current;
            flowCartItems.Controls.Clear();
            decimal total = 0; int totalItems = 0;

            foreach (var item in _cart)
            {
                decimal line = item.Price * item.Qty;
                total += line; totalItems += item.Qty;

                Panel row = new Panel { Size = new Size(320, 55), BackColor = s.Panel, Margin = new Padding(2, 3, 2, 3) };

                Label lblN = new Label { Text = item.Name, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = s.TextPrimary, Location = new Point(165, 5), Size = new Size(150, 22), TextAlign = ContentAlignment.MiddleRight };
                Label lblD = new Label { Text = item.Qty + " × " + item.Price.ToString("0.00") + " ريال = " + line.ToString("0.00") + " ريال", Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(0, 166, 62), Location = new Point(100, 30), Size = new Size(215, 18), TextAlign = ContentAlignment.MiddleRight };

                Button btnRm = new Button { Text = "✕", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(231, 0, 11), BackColor = s.Panel, FlatStyle = FlatStyle.Flat, Size = new Size(30, 30), Location = new Point(8, 12), Cursor = Cursors.Hand, Tag = item };
                btnRm.FlatAppearance.BorderSize = 0;
                btnRm.Click += (sender, e) => { _cart.Remove((CartItem)((Button)sender).Tag); RefreshCart(); };

                Button btnP = new Button { Text = "+", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = s.TextOnAccent, BackColor = s.SecondaryButton, FlatStyle = FlatStyle.Flat, Size = new Size(28, 28), Location = new Point(42, 13), Cursor = Cursors.Hand, Tag = item };
                btnP.FlatAppearance.BorderSize = 0;
                btnP.FlatAppearance.MouseOverBackColor = s.SecondaryButtonHover;
                btnP.Click += (sender, e) =>
                {
                    var ci = (CartItem)((Button)sender).Tag;
                    var pr = _products.FirstOrDefault(p => p.Name == ci.Name);
                    int avail = pr?.StockQty ?? 0;
                    if (ci.Qty >= avail)
                    {
                        ShowStockExceededWarning(ci.Name, ci.Qty + 1, avail);
                        return;
                    }
                    ci.Qty++;
                    RefreshCart();
                };

                Button btnM = new Button { Text = "-", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = s.TextOnAccent, BackColor = s.SecondaryButton, FlatStyle = FlatStyle.Flat, Size = new Size(28, 28), Location = new Point(72, 13), Cursor = Cursors.Hand, Tag = item };
                btnM.FlatAppearance.BorderSize = 0;
                btnM.FlatAppearance.MouseOverBackColor = s.SecondaryButtonHover;
                btnM.Click += (sender, e) => { var ci = (CartItem)((Button)sender).Tag; if (--ci.Qty <= 0) _cart.Remove(ci); RefreshCart(); };

                row.Controls.Add(lblN); row.Controls.Add(lblD);
                row.Controls.Add(btnRm); row.Controls.Add(btnP); row.Controls.Add(btnM);
                flowCartItems.Controls.Add(row);
            }

            lblTotalValue.Text = total.ToString("0.00") + " ريال";
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

            var stockErrors = new List<string>();
            foreach (var line in _cart)
            {
                var pr = _products.FirstOrDefault(p => p.Name == line.Name);
                int avail = pr?.StockQty ?? 0;
                if (line.Qty > avail)
                    stockErrors.Add(line.Name + ": المطلوب " + line.Qty + "، المتوفر " + avail);
            }
            if (stockErrors.Count > 0)
            {
                MessageBox.Show(
                    "لا يمكن إتمام البيع بسبب نقص المخزون:\n\n" + string.Join("\n", stockErrors),
                    "تنبيه المخزون", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var line in _cart)
            {
                var rec = GymDataStore.Data.StoreProducts.FirstOrDefault(x => x.Name == line.Name);
                if (rec != null)
                    rec.StockQty = Math.Max(0, rec.StockQty - line.Qty);
                var pr = _products.FirstOrDefault(x => x.Name == line.Name);
                if (pr != null)
                    pr.StockQty = Math.Max(0, pr.StockQty - line.Qty);
            }

            decimal total = 0;
            foreach (var item in _cart) total += item.Price * item.Qty;
            var sb = new StringBuilder();
            foreach (var item in _cart)
                sb.Append(item.Name).Append("×").Append(item.Qty).Append("؛ ");

            var sale = new StoreSaleRecord
            {
                SoldAt  = DateTime.Now.ToString("o"),
                Total   = total,
                Summary = sb.ToString().Trim()
            };
            foreach (var item in _cart)
                sale.Items.Add(new StoreSaleItemRecord { ProductName = item.Name, Price = item.Price, Qty = item.Qty });
            GymDataStore.Data.StoreSales.Add(sale);
            GymDataStore.Save();

            MessageBox.Show("تم إصدار الفاتورة بنجاح!\n\nالمجموع: " + total.ToString("0.00") + " ريال\nعدد المنتجات: " + _cart.Count,
                "✅ فاتورة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _cart.Clear();
            RefreshCart();
            PopulateProducts();
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

            int newId = GymDataStore.NextProductId();
            var p = new Product
            {
                Id       = newId,
                Name     = txtNewName.Text.Trim(),
                Price    = numNewPrice.Value,
                Category = cat,
                Emoji    = emoji,
                StockQty = (int)numNewStock.Value,
                Expiry   = dtpNewExpiry.Value,
                Photo    = photoCopy
            };

            _products.Add(p);
            GymDataStore.Data.StoreProducts.Add(ToRecord(p));
            GymDataStore.Save();

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
