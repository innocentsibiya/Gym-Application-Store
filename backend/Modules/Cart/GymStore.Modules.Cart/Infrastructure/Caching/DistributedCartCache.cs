using GymStore.Common.Caching;
using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Application.Responses;
using Microsoft.Extensions.Caching.Distributed;

namespace GymStore.Modules.Cart.Infrastructure.Caching;

/// <summary>
/// Cache-aside for carts. Owns the module-specific key and expiry ("cart:{userId}", 2h absolute /
/// 30m sliding); serialization/transport is delegated to the shared <see cref="ICacheStore"/>.
/// </summary>
internal sealed class DistributedCartCache : ICartCache
{
    private const string KeyPrefix = "cart:";

    private static readonly DistributedCacheEntryOptions Options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
        SlidingExpiration = TimeSpan.FromMinutes(30)
    };

    private readonly ICacheStore _store;

    public DistributedCartCache(ICacheStore store) => _store = store;

    private static string Key(long userId) => $"{KeyPrefix}{userId}";

    public Task<CartResponse?> GetAsync(long userId, CancellationToken ct) =>
        _store.GetAsync<CartResponse>(Key(userId), ct);

    public Task SetAsync(long userId, CartResponse cart, CancellationToken ct) =>
        _store.SetAsync(Key(userId), cart, Options, ct);

    public Task RemoveAsync(long userId, CancellationToken ct) =>
        _store.RemoveAsync(Key(userId), ct);
}
