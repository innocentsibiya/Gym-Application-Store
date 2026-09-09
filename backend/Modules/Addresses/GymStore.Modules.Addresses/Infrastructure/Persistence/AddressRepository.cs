using GymStore.Modules.Addresses.Application.Abstractions;
using GymStore.Modules.Addresses.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Addresses.Infrastructure.Persistence;

internal sealed class AddressRepository : IAddressRepository
{
    private readonly AddressesDbContext _db;

    public AddressRepository(AddressesDbContext db) => _db = db;

    // Tracked (no AsNoTracking) so mutations to IsDefault persist on SaveChanges.
    public async Task<IReadOnlyList<Address>> GetByUserIdAsync(long userId, CancellationToken ct) =>
        await _db.Addresses.Where(a => a.UserId == userId).ToListAsync(ct);

    public Task<Address?> GetDefaultAsync(long userId, string type, CancellationToken ct) =>
        _db.Addresses.FirstOrDefaultAsync(a => a.UserId == userId && a.AddressType == type && a.IsDefault, ct);

    public Task<Address?> GetByIdAsync(long id, CancellationToken ct) =>
        _db.Addresses.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Address address, CancellationToken ct) =>
        await _db.Addresses.AddAsync(address, ct);

    public void Remove(Address address) => _db.Addresses.Remove(address);

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
