using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Catalog.Infrastructure.Caching;
using GymStore.Modules.Catalog.Infrastructure.Persistence;
using GymStore.Modules.Catalog.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Catalog;

/// <summary>Composition entry point for the Catalog module (registered via the common IModule mechanism).</summary>
public sealed class CatalogModule : IModule
{
    public Assembly Assembly => typeof(CatalogModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductCache, DistributedProductCache>();
        services.AddScoped<ICatalogModuleApi, CatalogModuleApi>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
