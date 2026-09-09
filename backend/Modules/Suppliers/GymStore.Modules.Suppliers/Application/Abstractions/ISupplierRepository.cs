using GymStore.Modules.Suppliers.Domain;

namespace GymStore.Modules.Suppliers.Application.Abstractions;

/// <summary>Read operations for suppliers (module-internal).</summary>
public interface ISupplierRepository
{
    Task<IReadOnlyList<Supplier>> GetAllAsync(CancellationToken ct);
    Task<Supplier?> GetByIdAsync(long id, CancellationToken ct);
}
