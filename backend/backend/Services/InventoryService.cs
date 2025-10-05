using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly GymStoreContext _context;
        public InventoryService(GymStoreContext context) => _context = context;

        public async Task UpdateStockAsync(int productId, int change)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
            if (inventory == null) throw new KeyNotFoundException("Product not found in inventory.");

            inventory.QuantityAvailable += change;
            inventory.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetStockLevelAsync(int productId)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
            return inventory?.QuantityAvailable ?? 0;
        }
    }
}