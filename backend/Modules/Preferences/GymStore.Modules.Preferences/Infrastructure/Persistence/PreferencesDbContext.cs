using GymStore.Modules.Preferences.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Preferences.Infrastructure.Persistence;

/// <summary>
/// The Preferences module's own EF Core context. Maps Preference to the existing (singular)
/// Preference table, marked ExcludeFromMigrations because the DDL is owned by the host's
/// GymStoreContext.
/// </summary>
public class PreferencesDbContext : DbContext
{
    public PreferencesDbContext(DbContextOptions<PreferencesDbContext> options) : base(options)
    {
    }

    public DbSet<Preference> Preferences => Set<Preference>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var preference = modelBuilder.Entity<Preference>();
        preference.ToTable("Preference", t => t.ExcludeFromMigrations());
        preference.HasKey(p => p.Id);
    }
}
