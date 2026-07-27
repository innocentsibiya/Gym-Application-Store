using backend.Models;

namespace backend.IRepository
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
        Task<Wishlist> GetWishlistAsync(int userId);
        Task AddToWishlistAsync(int userId, int productId);
        Task RemoveFromWishlistAsync(int userId, int productId);
    }
}