using backend.Data;
using GymStore.Modules.Cart.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace backend.Adapters
{
    /// <summary>
    /// Host-side implementation of the Cart module's <see cref="IProductInfoProvider"/> port,
    /// backed by the shared catalog data in <see cref="GymStoreContext"/>. When a dedicated
    /// Catalog module is extracted, this adapter moves there and Cart depends on its contract.
    /// </summary>
    public sealed class ProductInfoProvider : IProductInfoProvider
    {
        private readonly GymStoreContext _context;

        public ProductInfoProvider(GymStoreContext context) => _context = context;

        public async Task<IReadOnlyDictionary<long, ProductInfo>> GetProductsAsync(
            IReadOnlyCollection<long> productIds, CancellationToken ct)
        {
            if (productIds.Count == 0)
            {
                return new Dictionary<long, ProductInfo>();
            }

            var rows = await _context.Products
                .AsNoTracking()
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    ImageUrls = p.Images.Select(img => img.ImageUrl).ToList()
                })
                .ToListAsync(ct);

            return rows.ToDictionary(
                r => r.Id,
                r => new ProductInfo(r.Id, r.Name, r.Price, r.ImageUrls));
        }
    }
}
