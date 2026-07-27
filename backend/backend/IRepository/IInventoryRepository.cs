using backend.Models;

namespace backend.IRepository
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task UpdateStockAsync(int productId, int change);
        Task<int> GetStockLevelAsync(int productId);
    }
}