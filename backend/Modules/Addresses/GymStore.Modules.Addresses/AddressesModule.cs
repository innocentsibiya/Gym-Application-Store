using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Addresses.Application.Abstractions;
using GymStore.Modules.Addresses.Infrastructure.Caching;
using GymStore.Modules.Addresses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Addresses;

/// <summary>Composition entry point for the Addresses module (registered via the common IModule mechanism).</summary>
public sealed class AddressesModule : IModule
{
    public Assembly Assembly => typeof(AddressesModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AddressesDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAddressCache, DistributedAddressCache>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
