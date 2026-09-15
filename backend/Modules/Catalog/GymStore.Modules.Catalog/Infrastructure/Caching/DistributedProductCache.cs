using System.Text.Json;
using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Application.Responses;
using Microsoft.Extensions.Caching.Distributed;

namespace GymStore.Modules.Catalog.Infrastructure.Caching;

/// <summary>
/// Cache-aside implementation over <see cref="IDistributedCache"/> (Redis). Keys and
/// expiration are kept identical to the original ProductService ("products:all" and
/// "product:{id}", 6h absolute / 1h sliding).
/// </summary>
internal sealed class DistributedProductCache : IProductCache
{
    private const string AllKey = "products:all";

    private static readonly DistributedCacheEntryOptions Options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
        SlidingExpiration = TimeSpan.FromHours(1)
    };

    private readonly IDistributedCache _cache;

    public DistributedProductCache(IDistributedCache cache) => _cache = cache;

    private static string IdKey(long id) => $"product:{id}";

    public async Task<IReadOnlyList<ProductResponse>?> GetAllAsync(CancellationToken ct)
    {
        var json = await _cache.GetStringAsync(AllKey, ct);
        return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<List<ProductResponse>>(json);
    }

    public Task SetAllAsync(IReadOnlyList<ProductResponse> products, CancellationToken ct) =>
        _cache.SetStringAsync(AllKey, JsonSerializer.Serialize(products), Options, ct);

    public async Task<ProductResponse?> GetByIdAsync(long id, CancellationToken ct)
    {
        var json = await _cache.GetStringAsync(IdKey(id), ct);
        return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<ProductResponse>(json);
    }

    public Task SetByIdAsync(long id, ProductResponse product, CancellationToken ct) =>
        _cache.SetStringAsync(IdKey(id), JsonSerializer.Serialize(product), Options, ct);
}
