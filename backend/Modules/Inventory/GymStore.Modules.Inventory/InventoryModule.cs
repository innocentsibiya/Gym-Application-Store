using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Inventory.Application.Abstractions;
using GymStore.Modules.Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Inventory;

/// <summary>Composition entry point for the Inventory module (registered via the common IModule mechanism).</summary>
public sealed class InventoryModule : IModule
{
    public Assembly Assembly => typeof(InventoryModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
