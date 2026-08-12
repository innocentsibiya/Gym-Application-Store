using backend.Interfaces;
using backend.IRepository;
using backend.Models;

namespace backend.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IPreferenceRepository _repository;

        public PreferenceService(IPreferenceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Preference> GetPreferencesAsync(long userId)
        {
            var prefs = await _repository.GetByUserIdAsync(userId);
            return prefs ?? new Preference { UserId = userId };
        }

        public async Task<Preference> SavePreferencesAsync(Preference prefs)
        {
            return await _repository.AddOrUpdateAsync(prefs);
        }
    }
}
