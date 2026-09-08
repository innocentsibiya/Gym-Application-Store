using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Shipping.Application.Abstractions;
using GymStore.Modules.Shipping.Contracts;
using GymStore.Modules.Shipping.Infrastructure.Persistence;
using GymStore.Modules.Shipping.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Shipping;

/// <summary>
/// Composition entry point for the Shipping module. The host calls <see cref="AddShippingModule"/>
/// and registers <see cref="Assembly"/> as an MVC application part so the module's controller is
/// discovered.
/// </summary>
public static class ShippingModuleExtensions
{
    /// <summary>The module assembly (used by the host for controller discovery).</summary>
    public static Assembly Assembly => typeof(ShippingModuleExtensions).Assembly;

    public static IServiceCollection AddShippingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ShippingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IShipmentRepository, ShipmentRepository>();
        services.AddScoped<IShippingModuleApi, ShippingModuleApi>();
        services.AddHandlersFromAssembly(Assembly);

        return services;
    }
}
