using GymStore.Modules.Ordering.Domain;

namespace GymStore.Modules.Ordering.Application.Abstractions;

/// <summary>Persistence operations for the Order aggregate (module-internal).</summary>
public interface IOrderRepository
{
    Task<Order> AddAsync(Order order, CancellationToken ct);
    Task<IReadOnlyList<Order>> GetByUserAsync(long userId, CancellationToken ct);
    Task<IReadOnlyList<Order>> GetByUserAndYearAsync(long userId, int year, CancellationToken ct);
}
