using GymStore.Modules.Suppliers.Application.Abstractions;
using GymStore.Modules.Suppliers.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Suppliers.Infrastructure.Persistence;

internal sealed class SupplierRepository : ISupplierRepository
{
    private readonly SuppliersDbContext _db;

    public SupplierRepository(SuppliersDbContext db) => _db = db;

    public async Task<IReadOnlyList<Supplier>> GetAllAsync(CancellationToken ct) =>
        await _db.Suppliers.AsNoTracking().ToListAsync(ct);

    public Task<Supplier?> GetByIdAsync(long id, CancellationToken ct) =>
        _db.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
}
