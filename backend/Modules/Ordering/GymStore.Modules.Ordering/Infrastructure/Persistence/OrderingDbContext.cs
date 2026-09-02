using GymStore.Modules.Ordering.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Ordering.Infrastructure.Persistence;

/// <summary>
/// The Ordering module's own EF Core context. Maps the order aggregate to the existing
/// Orders/OrderItems/Payments/Shipments tables, marked ExcludeFromMigrations because the DDL is
/// still owned by the host's GymStoreContext during this extraction (see plan).
/// </summary>
public class OrderingDbContext : DbContext
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Shipment> Shipments => Set<Shipment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<Order>();
        order.ToTable("Orders", t => t.ExcludeFromMigrations());
        order.HasKey(o => o.Id);
        order.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
        order.Property(o => o.Tax).HasColumnType("decimal(18,2)");
        order.Property(o => o.ShippingCost).HasColumnType("decimal(18,2)");
        order.Property(o => o.Discount).HasColumnType("decimal(18,2)");
        order.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
        order.HasMany(o => o.Items).WithOne().HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
        order.HasOne(o => o.Payment).WithOne().HasForeignKey<Payment>(p => p.OrderId).OnDelete(DeleteBehavior.Cascade);
        order.HasMany(o => o.Shipments).WithOne().HasForeignKey(s => s.OrderId).OnDelete(DeleteBehavior.Cascade);

        var item = modelBuilder.Entity<OrderItem>();
        item.ToTable("OrderItems", t => t.ExcludeFromMigrations());
        item.HasKey(i => i.Id);
        item.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
        item.Property(i => i.DiscountApplied).HasColumnType("decimal(18,2)");

        var payment = modelBuilder.Entity<Payment>();
        payment.ToTable("Payments", t => t.ExcludeFromMigrations());
        payment.HasKey(p => p.Id);
        payment.Property(p => p.Amount).HasColumnType("decimal(18,2)");

        var shipment = modelBuilder.Entity<Shipment>();
        shipment.ToTable("Shipments", t => t.ExcludeFromMigrations());
        shipment.HasKey(s => s.Id);
    }
}
