using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Inventory.Infrastructure.Persistence;

/// <summary>
/// The Inventory module's own EF Core context. Maps Inventory to the existing Inventories table,
/// marked ExcludeFromMigrations because the DDL is owned by the host's GymStoreContext.
/// </summary>
public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Inventory> Inventories => Set<Domain.Inventory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var inventory = modelBuilder.Entity<Domain.Inventory>();
        inventory.ToTable("Inventories", t => t.ExcludeFromMigrations());
        inventory.HasKey(i => i.Id);
    }
}
