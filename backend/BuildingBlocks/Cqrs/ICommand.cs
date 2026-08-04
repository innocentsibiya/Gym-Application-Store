namespace GymStore.BuildingBlocks.Cqrs;

/// <summary>
/// Marker for a request that changes state and returns <typeparamref name="TResponse"/>.
/// Commands are handled by exactly one <see cref="ICommandHandler{TCommand, TResponse}"/>.
/// </summary>
public interface ICommand<TResponse>
{
}
