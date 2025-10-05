using backend.Models;

namespace backend.Interfaces
{
    public interface IWishlistService
    {
        Task<Wishlist> GetWishlistAsync(int userId);
        Task AddToWishlistAsync(int userId, int productId);
        Task RemoveFromWishlistAsync(int userId, int productId);
    }
}