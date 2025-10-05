using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace backend.Services
{
    public class CartService : ICartService
    {
        private readonly GymStoreContext _context;
        private readonly IMemoryCache _cache;
        private const string CartCacheKeyPrefix = "CartCache";

        public CartService(GymStoreContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Cart> GetCartAsync(int userId)
        {
            var cacheKey = $"{CartCacheKeyPrefix}{userId}";

            if (!_cache.TryGetValue(cacheKey, out Cart? cart))
            {
                cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                _cache.Set(cacheKey, cart, TimeSpan.FromMinutes(15));
            }

            return cart!;
        }

        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId);

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CartId = cart.Id
                });
            }

            _context.Update(cart);
            await _context.SaveChangesAsync();

            _cache.Remove($"{CartCacheKeyPrefix}{userId}");
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await GetCartAsync(userId);

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            _cache.Remove($"{CartCacheKeyPrefix}{userId}");
        }
    }
}