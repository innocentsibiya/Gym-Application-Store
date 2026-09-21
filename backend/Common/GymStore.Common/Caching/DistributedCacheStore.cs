using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace GymStore.Common.Caching;

/// <summary>Default <see cref="ICacheStore"/> over <see cref="IDistributedCache"/> (Redis in production).</summary>
internal sealed class DistributedCacheStore : ICacheStore
{
    private readonly IDistributedCache _cache;

    public DistributedCacheStore(IDistributedCache cache) => _cache = cache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct)
    {
        var json = await _cache.GetStringAsync(key, ct);
        return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json);
    }

    public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken ct) =>
        _cache.SetStringAsync(key, JsonSerializer.Serialize(value), options, ct);

    public Task RemoveAsync(string key, CancellationToken ct) => _cache.RemoveAsync(key, ct);
}
