using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Suppliers.Application.Abstractions;
using GymStore.Modules.Suppliers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Suppliers;

/// <summary>Composition entry point for the Suppliers module (registered via the common IModule mechanism).</summary>
public sealed class SuppliersModule : IModule
{
    public Assembly Assembly => typeof(SuppliersModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SuppliersDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
