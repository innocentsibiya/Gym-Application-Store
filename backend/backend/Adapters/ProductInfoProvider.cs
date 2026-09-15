using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Catalog.Contracts;

namespace backend.Adapters
{
    /// <summary>
    /// Bridges the Cart module's <see cref="IProductInfoProvider"/> port to the Catalog module's
    /// public <see cref="ICatalogModuleApi"/>. This is composition-root wiring: Cart depends only
    /// on its own port, Catalog exposes its contract, and the host connects the two — so Cart no
    /// longer reads product data from the shared DbContext.
    /// </summary>
    public sealed class ProductInfoProvider : IProductInfoProvider
    {
        private readonly ICatalogModuleApi _catalog;

        public ProductInfoProvider(ICatalogModuleApi catalog) => _catalog = catalog;

        public async Task<IReadOnlyDictionary<long, ProductInfo>> GetProductsAsync(
            IReadOnlyCollection<long> productIds, CancellationToken ct)
        {
            var summaries = await _catalog.GetProductsByIdsAsync(productIds, ct);

            return summaries.ToDictionary(
                kv => kv.Key,
                kv => new ProductInfo(kv.Value.ProductId, kv.Value.Name, kv.Value.Price, kv.Value.ImageUrls));
        }
    }
}
