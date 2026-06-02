using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace gym_mangment_system
{
    /// <summary>
    /// Full-page notifications view matching the Figma "الإشعارات" screen:
    /// three summary counter cards + a list (or a friendly empty state).
    /// </summary>
    public partial class NotificationsForm : Form, IThemeAware
    {
        private sealed class NotifItem
        {
            public string Icon;
            public string Title;
            public string Body;
            public Color Accent;
        }

        private Label _lblTitle;
        private FlowLayoutPanel _counters;
        private Panel _body;

        public NotificationsForm()
        {
            InitializeComponent();
            RightToLeft = RightToLeft.Yes;
            BuildLayout();
            ApplyTheme(ThemeManager.Current);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 660);
            Name = "NotificationsForm";
            Text = "الإشعارات";
            ResumeLayout(false);
        }

        private static List<NotifItem> BuildItems()
        {
            var items = new List<NotifItem>();
            foreach (var p in GymDataStore.Data.StoreProducts)
            {
                if (p.StockQty > 5) continue;
                items.Add(new NotifItem { Icon = "⚠️", Title = "مخزون منخفض", Body = p.Name + " — المتبقي " + p.StockQty, Accent = FigmaPalette.Orange });
            }

            int soon = GymDataStore.MembersExpiringWithinDays(14);
            if (soon > 0)
                items.Add(new NotifItem { Icon = "🔴", Title = "اشتراكات قريبة الانتهاء", Body = soon + " عضو خلال أسبوعين", Accent = FigmaPalette.Red });

            var sales = new List<StoreSaleRecord>(GymDataStore.Data.StoreSales);
            sales.Sort((a, b) => string.CompareOrdinal(b.SoldAt, a.SoldAt));
            int take = Math.Min(5, sales.Count);
            for (int i = 0; i < take; i++)
            {
                var s = sales[i];
                items.Add(new NotifItem { Icon = "💰", Title = "بيع متجر", Body = (s.Summary ?? "").Trim(), Accent = FigmaPalette.GreenBtn });
            }

            return items;
        }

        private void BuildLayout()
        {
            _lblTitle = new Label
            {
                Dock      = DockStyle.Top,
                Height    = 56,
                Font      = new Font("Segoe UI", 20F, FontStyle.Bold),
                Padding   = new Padding(20, 10, 20, 0),
                TextAlign = ContentAlignment.MiddleRight,
                Text      = "🔔  الإشعارات"
            };

            _counters = new FlowLayoutPanel
            {
                Dock          = DockStyle.Top,
                Height        = 130,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents  = false,
                Padding       = new Padding(18, 6, 18, 6)
            };

            _body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(18) };

            Controls.Add(_body);
            Controls.Add(_counters);
            Controls.Add(_lblTitle);
        }

        private Guna2Panel CounterCard(string title, int value, Color accent, UiColorScheme s)
        {
            Guna2Panel card = GunaUi.Card(280, 104, s.Card, s.BorderSubtle);
            Label lblValue = new Label
            {
                Text = value.ToString(), Font = new Font("Segoe UI", 26F, FontStyle.Bold), ForeColor = accent,
                Location = new Point(18, 14), Size = new Size(244, 44), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent
            };
            Label lblTitle = new Label
            {
                Text = title, Font = new Font("Segoe UI", 11F), ForeColor = s.TextMuted,
                Location = new Point(18, 62), Size = new Size(244, 24), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent
            };
            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            return card;
        }

        private void RebuildBody(List<NotifItem> items, UiColorScheme s)
        {
            foreach (Control c in _body.Controls) c.Dispose();
            _body.Controls.Clear();

            if (items.Count == 0)
            {
                var empty = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
                Label icon = new Label { Text = "✅", Font = new Font("Segoe UI", 40F), Dock = DockStyle.Top, Height = 80, TextAlign = ContentAlignment.MiddleCenter, ForeColor = FigmaPalette.GreenBtn };
                Label l1 = new Label { Text = "لا توجد إشعارات جديدة", Font = new Font("Segoe UI", 16F, FontStyle.Bold), Dock = DockStyle.Top, Height = 36, TextAlign = ContentAlignment.MiddleCenter, ForeColor = s.TextPrimary };
                Label l2 = new Label { Text = "جميع الأمور على ما يرام!", Font = new Font("Segoe UI", 11F), Dock = DockStyle.Top, Height = 28, TextAlign = ContentAlignment.MiddleCenter, ForeColor = s.TextMuted };
                empty.Controls.Add(l2);
                empty.Controls.Add(l1);
                empty.Controls.Add(icon);
                _body.Controls.Add(empty);
                return;
            }

            var flow = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents  = false,
                AutoScroll    = true
            };
            foreach (var it in items)
                flow.Controls.Add(BuildItemCard(it, s));
            _body.Controls.Add(flow);
        }

        private Guna2Panel BuildItemCard(NotifItem it, UiColorScheme s)
        {
            Guna2Panel card = GunaUi.Card(880, 64, s.Card, s.BorderSubtle);
            card.Margin = new Padding(4);
            Panel strip = new Panel { Size = new Size(5, 64), BackColor = it.Accent, Dock = DockStyle.Right };
            Label icon = new Label { Text = it.Icon, Font = new Font("Segoe UI", 16F), Location = new Point(820, 6), Size = new Size(44, 50), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            Label title = new Label { Text = it.Title, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = s.TextPrimary, Location = new Point(20, 10), Size = new Size(790, 22), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            Label body = new Label { Text = it.Body, Font = new Font("Segoe UI", 9.5F), ForeColor = s.TextMuted, Location = new Point(20, 34), Size = new Size(790, 20), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.Transparent };
            card.Controls.Add(strip); card.Controls.Add(icon); card.Controls.Add(title); card.Controls.Add(body);
            return card;
        }

        public void ApplyTheme(UiColorScheme s)
        {
            BackColor = s.ContentHost;
            _lblTitle.ForeColor = s.TextPrimary;
            _counters.BackColor = s.ContentHost;
            _body.BackColor = s.ContentHost;

            int lowStock = 0;
            foreach (var x in GymDataStore.Data.StoreProducts)
                if (x.StockQty <= 5) lowStock++;
            int expiring = GymDataStore.MembersExpiringWithinDays(14);
            int positive = GymDataStore.Data.StoreSales.Count;

            foreach (Control c in _counters.Controls) c.Dispose();
            _counters.Controls.Clear();
            _counters.Controls.Add(CounterCard("تحذيرات هامة", lowStock, FigmaPalette.Orange, s));
            _counters.Controls.Add(CounterCard("تنبيهات", expiring, FigmaPalette.Red, s));
            _counters.Controls.Add(CounterCard("معلومات إيجابية", positive, FigmaPalette.GreenBtn, s));

            RebuildBody(BuildItems(), s);
        }
    }
}
