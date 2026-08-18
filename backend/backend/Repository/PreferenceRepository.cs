using backend.Data;
using backend.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly GymStoreContext _context;

        public PreferenceRepository(GymStoreContext context)
        {
            _context = context;
        }

        public async Task<Preference?> GetByUserIdAsync(long userId)
        {
            return await _context.Preference
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Preference> AddOrUpdateAsync(Preference prefs)
        {
            var existing = await _context.Preference
                .FirstOrDefaultAsync(p => p.UserId == prefs.UserId);

            if (existing == null)
            {
                _context.Preference.Add(prefs);
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

            await _context.SaveChangesAsync();
            return existing ?? prefs;
        }
    }
}