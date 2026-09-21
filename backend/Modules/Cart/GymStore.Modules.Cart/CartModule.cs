using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Contracts;
using GymStore.Modules.Cart.Infrastructure.Caching;
using GymStore.Modules.Cart.Infrastructure.Persistence;
using GymStore.Modules.Cart.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Cart;

/// <summary>
/// Composition entry point for the Cart module. The host registers it via the common
/// <see cref="IModule"/> mechanism; the host also supplies the <see cref="IProductInfoProvider"/>
/// implementation.
/// </summary>
public sealed class CartModule : IModule
{
    public Assembly Assembly => typeof(CartModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CartDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartCache, DistributedCartCache>();
        services.AddScoped<ICartModuleApi, CartModuleApi>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
