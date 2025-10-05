namespace backend.Interfaces
{
    public interface IInventoryService
    {
        Task UpdateStockAsync(int productId, int change);
        Task<int> GetStockLevelAsync(int productId);
    }
}