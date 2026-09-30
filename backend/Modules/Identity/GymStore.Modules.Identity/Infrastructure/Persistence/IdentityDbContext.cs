using GymStore.Modules.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Identity.Infrastructure.Persistence;

/// <summary>
/// The Identity module's own EF Core context. Maps User to the existing Users table, marked
/// ExcludeFromMigrations because the DDL is owned by the host's GymStoreContext.
/// </summary>
public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();
        user.ToTable("Users", t => t.ExcludeFromMigrations());
        user.HasKey(u => u.Id);
    }
}
