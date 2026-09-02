using GymStore.Modules.Catalog.Application.Responses;

namespace GymStore.Modules.Catalog.Application.Abstractions;

/// <summary>Cache-aside store for product responses (backed by Redis in production).</summary>
public interface IProductCache
{
    Task<IReadOnlyList<ProductResponse>?> GetAllAsync(CancellationToken ct);
    Task SetAllAsync(IReadOnlyList<ProductResponse> products, CancellationToken ct);

    Task<ProductResponse?> GetByIdAsync(long id, CancellationToken ct);
    Task SetByIdAsync(long id, ProductResponse product, CancellationToken ct);
}
