using GymStore.Modules.Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Catalog.Infrastructure.Persistence;

/// <summary>
/// The Catalog module's own EF Core context. Maps Product/Category/ProductImage to the
/// existing Products/Categories/ProductImages tables, marked ExcludeFromMigrations because the
/// DDL is still owned by the host's GymStoreContext during this extraction (see plan). Used
/// for all Catalog runtime reads.
/// </summary>
public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();
        product.ToTable("Products", t => t.ExcludeFromMigrations());
        product.HasKey(p => p.Id);
        product.Property(p => p.Price).HasColumnType("decimal(18,2)");
        product.Property(p => p.DiscountPrice).HasColumnType("decimal(18,2)");
        product.Property(p => p.Weight).HasColumnType("decimal(10,2)");
        product.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
        product.HasMany(p => p.Images)
            .WithOne()
            .HasForeignKey(i => i.ProductId);

        var category = modelBuilder.Entity<Category>();
        category.ToTable("Categories", t => t.ExcludeFromMigrations());
        category.HasKey(c => c.Id);
        category.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId);

        var image = modelBuilder.Entity<ProductImage>();
        image.ToTable("ProductImages", t => t.ExcludeFromMigrations());
        image.HasKey(i => i.Id);
    }
}
