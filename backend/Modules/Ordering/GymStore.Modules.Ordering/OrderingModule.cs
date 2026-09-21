using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Ordering.Application.Abstractions;
using GymStore.Modules.Ordering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Ordering;

/// <summary>
/// Composition entry point for the Ordering module (registered via the common IModule mechanism).
/// Cart/Catalog/Payments/Shipping contracts are supplied by their own modules' registrations.
/// </summary>
public sealed class OrderingModule : IModule
{
    public Assembly Assembly => typeof(OrderingModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
