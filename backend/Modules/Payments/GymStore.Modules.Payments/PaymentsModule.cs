using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Payments.Application.Abstractions;
using GymStore.Modules.Payments.Contracts;
using GymStore.Modules.Payments.Infrastructure.Persistence;
using GymStore.Modules.Payments.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Payments;

/// <summary>Composition entry point for the Payments module (registered via the common IModule mechanism).</summary>
public sealed class PaymentsModule : IModule
{
    public Assembly Assembly => typeof(PaymentsModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentModuleApi, PaymentModuleApi>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
