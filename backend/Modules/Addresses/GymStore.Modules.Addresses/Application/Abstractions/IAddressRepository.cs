using GymStore.Modules.Addresses.Domain;

namespace GymStore.Modules.Addresses.Application.Abstractions;

/// <summary>Persistence operations for addresses (module-internal). Entities are tracked so
/// default-flag changes persist on SaveChanges.</summary>
public interface IAddressRepository
{
    Task<IReadOnlyList<Address>> GetByUserIdAsync(long userId, CancellationToken ct);
    Task<Address?> GetDefaultAsync(long userId, string type, CancellationToken ct);
    Task<Address?> GetByIdAsync(long id, CancellationToken ct);
    Task AddAsync(Address address, CancellationToken ct);
    void Remove(Address address);
    Task SaveChangesAsync(CancellationToken ct);
}
