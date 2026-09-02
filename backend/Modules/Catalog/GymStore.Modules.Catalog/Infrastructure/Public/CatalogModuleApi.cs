using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Contracts;

namespace GymStore.Modules.Catalog.Infrastructure.Public;

/// <summary>
/// Implements the cross-module contract. Reads come straight from the repository
/// (authoritative DB state) so callers like Cart get current product data.
/// </summary>
internal sealed class CatalogModuleApi : ICatalogModuleApi
{
    private readonly IProductRepository _repository;

    public CatalogModuleApi(IProductRepository repository) => _repository = repository;

    public async Task<IReadOnlyDictionary<long, ProductSummaryDto>> GetProductsByIdsAsync(
        IReadOnlyCollection<long> productIds, CancellationToken ct = default)
    {
        if (productIds.Count == 0)
        {
            return new Dictionary<long, ProductSummaryDto>();
        }

        var products = await _repository.GetByIdsAsync(productIds, ct);

        return products.ToDictionary(
            p => p.Id,
            p => new ProductSummaryDto(p.Id, p.Name, p.Price, p.Images.Select(i => i.ImageUrl).ToList()));
    }
}
