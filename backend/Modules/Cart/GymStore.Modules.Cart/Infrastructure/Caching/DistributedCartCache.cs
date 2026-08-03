using System.Text.Json;
using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Application.Responses;
using Microsoft.Extensions.Caching.Distributed;

namespace GymStore.Modules.Cart.Infrastructure.Caching;

/// <summary>
/// Cache-aside implementation over <see cref="IDistributedCache"/> (Redis). Key format and
/// expiration are kept identical to the original CartService ("cart:{userId}", 2h absolute /
/// 30m sliding).
/// </summary>
internal sealed class DistributedCartCache : ICartCache
{
    private const string KeyPrefix = "cart:";

    private static readonly DistributedCacheEntryOptions Options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
        SlidingExpiration = TimeSpan.FromMinutes(30)
    };

    private readonly IDistributedCache _cache;

    public DistributedCartCache(IDistributedCache cache) => _cache = cache;

    private static string Key(long userId) => $"{KeyPrefix}{userId}";

    public async Task<CartResponse?> GetAsync(long userId, CancellationToken ct)
    {
        var json = await _cache.GetStringAsync(Key(userId), ct);
        return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<CartResponse>(json);
    }

    public Task SetAsync(long userId, CartResponse cart, CancellationToken ct) =>
        _cache.SetStringAsync(Key(userId), JsonSerializer.Serialize(cart), Options, ct);

    public Task RemoveAsync(long userId, CancellationToken ct) =>
        _cache.RemoveAsync(Key(userId), ct);
}
