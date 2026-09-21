using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Common.Modules;

/// <summary>
/// A modular-monolith module. Each module implements this so the host can register them all
/// uniformly: <see cref="Register"/> wires the module's services, and <see cref="Assembly"/>
/// lets the host discover the module's controllers as an MVC application part.
/// </summary>
public interface IModule
{
    /// <summary>The module's assembly (for controller discovery via AddApplicationPart).</summary>
    Assembly Assembly { get; }

    /// <summary>Registers the module's services (DbContext, repositories, handlers, etc.).</summary>
    void Register(IServiceCollection services, IConfiguration configuration);
}
