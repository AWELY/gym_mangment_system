using System;
using System.Collections.Generic;
using System.Linq;

namespace gym_mangment_system
{
    /// <summary>
    /// Single source of truth: in-memory lists only (session / runtime; no file persistence).
    /// </summary>
    public static class GymDataStore
    {
        private static GymDataSnapshot _data;

        public static GymDataSnapshot Data
        {
            get
            {
                if (_data == null)
                    _data = SeedDefaults();
                return _data;
            }
        }

        public static void Initialize()
        {
            if (_data == null)
                _data = SeedDefaults();
        }

        /// <summary>No-op: data is not written to disk. Kept for call sites that used to persist.</summary>
        public static void Save() { }

        private static GymDataSnapshot SeedDefaults()
        {
            var s = GymDataSnapshot.CreateEmpty();

            s.SubscriptionPlans.AddRange(new[]
            {
                new SubscriptionPlan { Id = 1, Name = "Basic Plan", Price = 30.00m, DurationValue = 1, DurationUnit = "شهر" },
                new SubscriptionPlan { Id = 2, Name = "Pro Plan", Price = 50.00m, DurationValue = 3, DurationUnit = "شهر" },
                new SubscriptionPlan { Id = 3, Name = "Annual Plan", Price = 300.00m, DurationValue = 1, DurationUnit = "سنة" }
            });

            s.Users.Add(new UserDirectoryEntry { Id = 1, FullName = "المدير العام", Username = "admin", Password = "admin", Role = AppSession.UserRole.Admin });
            s.Users.Add(new UserDirectoryEntry { Id = 2, FullName = "مستلم النظام", Username = "reception", Password = "1234", Role = AppSession.UserRole.Receptionist });

            s.Members.AddRange(new[]
            {
                new MemberRecord { Id = 1, FullName = "أحمد محمد", Phone = "0501234567", Gender = "ذكر", PlanName = "Basic Plan", PriceText = "30 ريال", DurationText = "1 شهر", JoinDate = "2026-01-15" },
                new MemberRecord { Id = 2, FullName = "سارة علي", Phone = "0559876543", Gender = "أنثى", PlanName = "Pro Plan", PriceText = "50 ريال", DurationText = "3 شهر", JoinDate = "2025-06-20" },
                new MemberRecord { Id = 3, FullName = "خالد إبراهيم", Phone = "0561112233", Gender = "ذكر", PlanName = "Annual Plan", PriceText = "300 ريال", DurationText = "1 سنة", JoinDate = "2025-11-01" },
                new MemberRecord { Id = 4, FullName = "نورة حسن", Phone = "0547778899", Gender = "أنثى", PlanName = "Basic Plan", PriceText = "30 ريال", DurationText = "1 شهر", JoinDate = "2026-02-10" },
                new MemberRecord { Id = 5, FullName = "عمر فاروق", Phone = "0533334455", Gender = "ذكر", PlanName = "Pro Plan", PriceText = "50 ريال", DurationText = "3 شهر", JoinDate = "2025-09-05" },
                new MemberRecord { Id = 6, FullName = "ليلى أحمد", Phone = "0522225566", Gender = "أنثى", PlanName = "Basic Plan", PriceText = "30 ريال", DurationText = "1 شهر", JoinDate = "2026-03-01" },
                new MemberRecord { Id = 7, FullName = "يوسف كمال", Phone = "0511116677", Gender = "ذكر", PlanName = "Annual Plan", PriceText = "300 ريال", DurationText = "1 سنة", JoinDate = "2025-08-15" },
                new MemberRecord { Id = 8, FullName = "فاطمة سعيد", Phone = "0588889900", Gender = "أنثى", PlanName = "Basic Plan", PriceText = "30 ريال", DurationText = "1 شهر", JoinDate = "2026-04-01" },
                new MemberRecord { Id = 9, FullName = "محمود عادل", Phone = "0577771122", Gender = "ذكر", PlanName = "Pro Plan", PriceText = "50 ريال", DurationText = "3 شهر", JoinDate = "2025-12-20" },
                new MemberRecord { Id = 10, FullName = "هند محمود", Phone = "0566662233", Gender = "أنثى", PlanName = "Annual Plan", PriceText = "300 ريال", DurationText = "1 سنة", JoinDate = "2026-01-05" }
            });

            s.Trainers.AddRange(new[]
            {
                new TrainerRecord { Id = 1, Name = "محمد السالم", Phone = "0501111222", Specialty = "رفع أثقال", Salary = 2500m, JoinDate = "2023-01-10" },
                new TrainerRecord { Id = 2, Name = "أنس العتيبي", Phone = "0522223333", Specialty = "كروس فيت", Salary = 2200m, JoinDate = "2023-03-15" },
                new TrainerRecord { Id = 3, Name = "ريم الزهراني", Phone = "0533334444", Specialty = "يوغا ولياقة", Salary = 2000m, JoinDate = "2023-06-01" },
                new TrainerRecord { Id = 4, Name = "فيصل الحربي", Phone = "0544445555", Specialty = "ملاكمة", Salary = 2800m, JoinDate = "2022-11-20" },
                new TrainerRecord { Id = 5, Name = "نورا الشمري", Phone = "0555556666", Specialty = "تمارين نسائية", Salary = 1900m, JoinDate = "2024-01-05" }
            });

            int pid = 1;
            void AddP(string name, decimal price, string cat, string emoji, int stock, string expiryIso)
            {
                s.StoreProducts.Add(new StoreProductRecord
                {
                    Id = pid++, Name = name, Price = price, Category = cat, Emoji = emoji,
                    StockQty = stock, Expiry = expiryIso, PhotoBase64 = null
                });
            }

            AddP("واي بروتين", 50m, "بروتين", "💪", 20, DateTime.Now.AddMonths(8).ToString("yyyy-MM-dd"));
            AddP("كرياتين مونو", 25m, "كرياتين", "⚡", 15, DateTime.Now.AddMonths(12).ToString("yyyy-MM-dd"));
            AddP("BCAA أحماض", 30m, "بروتين", "🧬", 10, DateTime.Now.AddMonths(6).ToString("yyyy-MM-dd"));
            AddP("فيتامين D3", 12m, "فيتامينات", "☀️", 30, DateTime.Now.AddMonths(18).ToString("yyyy-MM-dd"));
            AddP("أوميغا 3", 18m, "فيتامينات", "🐟", 25, DateTime.Now.AddMonths(10).ToString("yyyy-MM-dd"));
            AddP("مشروب طاقة", 5m, "مشروبات طاقة", "🥤", 50, DateTime.Now.AddMonths(4).ToString("yyyy-MM-dd"));
            AddP("حزام رفع أثقال", 35m, "معدات", "🏋️", 8, DateTime.Now.AddYears(3).ToString("yyyy-MM-dd"));
            AddP("قفازات تمرين", 15m, "معدات", "🧤", 12, DateTime.Now.AddYears(3).ToString("yyyy-MM-dd"));
            AddP("شيكر بروتين", 8m, "معدات", "🥤", 18, DateTime.Now.AddYears(2).ToString("yyyy-MM-dd"));
            AddP("بروتين بار", 3.5m, "بروتين", "🍫", 40, DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd"));
            AddP("جلوتامين", 22m, "بروتين", "💊", 14, DateTime.Now.AddMonths(9).ToString("yyyy-MM-dd"));
            AddP("ZMA مكمل", 16m, "فيتامينات", "💤", 11, DateTime.Now.AddMonths(15).ToString("yyyy-MM-dd"));

            s.FeedingPlans.AddRange(new[]
            {
                new FeedingPlanRecord { Name = "خطة التنشيف", PdfPath = @"C:\Plans\cutting_plan.pdf" },
                new FeedingPlanRecord { Name = "خطة التضخم", PdfPath = @"C:\Plans\bulking_plan.pdf" },
                new FeedingPlanRecord { Name = "خطة الحفاظ", PdfPath = @"C:\Plans\maintenance_plan.pdf" },
                new FeedingPlanRecord { Name = "خطة نباتية", PdfPath = @"C:\Plans\vegan_plan.pdf" },
                new FeedingPlanRecord { Name = "خطة كيتو", PdfPath = @"C:\Plans\keto_plan.pdf" }
            });

            return s;
        }

        public static int NextMemberId() => Data.Members.Count == 0 ? 1 : Data.Members.Max(m => m.Id) + 1;
        public static int NextTrainerId() => Data.Trainers.Count == 0 ? 1 : Data.Trainers.Max(m => m.Id) + 1;
        public static int NextProductId() => Data.StoreProducts.Count == 0 ? 1 : Data.StoreProducts.Max(m => m.Id) + 1;

        public static decimal ParsePriceFromMemberDisplay(string priceText)
        {
            if (string.IsNullOrWhiteSpace(priceText)) return 0;
            string d = new string(priceText.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());
            d = d.Replace(',', '.');
            return decimal.TryParse(d, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : 0;
        }

        /// <summary>12 elements: Jan..Dec current year store totals.</summary>
        public static decimal[] StoreTotalsByMonthCurrentYear()
        {
            var arr = new decimal[12];
            int y = DateTime.Now.Year;
            foreach (var sale in Data.StoreSales)
            {
                if (DateTime.TryParse(sale.SoldAt, out var dt) && dt.Year == y)
                    arr[dt.Month - 1] += sale.Total;
            }
            return arr;
        }

        /// <summary>Approximate subscription cash-in by member join month (one payment at join).</summary>
        public static decimal[] SubscriptionTotalsByMonthCurrentYear()
        {
            var arr = new decimal[12];
            int y = DateTime.Now.Year;
            foreach (var m in Data.Members)
            {
                if (!DateTime.TryParse(m.JoinDate, out var jd) || jd.Year != y) continue;
                arr[jd.Month - 1] += ParsePriceFromMemberDisplay(m.PriceText);
            }
            return arr;
        }

        public static int MembersExpiringWithinDays(int days)
        {
            var today     = DateTime.Today;
            var endWindow = today.AddDays(days);
            int n = 0;
            foreach (var m in Data.Members)
            {
                var plan = Data.SubscriptionPlans.FirstOrDefault(p => p.Name == m.PlanName);
                if (plan == null) continue;
                if (!DateTime.TryParse(m.JoinDate, out var start)) continue;
                DateTime end;
                try
                {
                    end = plan.DurationUnit != null && plan.DurationUnit.IndexOf("سنة", StringComparison.Ordinal) >= 0
                        ? start.AddYears(Math.Max(1, plan.DurationValue))
                        : start.AddMonths(Math.Max(1, plan.DurationValue));
                }
                catch { continue; }

                if (end >= today && end <= endWindow)
                    n++;
            }
            return n;
        }

        public static decimal StoreRevenueThisMonth()
        {
            int mo = DateTime.Now.Month, y = DateTime.Now.Year;
            return Data.StoreSales
                .Where(s => DateTime.TryParse(s.SoldAt, out var d) && d.Year == y && d.Month == mo)
                .Sum(s => s.Total);
        }

        public static decimal SubscriptionCashInThisMonth()
        {
            int mo = DateTime.Now.Month, y = DateTime.Now.Year;
            return Data.Members
                .Where(m => DateTime.TryParse(m.JoinDate, out var jd) && jd.Year == y && jd.Month == mo)
                .Sum(m => ParsePriceFromMemberDisplay(m.PriceText));
        }

        public static decimal StoreSalesToday()
        {
            var t = DateTime.Today;
            return Data.StoreSales
                .Where(s => DateTime.TryParse(s.SoldAt, out var d) && d.Date == t)
                .Sum(s => s.Total);
        }
    }
}
