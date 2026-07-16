using backend.Data;
using backend.DTO;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace backend.Services
{
    public class CartService : ICartService
    {
        private readonly GymStoreContext _context;
        private readonly IDistributedCache _cache;

        private const string CartKeyPrefix = "cart:";

        public CartService(
            GymStoreContext context,
            IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        private static string GetKey(long userId)
            => $"{CartKeyPrefix}{userId}";

        public async Task<CartDto> GetCartAsync(int userId)
        {
          var cacheKey = GetKey(userId);

            var cachedCart = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedCart))
            {
                return JsonSerializer.Deserialize<CartDto>(cachedCart)!;
            }

            var cart = await _context.Carts
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Items = c.Items.Select(i => new CartItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        ImageUrls = i.Product.Images.Select(img => img.ImageUrl).ToList(),
                        Price = i.Product.Price,
                        Quantity = i.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                var newCart = new Cart
                {
                    UserId = userId
                };

                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();

                cart = new CartDto
                {
                    Id = newCart.Id,
                    UserId = userId
                };
            }

            await SaveCartToCache(cart);

            return cart;
        }

        public async Task<CartDto> AddToCartAsync(
            int userId,
            int productId,
            int quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

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
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();

            var updatedCart = await GetCartFromDatabase(userId);

            await SaveCartToCache(updatedCart);

            return updatedCart;
        }

        public async Task<CartDto> RemoveFromCartAsync(
            int userId,
            int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId);

            if (cart == null)
                throw new Exception("Cart not found");

            var item =
                cart.Items.FirstOrDefault(i =>
                    i.ProductId == productId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            var updatedCart = await GetCartFromDatabase(userId);

            await SaveCartToCache(updatedCart);

            return updatedCart;
        }

        private async Task<CartDto> GetCartFromDatabase(int userId)
        {
            return await _context.Carts
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Items = c.Items.Select(i => new CartItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        ImageUrls = i.Product.Images.Select(img => img.ImageUrl).ToList(),
                        Price = i.Product.Price,
                        Quantity = i.Quantity
                    }).ToList()
                })
                .FirstAsync();
        }

        private async Task SaveCartToCache(CartDto cart)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromHours(2),

                SlidingExpiration =
                    TimeSpan.FromMinutes(30)
            };

            await _cache.SetStringAsync(
                GetKey(cart.UserId),
                JsonSerializer.Serialize(cart),
                options);
        }
    }
}