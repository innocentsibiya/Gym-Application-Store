using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Ordering.Application.Abstractions;
using GymStore.Modules.Ordering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Ordering;

/// <summary>
/// Composition entry point for the Ordering module. The host calls <see cref="AddOrderingModule"/>
/// and registers <see cref="Assembly"/> as an MVC application part so the module's controller is
/// discovered. Cart/Catalog contracts are supplied by their own modules' registrations.
/// </summary>
public static class OrderingModuleExtensions
{
    /// <summary>The module assembly (used by the host for controller discovery).</summary>
    public static Assembly Assembly => typeof(OrderingModuleExtensions).Assembly;

    public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddHandlersFromAssembly(Assembly);

        return services;
    }
}
