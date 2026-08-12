using backend.Models;

namespace backend.IRepository
{
    public interface IPreferenceRepository
    {
        Task<Preference?> GetByUserIdAsync(long userId);
        Task<Preference> AddOrUpdateAsync(Preference prefs);
    }
}