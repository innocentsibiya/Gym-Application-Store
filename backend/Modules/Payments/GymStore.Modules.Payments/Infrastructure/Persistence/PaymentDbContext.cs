using GymStore.Modules.Payments.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Payments.Infrastructure.Persistence;

/// <summary>
/// The Payments module's own EF Core context. Maps Payment to the existing Payments table,
/// marked ExcludeFromMigrations because the DDL is owned by the host's GymStoreContext. The
/// table's unique index on OrderId enforces one payment per order.
/// </summary>
public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
    }

    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var payment = modelBuilder.Entity<Payment>();
        payment.ToTable("Payments", t => t.ExcludeFromMigrations());
        payment.HasKey(p => p.Id);
        payment.Property(p => p.Amount).HasColumnType("decimal(18,2)");
    }
}
