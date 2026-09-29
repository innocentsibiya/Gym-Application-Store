using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Wishlist.Application.Abstractions;
using GymStore.Modules.Wishlist.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Wishlist;

/// <summary>
/// Composition entry point for the Wishlist module (registered via the common IModule mechanism).
/// Depends on the Catalog contract to enrich items with product data.
/// </summary>
public sealed class WishlistModule : IModule
{
    public Assembly Assembly => typeof(WishlistModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WishlistDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
