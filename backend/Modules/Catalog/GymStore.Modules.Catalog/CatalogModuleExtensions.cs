using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Catalog.Infrastructure.Caching;
using GymStore.Modules.Catalog.Infrastructure.Persistence;
using GymStore.Modules.Catalog.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Catalog;

/// <summary>
/// Composition entry point for the Catalog module. The host calls <see cref="AddCatalogModule"/>
/// and registers <see cref="Assembly"/> as an MVC application part so the module's controllers
/// are discovered.
/// </summary>
public static class CatalogModuleExtensions
{
    /// <summary>The module assembly (used by the host for controller discovery).</summary>
    public static Assembly Assembly => typeof(CatalogModuleExtensions).Assembly;

    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductCache, DistributedProductCache>();
        services.AddScoped<ICatalogModuleApi, CatalogModuleApi>();

        services.AddHandlersFromAssembly(Assembly);

        return services;
    }
}
