namespace GymStore.BuildingBlocks.Cqrs;

/// <summary>
/// Handles a single <typeparamref name="TQuery"/>, producing <typeparamref name="TResponse"/>.
/// </summary>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken ct);
}
