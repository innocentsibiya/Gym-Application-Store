namespace GymStore.BuildingBlocks.Cqrs;

/// <summary>
/// Marker for a read-only request that returns <typeparamref name="TResponse"/>.
/// Queries are handled by exactly one <see cref="IQueryHandler{TQuery, TResponse}"/>.
/// </summary>
public interface IQuery<TResponse>
{
}
