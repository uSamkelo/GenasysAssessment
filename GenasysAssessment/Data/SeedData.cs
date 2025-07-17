using System.Text.Json;
using GenasysAssessment.Models;
using GenasysAssessment.Data;
using Microsoft.EntityFrameworkCore;

namespace GenasysAssessment.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDataContext context)
        {
            if (context.Orders.Any() || context.InventoryItems.Any() || context.PaymentTransactions.Any())
                return;

            var json = File.ReadAllText("seedData.json");
            var seed = JsonSerializer.Deserialize<SeedDataModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (seed == null) return;

            context.Orders.AddRange(seed.Orders);
            context.InventoryItems.AddRange(seed.InventoryItems);
            context.PaymentTransactions.AddRange(seed.PaymentTransactions);
            context.SaveChanges();
        }
    }

    public class SeedDataModel
    {
        public List<Order> Orders { get; set; } = new();
        public List<InventoryItem> InventoryItems { get; set; } = new();
        public List<PaymentTransaction> PaymentTransactions { get; set; } = new();
    }
}
