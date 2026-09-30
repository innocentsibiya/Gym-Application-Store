using GymStore.Modules.Identity.Contracts;
using GymStore.Modules.Reviews.Application.Abstractions;

namespace backend.Adapters
{
    /// <summary>
    /// Bridges the Reviews module's <see cref="IReviewerInfoProvider"/> port to the Identity
    /// module's public <see cref="IIdentityModuleApi"/>. Composition-root wiring: Reviews depends
    /// only on its own port, Identity owns users, and the host connects the two — so no module
    /// reads the Users table directly.
    /// </summary>
    public sealed class ReviewerInfoProvider : IReviewerInfoProvider
    {
        private readonly IIdentityModuleApi _identity;

        public ReviewerInfoProvider(IIdentityModuleApi identity) => _identity = identity;

        public async Task<IReadOnlyDictionary<long, ReviewerInfo>> GetReviewersAsync(
            IReadOnlyCollection<long> userIds, CancellationToken ct)
        {
            var users = await _identity.GetUsersByIdsAsync(userIds, ct);

            return users.ToDictionary(
                kv => kv.Key,
                kv => new ReviewerInfo(kv.Value.Id, kv.Value.FullName));
        }
    }
}
