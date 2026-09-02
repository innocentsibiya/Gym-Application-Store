using GymStore.Modules.Catalog.Domain;

namespace GymStore.Modules.Catalog.Application.Abstractions;

/// <summary>Read operations for products (module-internal).</summary>
public interface IProductRepository
{
    /// <summary>Active products with Category and Images loaded (for listing).</summary>
    Task<IReadOnlyList<Product>> GetActiveAsync(CancellationToken ct);

    /// <summary>A single active product with Category and Images, or null.</summary>
    Task<Product?> GetByIdAsync(long id, CancellationToken ct);

    /// <summary>Paged search over active products; returns the page plus the total match count.</summary>
    Task<(IReadOnlyList<Product> Products, int TotalCount)> SearchAsync(
        string term, int page, int pageSize, CancellationToken ct);

    /// <summary>Products (with Images) for the given ids — no active filter; for cross-module lookups.</summary>
    Task<IReadOnlyList<Product>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken ct);
}
