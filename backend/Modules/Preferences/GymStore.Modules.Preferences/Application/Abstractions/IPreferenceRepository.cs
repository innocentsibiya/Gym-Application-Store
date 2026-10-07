namespace GymStore.Modules.Preferences.Application.Abstractions;

/// <summary>Persistence operations for preferences (module-internal).</summary>
public interface IPreferenceRepository
{
    Task<Domain.Preference?> GetByUserIdAsync(long userId, CancellationToken ct);
    Task<Domain.Preference> AddOrUpdateAsync(Domain.Preference prefs, CancellationToken ct);
}
