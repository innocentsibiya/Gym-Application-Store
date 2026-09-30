using GymStore.Modules.Identity.Domain;

namespace GymStore.Modules.Identity.Application.Abstractions;

/// <summary>Persistence operations for users (module-internal).</summary>
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task<IReadOnlyList<User>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
