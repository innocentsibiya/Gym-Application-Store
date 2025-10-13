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
            var product = (await GetProductsAsync()).FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            return product;
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(string term, int page, int pageSize)
        {
            IQueryable<Product> query = _storeContext.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(term))
            {
                term = term.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Description.ToLower().Contains(term) ||
                    (p.Brand != null && p.Brand.ToLower().Contains(term)) ||
                    (p.Category != null && p.Category.Name.ToLower().Contains(term))
                );
            }

            int totalCount = await query.CountAsync();
            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }
    }
}