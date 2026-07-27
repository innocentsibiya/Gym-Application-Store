using backend.DTO;
using backend.Interfaces;
using backend.IRepository;
using backend.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace backend.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDistributedCache _cache;
        private const string ProductCacheKey = "products:all";

        public ProductService(IProductRepository productRepository, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var cachedProducts = await _cache.GetStringAsync(ProductCacheKey);
            if (!string.IsNullOrEmpty(cachedProducts))
                return JsonSerializer.Deserialize<List<ProductDto>>(cachedProducts)! ?? Enumerable.Empty<ProductDto>();

            var products = await _productRepository.GetProductsAsync();

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            await _cache.SetStringAsync(ProductCacheKey, JsonSerializer.Serialize(products), cacheOptions);
            return products;
        }

        public async Task<ProductDto?> GetProductByIdAsync(long id)
        {
            var cacheKey = $"product:{id}";
            var cachedProduct = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedProduct))
                return JsonSerializer.Deserialize<ProductDto>(cachedProduct);

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(product),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                    SlidingExpiration = TimeSpan.FromHours(1)
                });

            return product;
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(string term, int page, int pageSize)
            => await _productRepository.SearchAsync(term, page, pageSize);

        public async Task InvalidateProductCacheAsync(long productId)
        {
            await _cache.RemoveAsync(ProductCacheKey);
            await _cache.RemoveAsync($"product:{productId}");
        }
    }
}