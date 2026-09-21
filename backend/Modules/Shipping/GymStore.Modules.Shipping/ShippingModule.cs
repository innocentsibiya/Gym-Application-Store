using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Shipping.Application.Abstractions;
using GymStore.Modules.Shipping.Contracts;
using GymStore.Modules.Shipping.Infrastructure.Persistence;
using GymStore.Modules.Shipping.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Shipping;

/// <summary>Composition entry point for the Shipping module (registered via the common IModule mechanism).</summary>
public sealed class ShippingModule : IModule
{
    public Assembly Assembly => typeof(ShippingModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ShippingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IShipmentRepository, ShipmentRepository>();
        services.AddScoped<IShippingModuleApi, ShippingModuleApi>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
