using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace backend.Services
{
    public class ProductService : IProductService
    {
        private readonly GymStoreContext _storeContext;
        private readonly IMemoryCache _memoryCache;

        private const string ProductCacheKey = "all_products";
        public ProductService(GymStoreContext context, IMemoryCache cache)
        {
            _storeContext = context;
            _memoryCache = cache;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            if (!_memoryCache.TryGetValue(ProductCacheKey, out IEnumerable<Product>? products))
            {
                products = await _storeContext.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                };

                _memoryCache.Set(ProductCacheKey, products, cacheOptions);
            }

            return products!;

        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var products = await GetProductsAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }
    }
}
