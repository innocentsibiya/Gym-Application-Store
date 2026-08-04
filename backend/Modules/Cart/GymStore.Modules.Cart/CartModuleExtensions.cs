using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
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
/// Composition entry point for the Cart module. The host calls <see cref="AddCartModule"/>
/// and registers <see cref="Assembly"/> as an MVC application part so the module's controller
/// is discovered. The host also supplies the <see cref="IProductInfoProvider"/> implementation.
/// </summary>
public static class CartModuleExtensions
{
    /// <summary>The module assembly (used by the host for controller discovery).</summary>
    public static Assembly Assembly => typeof(CartModuleExtensions).Assembly;

    public static IServiceCollection AddCartModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CartDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartCache, DistributedCartCache>();
        services.AddScoped<ICartModuleApi, CartModuleApi>();

        services.AddHandlersFromAssembly(Assembly);

        return services;
    }
}
