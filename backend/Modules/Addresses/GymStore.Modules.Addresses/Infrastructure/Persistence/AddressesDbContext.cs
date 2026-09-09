using GymStore.Modules.Addresses.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Addresses.Infrastructure.Persistence;

/// <summary>
/// The Addresses module's own EF Core context. Maps Address to the existing Addresses table,
/// marked ExcludeFromMigrations because the DDL is owned by the host's GymStoreContext.
/// </summary>
public class AddressesDbContext : DbContext
{
    public AddressesDbContext(DbContextOptions<AddressesDbContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses => Set<Address>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var address = modelBuilder.Entity<Address>();
        address.ToTable("Addresses", t => t.ExcludeFromMigrations());
        address.HasKey(a => a.Id);
    }
}
