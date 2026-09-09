using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Preferences.Application.Abstractions;
using GymStore.Modules.Preferences.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Preferences;

/// <summary>Composition entry point for the Preferences module (registered via the common IModule mechanism).</summary>
public sealed class PreferencesModule : IModule
{
    public Assembly Assembly => typeof(PreferencesModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PreferencesDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IPreferenceRepository, PreferenceRepository>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
