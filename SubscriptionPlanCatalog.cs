using System;
using System.Collections.Generic;
using System.Linq;

namespace gym_mangment_system
{
    public class SubscriptionPlan
    {
        public SubscriptionPlan() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationValue { get; set; }
        public string DurationUnit { get; set; }

        /// <summary>المميزات: feature/benefit lines shown on the plan card and member form.</summary>
        public List<string> Features { get; set; } = new List<string>();

        public string DisplayName => $"{Name} - {Price.ToString("0.##")}$ / {DurationValue} {DurationUnit}";
    }

    /// <summary>Default catalogue of gym features (المميزات) offered as checkboxes per plan.</summary>
    public static class SubscriptionFeatureCatalog
    {
        public static readonly string[] DefaultFeatures =
        {
            "دخول غير محدود",
            "حصص جماعية",
            "مدرب شخصي",
            "غرفة ساونا",
            "خزانة خاصة",
            "خصم على المتجر",
            "خطة تغذية",
            "مواقف سيارات",
            "مناشف مجانية",
            "تقييم لياقة شهري"
        };
    }

    public static class SubscriptionPlanCatalog
    {
        public static IEnumerable<SubscriptionPlan> GetPlans() => GymDataStore.Data.SubscriptionPlans;

        public static void Upsert(SubscriptionPlan plan)
        {
            var list = GymDataStore.Data.SubscriptionPlans;
            var existing = list.FirstOrDefault(p => p.Id == plan.Id);
            if (existing != null)
            {
                existing.Name = plan.Name;
                existing.Price = plan.Price;
                existing.DurationValue = plan.DurationValue;
                existing.DurationUnit = plan.DurationUnit;
                existing.Features = plan.Features ?? new List<string>();
            }
            else
            {
                list.Add(plan);
            }
            GymDataStore.Save();
        }

        public static void Delete(int id)
        {
            var list = GymDataStore.Data.SubscriptionPlans;
            var existing = list.FirstOrDefault(p => p.Id == id);
            if (existing != null)
                list.Remove(existing);
            GymDataStore.Save();
        }
    }
}
