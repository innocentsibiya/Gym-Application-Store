using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.BuildingBlocks.Cqrs;

/// <summary>
/// Default <see cref="IDispatcher"/>. Resolves the closed-generic handler for the runtime
/// type of the command/query from the DI container and invokes it through the handler
/// interface's <c>Handle</c> method. Invoking via the (public) interface method — rather than
/// <c>dynamic</c> — lets handler implementations stay <c>internal</c> to their module.
/// </summary>
internal sealed class Dispatcher : IDispatcher
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> HandleMethods = new();

    private readonly IServiceProvider _provider;

    public Dispatcher(IServiceProvider provider) => _provider = provider;

    public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        return Invoke<TResponse>(handlerType, command, ct);
    }

    public Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        return Invoke<TResponse>(handlerType, query, ct);
    }

    private Task<TResponse> Invoke<TResponse>(Type handlerType, object message, CancellationToken ct)
    {
        var handler = _provider.GetRequiredService(handlerType);
        var handle = HandleMethods.GetOrAdd(handlerType, t => t.GetMethod("Handle")!);
        return (Task<TResponse>)handle.Invoke(handler, new[] { message, ct })!;
    }
}
