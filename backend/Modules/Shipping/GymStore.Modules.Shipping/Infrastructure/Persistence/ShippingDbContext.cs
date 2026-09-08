using GymStore.Modules.Shipping.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Shipping.Infrastructure.Persistence;

/// <summary>
/// The Shipping module's own EF Core context. Maps Shipment to the existing Shipments table,
/// marked ExcludeFromMigrations because the DDL is owned by the host's GymStoreContext.
/// </summary>
public class ShippingDbContext : DbContext
{
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options)
    {
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var shipment = modelBuilder.Entity<Shipment>();
        shipment.ToTable("Shipments", t => t.ExcludeFromMigrations());
        shipment.HasKey(s => s.Id);
    }
}
