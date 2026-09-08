using backend.Data;
using GymStore.Modules.Reviews.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace backend.Adapters
{
    /// <summary>
    /// Host-side implementation of the Reviews module's <see cref="IReviewerInfoProvider"/> port,
    /// backed by the user store in <see cref="GymStoreContext"/>. Exposes only a display name —
    /// never any user secret. Moves to a Users/Identity module when one is extracted.
    /// </summary>
    public sealed class ReviewerInfoProvider : IReviewerInfoProvider
    {
        private readonly GymStoreContext _context;

        public ReviewerInfoProvider(GymStoreContext context) => _context = context;

        public async Task<IReadOnlyDictionary<long, ReviewerInfo>> GetReviewersAsync(
            IReadOnlyCollection<long> userIds, CancellationToken ct)
        {
            if (userIds.Count == 0)
            {
                return new Dictionary<long, ReviewerInfo>();
            }

            var rows = await _context.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.FirstName, u.LastName })
                .ToListAsync(ct);

            return rows.ToDictionary(
                u => u.Id,
                u => new ReviewerInfo(u.Id, $"{u.FirstName} {u.LastName}".Trim()));
        }
    }
}
