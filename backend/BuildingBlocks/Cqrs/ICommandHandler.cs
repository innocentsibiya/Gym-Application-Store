namespace GymStore.BuildingBlocks.Cqrs;

/// <summary>
/// Handles a single <typeparamref name="TCommand"/>, producing <typeparamref name="TResponse"/>.
/// </summary>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken ct);
}
