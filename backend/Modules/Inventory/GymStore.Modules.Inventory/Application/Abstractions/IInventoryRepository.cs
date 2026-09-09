namespace GymStore.Modules.Inventory.Application.Abstractions;

/// <summary>Persistence operations for inventory (module-internal).</summary>
public interface IInventoryRepository
{
    Task<Domain.Inventory?> GetByProductIdAsync(long productId, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
