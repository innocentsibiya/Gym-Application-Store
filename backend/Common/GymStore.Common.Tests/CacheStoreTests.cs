using GymStore.Common;
using GymStore.Common.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Common.Tests;

public class CacheStoreTests
{
    private sealed record Sample(long Id, string Name, IReadOnlyList<string> Tags);

    private static ICacheStore NewStore()
    {
        var services = new ServiceCollection();
        services.AddDistributedMemoryCache(); // in-memory IDistributedCache
        services.AddCommon();
        return services.BuildServiceProvider().GetRequiredService<ICacheStore>();
    }

    private static readonly DistributedCacheEntryOptions Options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
    };

    [Fact]
    public async Task Set_Then_Get_RoundTripsTheValue()
    {
        var store = NewStore();
        var value = new Sample(1, "Dumbbell", new[] { "a", "b" });

        await store.SetAsync("k1", value, Options, CancellationToken.None);
        var read = await store.GetAsync<Sample>("k1", CancellationToken.None);

        Assert.NotNull(read);
        Assert.Equal(value.Id, read!.Id);
        Assert.Equal(value.Name, read.Name);
        Assert.Equal(value.Tags, read.Tags); // Assert.Equal does sequence comparison for IEnumerable
    }

    [Fact]
    public async Task Get_MissingKey_ReturnsNull()
    {
        var store = NewStore();

        var read = await store.GetAsync<Sample>("does-not-exist", CancellationToken.None);

        Assert.Null(read);
    }

    [Fact]
    public async Task Remove_EvictsTheValue()
    {
        var store = NewStore();
        await store.SetAsync("k1", new Sample(1, "x", Array.Empty<string>()), Options, CancellationToken.None);

        await store.RemoveAsync("k1", CancellationToken.None);
        var read = await store.GetAsync<Sample>("k1", CancellationToken.None);

        Assert.Null(read);
    }
}
