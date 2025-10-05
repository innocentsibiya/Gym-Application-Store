using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly GymStoreContext _context;
        public WishlistService(GymStoreContext context) => _context = context;

        public async Task<Wishlist> GetWishlistAsync(int userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = userId, Items = new List<WishlistItem>() };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            return wishlist;
        }

        public async Task AddToWishlistAsync(int userId, int productId)
        {
            var wishlist = await GetWishlistAsync(userId);
            if (!wishlist.Items.Any(i => i.ProductId == productId))
            {
                wishlist.Items.Add(new WishlistItem { ProductId = productId, WishlistId = wishlist.Id });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromWishlistAsync(int userId, int productId)
        {
            var wishlist = await GetWishlistAsync(userId);
            var item = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                wishlist.Items.Remove(item);
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}