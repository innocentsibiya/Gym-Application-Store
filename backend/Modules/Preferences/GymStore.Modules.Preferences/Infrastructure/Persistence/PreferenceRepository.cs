using GymStore.Modules.Preferences.Application.Abstractions;
using GymStore.Modules.Preferences.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Preferences.Infrastructure.Persistence;

internal sealed class PreferenceRepository : IPreferenceRepository
{
    private readonly PreferencesDbContext _db;

    public PreferenceRepository(PreferencesDbContext db) => _db = db;

    public Task<Preference?> GetByUserIdAsync(long userId, CancellationToken ct) =>
        _db.Preferences.FirstOrDefaultAsync(p => p.UserId == userId, ct);

    public async Task<Preference> AddOrUpdateAsync(Preference prefs, CancellationToken ct)
    {
        var existing = await _db.Preferences.FirstOrDefaultAsync(p => p.UserId == prefs.UserId, ct);

        if (existing is null)
        {
            _db.Preferences.Add(prefs);
        }
        else
        {
            existing.Theme = prefs.Theme;
            existing.ItemsPerPage = prefs.ItemsPerPage;
            existing.SortOrder = prefs.SortOrder;
            existing.Currency = prefs.Currency;
            existing.Language = prefs.Language;
            existing.Notifications = prefs.Notifications;
        }

        await _db.SaveChangesAsync(ct);
        return existing ?? prefs;
    }
}
