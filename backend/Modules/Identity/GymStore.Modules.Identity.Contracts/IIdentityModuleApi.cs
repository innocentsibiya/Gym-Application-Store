namespace GymStore.Modules.Identity.Contracts;

/// <summary>
/// The Identity module's public surface for other modules (e.g. Reviews resolving reviewer names).
/// Exposes only non-sensitive user summary data — never password hashes or auth material.
/// </summary>
public interface IIdentityModuleApi
{
    Task<IReadOnlyDictionary<long, UserSummaryDto>> GetUsersByIdsAsync(
        IReadOnlyCollection<long> userIds, CancellationToken ct = default);
}

/// <summary>Minimal, non-sensitive user projection other modules may consume.</summary>
public sealed record UserSummaryDto(long Id, string FullName, string Email);
