using System;
using System.Collections.Generic;
using System.Linq;

namespace gym_mangment_system
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationValue { get; set; }
        public string DurationUnit { get; set; }

        public string DisplayName => $"{Name} - {Price.ToString("0.##")}$ / {DurationValue} {DurationUnit}";
    }

    public static class SubscriptionPlanCatalog
    {
        private static List<SubscriptionPlan> _plans = new List<SubscriptionPlan>
        {   
            new SubscriptionPlan { Id = 1, Name = "Basic Plan", Price = 30.00m, DurationValue = 1, DurationUnit = "شهر" },
            new SubscriptionPlan { Id = 2, Name = "Pro Plan", Price = 50.00m, DurationValue = 3, DurationUnit = "شهر" },
            new SubscriptionPlan { Id = 3, Name = "Annual Plan", Price = 300.00m, DurationValue = 1, DurationUnit = "سنة" }
        };

        public static IEnumerable<SubscriptionPlan> GetPlans()
        {
            return _plans;
        }

        public static void Upsert(SubscriptionPlan plan)
        {
            var existing = _plans.FirstOrDefault(p => p.Id == plan.Id);
            if (existing != null)
            {
                existing.Name = plan.Name;
                existing.Price = plan.Price;
                existing.DurationValue = plan.DurationValue;
                existing.DurationUnit = plan.DurationUnit;
            }
            else
            {
                _plans.Add(plan);
            }
        }

        public static void Delete(int id)
        {
            var existing = _plans.FirstOrDefault(p => p.Id == id);
            if (existing != null)
            {
                _plans.Remove(existing);
            }
        }
    }
}
