using GymStore.Modules.Suppliers.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Suppliers.Infrastructure.Persistence;

/// <summary>
/// The Suppliers module's own EF Core context. Maps Supplier to the existing Suppliers table,
/// marked ExcludeFromMigrations because the DDL is owned by the host's GymStoreContext.
/// </summary>
public class SuppliersDbContext : DbContext
{
    public SuppliersDbContext(DbContextOptions<SuppliersDbContext> options) : base(options)
    {
    }

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var supplier = modelBuilder.Entity<Supplier>();
        supplier.ToTable("Suppliers", t => t.ExcludeFromMigrations());
        supplier.HasKey(s => s.Id);
    }
}
