using backend.Data;
using backend.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private readonly GymStoreContext _context;

        public InventoryRepository(GymStoreContext context) : base(context)
        {
            _context = context;
        }

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