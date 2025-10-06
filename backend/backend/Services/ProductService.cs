using backend.Data;
using backend.DTO;
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

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            if (!_memoryCache.TryGetValue("all_products", out IEnumerable<ProductDto>? products))
            {
                products = await _storeContext.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        Name = p.Name,
                        Slug = p.Slug,
                        Description = p.Description,
                        Brand = p.Brand,
                        Price = p.Price,
                        DiscountPrice = p.DiscountPrice,
                        SKU = p.SKU,
                        StockQuantity = p.StockQuantity,
                        Weight = p.Weight,
                        Dimensions = p.Dimensions,
                        IsActive = p.IsActive,
                        ImageUrls = p.Images.Select(img => img.ImageUrl).ToList()
                    })
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                };

                _memoryCache.Set("all_products", products, cacheOptions);
            }

            return products!;
        }


        public async Task<ProductDto?> GetProductByIdAsync(long id)
        {
            var products = await GetProductsAsync(); 
            return products.FirstOrDefault(p => p.Id == id);
        }
    }
}