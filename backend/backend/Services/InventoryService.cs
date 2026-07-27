using backend.Interfaces;
using backend.IRepository;

namespace backend.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task UpdateStockAsync(int productId, int change)
        {
            await _inventoryRepository.UpdateStockAsync(productId, change);
        }

        public async Task<int> GetStockLevelAsync(int productId)
        {
            return await _inventoryRepository.GetStockLevelAsync(productId);
        }
    }
}