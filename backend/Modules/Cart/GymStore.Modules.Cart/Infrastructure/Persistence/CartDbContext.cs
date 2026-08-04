using GymStore.Modules.Cart.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Cart.Infrastructure.Persistence;

/// <summary>
/// The Cart module's own EF Core context. It maps only Cart/CartItem and targets the
/// existing <c>Carts</c>/<c>CartItems</c> tables. Both entities are marked
/// <see cref="RelationalEntityTypeBuilderExtensions.ExcludeFromMigrations"/> because the
/// tables' DDL is still owned by the host's GymStoreContext during this first extraction
/// (see plan). This context is used for all Cart runtime reads/writes.
/// </summary>
public class CartDbContext : DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Cart> Carts => Set<Domain.Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var cart = modelBuilder.Entity<Domain.Cart>();
        cart.ToTable("Carts", t => t.ExcludeFromMigrations());
        cart.HasKey(c => c.Id);
        cart.Property(c => c.UserId);
        cart.Property(c => c.CreatedAt);
        cart.Property(c => c.UpdatedAt);
        cart.HasMany(c => c.Items)
            .WithOne()
            .HasForeignKey(i => i.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        var item = modelBuilder.Entity<CartItem>();
        item.ToTable("CartItems", t => t.ExcludeFromMigrations());
        item.HasKey(i => i.Id);
        item.Property(i => i.ProductId);
        item.Property(i => i.Quantity);
        item.Property(i => i.Price).HasColumnType("decimal(18,2)");
    }
}
