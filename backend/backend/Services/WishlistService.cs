using backend.Interfaces;
using backend.IRepository;
using backend.Models;

namespace backend.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<Wishlist> GetWishlistAsync(int userId)
        {
            return await _wishlistRepository.GetWishlistAsync(userId);
        }

        public async Task AddToWishlistAsync(int userId, int productId)
        {
            await _wishlistRepository.AddToWishlistAsync(userId, productId);
        }

        public async Task RemoveFromWishlistAsync(int userId, int productId)
        {
            await _wishlistRepository.RemoveFromWishlistAsync(userId, productId);
        }
    }
}