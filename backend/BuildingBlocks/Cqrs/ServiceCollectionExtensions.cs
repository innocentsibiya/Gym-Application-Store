using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.BuildingBlocks.Cqrs;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers the CQRS dispatcher. Call once at composition root.</summary>
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
        return services;
    }

    /// <summary>
    /// Scans <paramref name="assembly"/> and registers every concrete
    /// <see cref="ICommandHandler{TCommand, TResponse}"/> and
    /// <see cref="IQueryHandler{TQuery, TResponse}"/> against its closed interface.
    /// Each module calls this for its own assembly.
    /// </summary>
    public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var openHandlerInterfaces = new[] { typeof(ICommandHandler<,>), typeof(IQueryHandler<,>) };

        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            var closedHandlerInterfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && openHandlerInterfaces.Contains(i.GetGenericTypeDefinition()));

            foreach (var handlerInterface in closedHandlerInterfaces)
            {
                services.AddScoped(handlerInterface, type);
            }
        }

        return services;
    }
}
