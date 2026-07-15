using System.Text.Json;
using backend.Data;
using backend.DTO;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace backend.Services
{
    public class ProductService : IProductService
    {
        private readonly GymStoreContext _storeContext;
        private readonly IDistributedCache _cache;

        private const string ProductCacheKey = "products:all";

        public ProductService(
            GymStoreContext context,
            IDistributedCache cache)
        {
            _storeContext = context;
            _cache = cache;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var cachedProducts =
                await _cache.GetStringAsync(ProductCacheKey);

            if (!string.IsNullOrEmpty(cachedProducts))
            {
                return JsonSerializer.Deserialize<List<ProductDto>>(cachedProducts)!
                       ?? Enumerable.Empty<ProductDto>();
            }

            var products = await _storeContext.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
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
                    ImageUrls = p.Images
                        .Select(img => img.ImageUrl)
                        .ToList()
                })
                .ToListAsync();

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            await _cache.SetStringAsync(
                ProductCacheKey,
                JsonSerializer.Serialize(products),
                cacheOptions);

            return products;
        }

        public async Task<ProductDto?> GetProductByIdAsync(long id)
        {
            var cacheKey = $"product:{id}";

            var cachedProduct =
                await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedProduct))
            {
                return JsonSerializer.Deserialize<ProductDto>(cachedProduct);
            }

            var product = await _storeContext.Products
                .AsNoTracking()
                .Where(p => p.Id == id && p.IsActive)
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
                    ImageUrls = p.Images
                        .Select(img => img.ImageUrl)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new KeyNotFoundException(
                    $"Product with ID {id} not found.");
            }

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(product),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                    SlidingExpiration = TimeSpan.FromHours(1)
                });

            return product;
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(
            string term,
            int page,
            int pageSize)
        {
            IQueryable<Product> query = _storeContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(term))
            {
                term = term.ToLower();

                query = query.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Description.ToLower().Contains(term) ||
                    (p.Brand != null &&
                     p.Brand.ToLower().Contains(term)) ||
                    (p.Category != null &&
                     p.Category.Name.ToLower().Contains(term)));
            }

            var totalCount = await query.CountAsync();

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task InvalidateProductCacheAsync(long productId)
        {
            await _cache.RemoveAsync(ProductCacheKey);
            await _cache.RemoveAsync($"product:{productId}");
        }
    }
}