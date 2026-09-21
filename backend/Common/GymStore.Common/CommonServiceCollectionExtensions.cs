using GymStore.Common.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GymStore.Common;

public static class CommonServiceCollectionExtensions
{
    /// <summary>
    /// Registers shared-kernel services. Call once at the composition root. Expects an
    /// <see cref="IDistributedCache"/> to already be registered by the host.
    /// </summary>
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.TryAddSingleton<ICacheStore, DistributedCacheStore>();
        return services;
    }
}
