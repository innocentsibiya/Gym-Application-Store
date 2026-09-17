using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Payments.Application.Abstractions;
using GymStore.Modules.Payments.Contracts;
using GymStore.Modules.Payments.Infrastructure.Persistence;
using GymStore.Modules.Payments.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Payments;

/// <summary>
/// Composition entry point for the Payments module. The host calls <see cref="AddPaymentsModule"/>
/// and registers <see cref="Assembly"/> as an MVC application part so the module's controller is
/// discovered.
/// </summary>
public static class PaymentsModuleExtensions
{
    /// <summary>The module assembly (used by the host for controller discovery).</summary>
    public static Assembly Assembly => typeof(PaymentsModuleExtensions).Assembly;

    public static IServiceCollection AddPaymentsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentModuleApi, PaymentModuleApi>();
        services.AddHandlersFromAssembly(Assembly);

        return services;
    }
}
