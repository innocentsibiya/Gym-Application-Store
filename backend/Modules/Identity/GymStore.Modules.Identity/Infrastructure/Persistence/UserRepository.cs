using GymStore.Modules.Identity.Application.Abstractions;
using GymStore.Modules.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Identity.Infrastructure.Persistence;

internal sealed class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _db;

    public UserRepository(IdentityDbContext db) => _db = db;

    // Tracked so login can update LastLoginAt / upgrade the password hash on SaveChanges.
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct) =>
        _db.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<IReadOnlyList<User>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken ct) =>
        await _db.Users.AsNoTracking().Where(u => ids.Contains(u.Id)).ToListAsync(ct);

    public async Task AddAsync(User user, CancellationToken ct) => await _db.Users.AddAsync(user, ct);

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
