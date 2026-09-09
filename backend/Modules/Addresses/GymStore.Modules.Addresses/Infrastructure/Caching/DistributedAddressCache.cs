using GymStore.Common.Caching;
using GymStore.Modules.Addresses.Application.Abstractions;
using GymStore.Modules.Addresses.Application.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace GymStore.Modules.Addresses.Infrastructure.Caching;

/// <summary>
/// Cache-aside for a user's addresses. Keeps the module-specific key and expiry
/// ("addresses:{userId}", 30m absolute) from the original AddressService; serialization is
/// delegated to the shared <see cref="ICacheStore"/>.
/// </summary>
internal sealed class DistributedAddressCache : IAddressCache
{
    private const string KeyPrefix = "addresses:";

    private static readonly DistributedCacheEntryOptions Options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
    };

    private readonly ICacheStore _store;

    public DistributedAddressCache(ICacheStore store) => _store = store;

    private static string Key(long userId) => $"{KeyPrefix}{userId}";

    public Task<IReadOnlyList<AddressDto>?> GetAsync(long userId, CancellationToken ct) =>
        _store.GetAsync<IReadOnlyList<AddressDto>>(Key(userId), ct);

    public Task SetAsync(long userId, IReadOnlyList<AddressDto> addresses, CancellationToken ct) =>
        _store.SetAsync(Key(userId), addresses, Options, ct);

    public Task RemoveAsync(long userId, CancellationToken ct) =>
        _store.RemoveAsync(Key(userId), ct);
}
