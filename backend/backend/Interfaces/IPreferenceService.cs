using backend.Models;

namespace backend.Interfaces
{
    public interface IPreferenceService
    {
        Task<Preference> GetPreferencesAsync(long userId);
        Task<Preference> SavePreferencesAsync(Preference prefs);
    }
}