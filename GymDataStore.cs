using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace gym_mangment_system
{
    /// <summary>
    /// Single source of truth. The in-memory snapshot is loaded from MSSQL on
    /// <see cref="Initialize"/> and persisted back on every <see cref="Save"/>.
    /// Forms keep reading/mutating the snapshot exactly as before.
    /// </summary>
    public static class GymDataStore
    {
        private static GymDataSnapshot _data;

        public static GymDataSnapshot Data
        {
            get
            {
                if (_data == null)
                    Initialize();
                return _data;
            }
        }

        public static void Initialize()
        {
            if (_data != null)
                return;

            try
            {
                _data = LoadFromDatabase();
            }
            catch (Exception ex)
            {
                _data = GymDataSnapshot.CreateEmpty();
                MessageBox.Show(
                    "تعذر الاتصال بقاعدة البيانات. سيتم تشغيل النظام بدون بيانات.\n\n" +
                    "تأكد من تشغيل SQL Server وتنفيذ سكربت قاعدة البيانات (GymDB.sql)،\n" +
                    "ويمكنك تعديل سلسلة الاتصال من صفحة الإعدادات.\n\n" +
                    "تفاصيل الخطأ:\n" + ex.Message,
                    "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>Persists the whole in-memory snapshot to MSSQL in one transaction.</summary>
        public static void Save()
        {
            if (_data == null)
                return;

            try
            {
                PersistToDatabase(_data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "تعذر حفظ البيانات في قاعدة البيانات.\n\nتفاصيل الخطأ:\n" + ex.Message,
                    "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── load ───────────────────────────────────────────
        private static GymDataSnapshot LoadFromDatabase()
        {
            var s = GymDataSnapshot.CreateEmpty();

            using (var conn = Db.GetOpenConnection())
            {
                using (var cmd = Db.Proc("dbo.usp_Users_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        s.Users.Add(new UserDirectoryEntry
                        {
                            Id = r.GetInt32(0),
                            FullName = AsString(r, 1),
                            Username = AsString(r, 2),
                            Password = AsString(r, 3),
                            Role = (AppSession.UserRole)r.GetInt32(4)
                        });

                using (var cmd = Db.Proc("dbo.usp_SubscriptionPlans_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        s.SubscriptionPlans.Add(new SubscriptionPlan
                        {
                            Id = r.GetInt32(0),
                            Name = AsString(r, 1),
                            Price = r.GetDecimal(2),
                            DurationValue = r.GetInt32(3),
                            DurationUnit = AsString(r, 4),
                            Features = new List<string>()
                        });

                using (var cmd = Db.Proc("dbo.usp_PlanFeatures_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                    {
                        int planId = r.GetInt32(0);
                        SubscriptionPlan plan = null;
                        foreach (var p in s.SubscriptionPlans)
                            if (p.Id == planId) { plan = p; break; }
                        if (plan != null)
                            plan.Features.Add(AsString(r, 2));
                    }

                using (var cmd = Db.Proc("dbo.usp_Members_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        s.Members.Add(new MemberRecord
                        {
                            Id = r.GetInt32(0),
                            FullName = AsString(r, 1),
                            Phone = AsString(r, 2),
                            Gender = AsString(r, 3),
                            PlanName = AsString(r, 4),
                            PriceText = AsString(r, 5),
                            DurationText = AsString(r, 6),
                            JoinDate = AsString(r, 7),
                            PlanId = AsNullableInt(r, 8)
                        });

                using (var cmd = Db.Proc("dbo.usp_Trainers_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        s.Trainers.Add(new TrainerRecord
                        {
                            Id = r.GetInt32(0),
                            Name = AsString(r, 1),
                            Phone = AsString(r, 2),
                            Specialty = AsString(r, 3),
                            Salary = r.GetDecimal(4),
                            JoinDate = AsString(r, 5)
                        });

                using (var cmd = Db.Proc("dbo.usp_StoreProducts_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        s.StoreProducts.Add(new StoreProductRecord
                        {
                            Id = r.GetInt32(0),
                            Name = AsString(r, 1),
                            Price = r.GetDecimal(2),
                            Category = AsString(r, 3),
                            Emoji = AsString(r, 4),
                            StockQty = r.GetInt32(5),
                            Expiry = AsString(r, 6),
                            PhotoBase64 = AsString(r, 7)
                        });

                var salesById = new Dictionary<int, StoreSaleRecord>();
                using (var cmd = Db.Proc("dbo.usp_StoreSales_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                    {
                        var sale = new StoreSaleRecord
                        {
                            SoldAt = AsString(r, 1),
                            Total = r.GetDecimal(2),
                            Summary = AsString(r, 3),
                            Items = new List<StoreSaleItemRecord>()
                        };
                        salesById[r.GetInt32(0)] = sale;
                        s.StoreSales.Add(sale);
                    }

                using (var cmd = Db.Proc("dbo.usp_StoreSaleItems_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                    {
                        if (salesById.TryGetValue(r.GetInt32(0), out var sale))
                            sale.Items.Add(new StoreSaleItemRecord
                            {
                                ProductName = AsString(r, 2),
                                Price = r.GetDecimal(3),
                                Qty = r.GetInt32(4),
                                ProductId = AsNullableInt(r, 5)
                            });
                    }

                using (var cmd = Db.Proc("dbo.usp_FeedingPlans_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        s.FeedingPlans.Add(new FeedingPlanRecord
                        {
                            Name = AsString(r, 1),
                            PdfPath = AsString(r, 2)
                        });

                using (var cmd = Db.Proc("dbo.usp_DietSendHistory_SelectAll", conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        s.DietSendHistory.Add(AsString(r, 1));
            }

            return s;
        }

        // ── persist ────────────────────────────────────────
        private static void PersistToDatabase(GymDataSnapshot s)
        {
            using (var conn = Db.GetOpenConnection())
            using (var tx = conn.BeginTransaction())
            {
                using (var clear = Db.Proc("dbo.usp_ClearAllData", conn, tx))
                    clear.ExecuteNonQuery();

                foreach (var u in s.Users)
                    using (var cmd = Db.Proc("dbo.usp_Users_Insert", conn, tx))
                    {
                        cmd.AddParam("@UserId", u.Id);
                        cmd.AddParam("@FullName", u.FullName);
                        cmd.AddParam("@Username", u.Username);
                        cmd.AddParam("@Password", u.Password);
                        cmd.AddParam("@Role", (int)u.Role);
                        cmd.ExecuteNonQuery();
                    }

                foreach (var p in s.SubscriptionPlans)
                {
                    using (var cmd = Db.Proc("dbo.usp_SubscriptionPlans_Insert", conn, tx))
                    {
                        cmd.AddParam("@PlanId", p.Id);
                        cmd.AddParam("@Name", p.Name);
                        cmd.AddParam("@Price", p.Price);
                        cmd.AddParam("@DurationValue", p.DurationValue);
                        cmd.AddParam("@DurationUnit", p.DurationUnit);
                        cmd.ExecuteNonQuery();
                    }

                    var features = p.Features ?? new List<string>();
                    for (int i = 0; i < features.Count; i++)
                        using (var cmd = Db.Proc("dbo.usp_PlanFeatures_Insert", conn, tx))
                        {
                            cmd.AddParam("@PlanId", p.Id);
                            cmd.AddParam("@FeatureNo", i + 1);
                            cmd.AddParam("@Feature", features[i]);
                            cmd.ExecuteNonQuery();
                        }
                }

                foreach (var m in s.Members)
                    using (var cmd = Db.Proc("dbo.usp_Members_Insert", conn, tx))
                    {
                        cmd.AddParam("@MemberId", m.Id);
                        cmd.AddParam("@FullName", m.FullName);
                        cmd.AddParam("@Phone", m.Phone);
                        cmd.AddParam("@Gender", m.Gender);
                        cmd.AddParam("@PlanName", m.PlanName);
                        cmd.AddParam("@PriceText", m.PriceText);
                        cmd.AddParam("@DurationText", m.DurationText);
                        cmd.AddParam("@JoinDate", ToDateParam(m.JoinDate));
                        cmd.AddParam("@PlanId", ResolvePlanId(s, m.PlanId));
                        cmd.ExecuteNonQuery();
                    }

                foreach (var t in s.Trainers)
                    using (var cmd = Db.Proc("dbo.usp_Trainers_Insert", conn, tx))
                    {
                        cmd.AddParam("@TrainerId", t.Id);
                        cmd.AddParam("@Name", t.Name);
                        cmd.AddParam("@Phone", t.Phone);
                        cmd.AddParam("@Specialty", t.Specialty);
                        cmd.AddParam("@Salary", t.Salary);
                        cmd.AddParam("@JoinDate", ToDateParam(t.JoinDate));
                        cmd.ExecuteNonQuery();
                    }

                foreach (var p in s.StoreProducts)
                    using (var cmd = Db.Proc("dbo.usp_StoreProducts_Insert", conn, tx))
                    {
                        cmd.AddParam("@ProductId", p.Id);
                        cmd.AddParam("@Name", p.Name);
                        cmd.AddParam("@Price", p.Price);
                        cmd.AddParam("@Category", p.Category);
                        cmd.AddParam("@Emoji", p.Emoji);
                        cmd.AddParam("@StockQty", p.StockQty);
                        cmd.AddParam("@Expiry", ToDateParam(p.Expiry));
                        cmd.AddParam("@PhotoBase64", p.PhotoBase64);
                        cmd.ExecuteNonQuery();
                    }

                int saleId = 0;
                foreach (var sale in s.StoreSales)
                {
                    saleId++;
                    using (var cmd = Db.Proc("dbo.usp_StoreSales_Insert", conn, tx))
                    {
                        cmd.AddParam("@SaleId", saleId);
                        cmd.AddParam("@SoldAt", sale.SoldAt);
                        cmd.AddParam("@Total", sale.Total);
                        cmd.AddParam("@Summary", sale.Summary);
                        cmd.ExecuteNonQuery();
                    }

                    var items = sale.Items ?? new List<StoreSaleItemRecord>();
                    for (int i = 0; i < items.Count; i++)
                        using (var cmd = Db.Proc("dbo.usp_StoreSaleItems_Insert", conn, tx))
                        {
                            cmd.AddParam("@SaleId", saleId);
                            cmd.AddParam("@LineNo", i + 1);
                            cmd.AddParam("@ProductName", items[i].ProductName);
                            cmd.AddParam("@Price", items[i].Price);
                            cmd.AddParam("@Qty", items[i].Qty);
                            cmd.AddParam("@ProductId", ResolveProductId(s, items[i].ProductId));
                            cmd.ExecuteNonQuery();
                        }
                }

                int feedingId = 0;
                foreach (var f in s.FeedingPlans)
                    using (var cmd = Db.Proc("dbo.usp_FeedingPlans_Insert", conn, tx))
                    {
                        cmd.AddParam("@FeedingPlanId", ++feedingId);
                        cmd.AddParam("@Name", f.Name);
                        cmd.AddParam("@PdfPath", f.PdfPath);
                        cmd.ExecuteNonQuery();
                    }

                int historyId = 0;
                foreach (var entry in s.DietSendHistory)
                    using (var cmd = Db.Proc("dbo.usp_DietSendHistory_Insert", conn, tx))
                    {
                        cmd.AddParam("@HistoryId", ++historyId);
                        cmd.AddParam("@Entry", entry);
                        cmd.ExecuteNonQuery();
                    }

                tx.Commit();
            }
        }

        // ── helpers ────────────────────────────────────────
        private static string AsString(SqlDataReader r, int ordinal)
            => r.IsDBNull(ordinal) ? null : r.GetValue(ordinal).ToString();

        private static int? AsNullableInt(SqlDataReader r, int ordinal)
            => r.IsDBNull(ordinal) ? (int?)null : r.GetInt32(ordinal);

        /// <summary>Returns the plan id only when it still exists in the snapshot, else DBNull (keeps the FK valid).</summary>
        private static object ResolvePlanId(GymDataSnapshot s, int? planId)
        {
            if (!planId.HasValue) return DBNull.Value;
            foreach (var p in s.SubscriptionPlans)
                if (p.Id == planId.Value) return planId.Value;
            return DBNull.Value;
        }

        /// <summary>Returns the product id only when it still exists in the snapshot, else DBNull (keeps the FK valid).</summary>
        private static object ResolveProductId(GymDataSnapshot s, int? productId)
        {
            if (!productId.HasValue) return DBNull.Value;
            foreach (var p in s.StoreProducts)
                if (p.Id == productId.Value) return productId.Value;
            return DBNull.Value;
        }

        /// <summary>Converts an app date string ("yyyy-MM-dd") to a DATE param value (DBNull when blank/invalid).</summary>
        private static object ToDateParam(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return DBNull.Value;
            return DateTime.TryParse(value, out var dt) ? (object)dt.Date : DBNull.Value;
        }

        public static int NextMemberId()
        {
            int max = 0;
            foreach (var m in Data.Members)
                if (m.Id > max) max = m.Id;
            return max + 1;
        }

        public static int NextTrainerId()
        {
            int max = 0;
            foreach (var t in Data.Trainers)
                if (t.Id > max) max = t.Id;
            return max + 1;
        }

        public static int NextProductId()
        {
            int max = 0;
            foreach (var p in Data.StoreProducts)
                if (p.Id > max) max = p.Id;
            return max + 1;
        }

        public static decimal ParsePriceFromMemberDisplay(string priceText)
        {
            if (string.IsNullOrWhiteSpace(priceText)) return 0;
            var sb = new StringBuilder();
            foreach (char c in priceText)
                if (char.IsDigit(c) || c == '.' || c == ',') sb.Append(c);
            string d = sb.ToString().Replace(',', '.');
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
                SubscriptionPlan plan = null;
                foreach (var p in Data.SubscriptionPlans)
                    if (p.Name == m.PlanName) { plan = p; break; }
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
            decimal total = 0;
            foreach (var s in Data.StoreSales)
                if (DateTime.TryParse(s.SoldAt, out var d) && d.Year == y && d.Month == mo)
                    total += s.Total;
            return total;
        }

        public static decimal SubscriptionCashInThisMonth()
        {
            int mo = DateTime.Now.Month, y = DateTime.Now.Year;
            decimal total = 0;
            foreach (var m in Data.Members)
                if (DateTime.TryParse(m.JoinDate, out var jd) && jd.Year == y && jd.Month == mo)
                    total += ParsePriceFromMemberDisplay(m.PriceText);
            return total;
        }

        public static decimal StoreSalesToday()
        {
            var t = DateTime.Today;
            decimal total = 0;
            foreach (var s in Data.StoreSales)
                if (DateTime.TryParse(s.SoldAt, out var d) && d.Date == t)
                    total += s.Total;
            return total;
        }
    }
}
