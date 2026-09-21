using GymStore.Common.Caching;
using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Application.Responses;
using Microsoft.Extensions.Caching.Distributed;

namespace GymStore.Modules.Catalog.Infrastructure.Caching;

/// <summary>
/// Cache-aside for products. Owns the module-specific keys and expiry ("products:all" and
/// "product:{id}", 6h absolute / 1h sliding); serialization is delegated to the shared
/// <see cref="ICacheStore"/>.
/// </summary>
internal sealed class DistributedProductCache : IProductCache
{
    private const string AllKey = "products:all";

    private static readonly DistributedCacheEntryOptions Options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
        SlidingExpiration = TimeSpan.FromHours(1)
    };

    private readonly ICacheStore _store;

    public DistributedProductCache(ICacheStore store) => _store = store;

    private static string IdKey(long id) => $"product:{id}";

    public Task<IReadOnlyList<ProductResponse>?> GetAllAsync(CancellationToken ct) =>
        _store.GetAsync<IReadOnlyList<ProductResponse>>(AllKey, ct);

    public Task SetAllAsync(IReadOnlyList<ProductResponse> products, CancellationToken ct) =>
        _store.SetAsync(AllKey, products, Options, ct);

    public Task<ProductResponse?> GetByIdAsync(long id, CancellationToken ct) =>
        _store.GetAsync<ProductResponse>(IdKey(id), ct);

    public Task SetByIdAsync(long id, ProductResponse product, CancellationToken ct) =>
        _store.SetAsync(IdKey(id), product, Options, ct);
}
