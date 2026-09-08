using GymStore.Modules.Reviews.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Reviews.Infrastructure.Persistence;

/// <summary>
/// The Reviews module's own EF Core context. Maps Review to the existing Reviews table, marked
/// ExcludeFromMigrations because the DDL is owned by the host's GymStoreContext.
/// </summary>
public class ReviewsDbContext : DbContext
{
    public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options) : base(options)
    {
    }

    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var review = modelBuilder.Entity<Review>();
        review.ToTable("Reviews", t => t.ExcludeFromMigrations());
        review.HasKey(r => r.Id);
    }
}
