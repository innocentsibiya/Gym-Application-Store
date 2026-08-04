namespace GymStore.BuildingBlocks.Cqrs;

/// <summary>
/// Entry point that routes a command or query to its registered handler.
/// Controllers and cross-module callers depend on this abstraction rather than
/// on concrete handlers.
/// </summary>
public interface IDispatcher
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
    Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);
}
