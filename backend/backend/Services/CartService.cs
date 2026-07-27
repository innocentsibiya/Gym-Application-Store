using backend.Data;
using backend.DTO;
using backend.Interfaces;
using backend.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace backend.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IDistributedCache _cache;
        private const string CartKeyPrefix = "cart:";

        public CartService(ICartRepository cartRepository, IDistributedCache cache)
        {
            _cartRepository = cartRepository;
            _cache = cache;
        }

        private static string GetKey(long userId) => $"{CartKeyPrefix}{userId}";

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cacheKey = GetKey(userId);
            var cachedCart = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedCart))
                return JsonSerializer.Deserialize<CartDto>(cachedCart)!;

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                var newCart = new Cart { UserId = userId };
                await _cartRepository.AddAsync(newCart);
                await _cartRepository.SaveChangesAsync();

                cart = new CartDto { Id = newCart.Id, UserId = userId };
            }

            await SaveCartToCache(cart);
            return cart;
        }

        public async Task<CartDto> AddToCartAsync(int userId, int productId, int quantity)
        {
            var updatedCart = await _cartRepository.AddItemAsync(userId, productId, quantity);
            await SaveCartToCache(updatedCart);
            return updatedCart;
        }

        public async Task<CartDto> RemoveFromCartAsync(int userId, int productId)
        {
            var updatedCart = await _cartRepository.RemoveItemAsync(userId, productId);
            await SaveCartToCache(updatedCart);
            return updatedCart;
        }

        private async Task SaveCartToCache(CartDto cart)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };

            await _cache.SetStringAsync(GetKey(cart.UserId), JsonSerializer.Serialize(cart), options);
        }
    }
}