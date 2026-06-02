using System;
using System.Collections.Generic;

namespace gym_mangment_system
{
    public sealed class MemberRecord
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }

        /// <summary>FK to SubscriptionPlans.PlanId (null when no/unknown plan).</summary>
        public int? PlanId { get; set; }

        public string PlanName { get; set; }
        public string PriceText { get; set; }
        public string DurationText { get; set; }
        public string JoinDate { get; set; }
    }

    public sealed class TrainerRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Specialty { get; set; }
        public decimal Salary { get; set; }
        public string JoinDate { get; set; }
    }

    public sealed class StoreProductRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Emoji { get; set; }
        public int StockQty { get; set; }
        public string Expiry { get; set; }
        public string PhotoBase64 { get; set; }
    }

    public sealed class StoreSaleItemRecord
    {
        /// <summary>FK to StoreProducts.ProductId (null when the product was removed).</summary>
        public int? ProductId { get; set; }

        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }

    public sealed class StoreSaleRecord
    {
        public string SoldAt { get; set; }
        public decimal Total { get; set; }
        public string Summary { get; set; }

        /// <summary>Normalized line items (one entry per cart line) for this sale.</summary>
        public List<StoreSaleItemRecord> Items { get; set; } = new List<StoreSaleItemRecord>();
    }

    public sealed class FeedingPlanRecord
    {
        public string Name { get; set; }
        public string PdfPath { get; set; }
    }

    /// <summary>Root in-memory snapshot (session only; not persisted to disk).</summary>
    public sealed class GymDataSnapshot
    {
        public int Version { get; set; } = 1;
        public List<MemberRecord> Members { get; set; }
        public List<TrainerRecord> Trainers { get; set; }
        public List<UserDirectoryEntry> Users { get; set; }
        public List<SubscriptionPlan> SubscriptionPlans { get; set; }
        public List<StoreProductRecord> StoreProducts { get; set; }
        public List<StoreSaleRecord> StoreSales { get; set; }
        public List<FeedingPlanRecord> FeedingPlans { get; set; }
        public List<string> DietSendHistory { get; set; }

        public static GymDataSnapshot CreateEmpty()
        {
            return new GymDataSnapshot
            {
                Members            = new List<MemberRecord>(),
                Trainers           = new List<TrainerRecord>(),
                Users              = new List<UserDirectoryEntry>(),
                SubscriptionPlans  = new List<SubscriptionPlan>(),
                StoreProducts      = new List<StoreProductRecord>(),
                StoreSales         = new List<StoreSaleRecord>(),
                FeedingPlans       = new List<FeedingPlanRecord>(),
                DietSendHistory    = new List<string>()
            };
        }
    }
}
