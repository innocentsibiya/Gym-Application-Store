using Microsoft.Extensions.Caching.Distributed;

namespace GymStore.Common.Caching;

/// <summary>
/// Thin, typed wrapper over <see cref="IDistributedCache"/> that handles JSON serialization.
/// Modules use it for cache-aside so they don't each re-implement serialize/get/set.
/// </summary>
public interface ICacheStore
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct);
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken ct);
    Task RemoveAsync(string key, CancellationToken ct);
}
