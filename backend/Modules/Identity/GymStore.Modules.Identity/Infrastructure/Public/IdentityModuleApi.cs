using GymStore.Modules.Identity.Application.Abstractions;
using GymStore.Modules.Identity.Contracts;

namespace GymStore.Modules.Identity.Infrastructure.Public;

/// <summary>
/// Implements the cross-module contract. Exposes only non-sensitive user summary data.
/// </summary>
internal sealed class IdentityModuleApi : IIdentityModuleApi
{
    private readonly IUserRepository _users;

    public IdentityModuleApi(IUserRepository users) => _users = users;

    public async Task<IReadOnlyDictionary<long, UserSummaryDto>> GetUsersByIdsAsync(
        IReadOnlyCollection<long> userIds, CancellationToken ct = default)
    {
        if (userIds.Count == 0)
        {
            return new Dictionary<long, UserSummaryDto>();
        }

        var users = await _users.GetByIdsAsync(userIds, ct);

        return users.ToDictionary(
            u => u.Id,
            u => new UserSummaryDto(u.Id, $"{u.FirstName} {u.LastName}".Trim(), u.Email));
    }
}
