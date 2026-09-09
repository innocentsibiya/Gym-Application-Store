using GymStore.Modules.Wishlist.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Wishlist.Infrastructure.Persistence;

/// <summary>
/// The Wishlist module's own EF Core context. Maps Wishlist/WishlistItem to the existing
/// Wishlists/WishlistItems tables, marked ExcludeFromMigrations because the DDL is owned by the
/// host's GymStoreContext.
/// </summary>
public class WishlistDbContext : DbContext
{
    public WishlistDbContext(DbContextOptions<WishlistDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Wishlist> Wishlists => Set<Domain.Wishlist>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var wishlist = modelBuilder.Entity<Domain.Wishlist>();
        wishlist.ToTable("Wishlists", t => t.ExcludeFromMigrations());
        wishlist.HasKey(w => w.Id);
        wishlist.HasMany(w => w.Items)
            .WithOne()
            .HasForeignKey(i => i.WishlistId)
            .OnDelete(DeleteBehavior.Cascade);

        var item = modelBuilder.Entity<WishlistItem>();
        item.ToTable("WishlistItems", t => t.ExcludeFromMigrations());
        item.HasKey(i => i.Id);
    }
}
