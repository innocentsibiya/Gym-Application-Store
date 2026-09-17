using GymStore.Modules.Ordering.Application.Abstractions;
using GymStore.Modules.Ordering.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Ordering.Infrastructure.Persistence;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly OrderingDbContext _db;

    public OrderRepository(OrderingDbContext db) => _db = db;

    public async Task<Order> AddAsync(Order order, CancellationToken ct)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetByUserAsync(long userId, CancellationToken ct) =>
        await _db.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Order>> GetByUserAndYearAsync(long userId, int year, CancellationToken ct) =>
        await _db.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId && o.CreatedAt.Year == year)
            .ToListAsync(ct);
}
