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

        public string DisplayName => $"{Name} - {Price.ToString("0.##")}$ / {DurationValue} {DurationUnit}";
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
