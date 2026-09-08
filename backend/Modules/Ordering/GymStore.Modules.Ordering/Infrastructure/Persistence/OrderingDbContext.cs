using GymStore.Modules.Ordering.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Ordering.Infrastructure.Persistence;

/// <summary>
/// The Ordering module's own EF Core context. Maps the order aggregate to the existing
/// Orders/OrderItems tables, marked ExcludeFromMigrations because the DDL is still owned by the
/// host's GymStoreContext during this extraction (see plan). Payments and Shipments are owned by
/// their own modules.
/// </summary>
public class OrderingDbContext : DbContext
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

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

        var item = modelBuilder.Entity<OrderItem>();
        item.ToTable("OrderItems", t => t.ExcludeFromMigrations());
        item.HasKey(i => i.Id);
        item.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
        item.Property(i => i.DiscountApplied).HasColumnType("decimal(18,2)");
    }
}
