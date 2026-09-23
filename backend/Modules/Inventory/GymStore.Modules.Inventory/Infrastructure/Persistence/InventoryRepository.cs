using GymStore.Modules.Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Inventory.Infrastructure.Persistence;

internal sealed class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _db;

    public InventoryRepository(InventoryDbContext db) => _db = db;

    // Tracked so the update handler can mutate and persist via SaveChangesAsync.
    public Task<Domain.Inventory?> GetByProductIdAsync(long productId, CancellationToken ct) =>
        _db.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId, ct);

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
