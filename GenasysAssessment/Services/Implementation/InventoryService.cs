using GenasysAssessment.Models;
using GenasysAssessment.Services.Interfaces;
using System.Collections.Concurrent;

namespace GenasysAssessment.Services.Implementation
{
    public class InventoryService : IInventoryService
    {
        private static readonly Dictionary<int, InventoryItem> Inventory = new();
        private static readonly object _lock = new();

        public bool CheckAvailability(int productId, int quantity)
        {
            if (!Inventory.TryGetValue(productId, out var item))
            {
                item = new InventoryItem { ProductId = productId, AvailableQuantity = 100, ReservedQuantity = 0 };
                Inventory[productId] = item;
            }
            return item.AvailableQuantity >= quantity;
        }

        public bool Reserve(int productId, int quantity)
        {
            lock (_lock)
            {
                if (!Inventory.TryGetValue(productId, out var item))
                {
                    item = new InventoryItem { ProductId = productId, AvailableQuantity = 100, ReservedQuantity = 0 };
                    Inventory[productId] = item;
                }
                if (item.AvailableQuantity < quantity)
                    return false;
                item.AvailableQuantity -= quantity;
                item.ReservedQuantity += quantity;
                return true;
            }
        }

        public void Release(int productId, int quantity)
        {
            lock (_lock)
            {
                if (!Inventory.TryGetValue(productId, out var item))
                {
                    item = new InventoryItem { ProductId = productId, AvailableQuantity = 100, ReservedQuantity = 0 };
                    Inventory[productId] = item;
                }
                item.AvailableQuantity += quantity;
                item.ReservedQuantity = Math.Max(0, item.ReservedQuantity - quantity);
            }
        }
    }
}