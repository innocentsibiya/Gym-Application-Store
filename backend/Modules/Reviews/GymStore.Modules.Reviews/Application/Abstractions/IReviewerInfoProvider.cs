namespace GymStore.Modules.Reviews.Application.Abstractions;

/// <summary>
/// Port the Reviews module uses to resolve reviewer display names. Consumer-defined interface
/// (DIP): the host implements it over the user store today; a future Users/Identity module could
/// implement it via its own contract — Reviews never depends on user internals or its secrets.
/// </summary>
public interface IReviewerInfoProvider
{
    Task<IReadOnlyDictionary<long, ReviewerInfo>> GetReviewersAsync(
        IReadOnlyCollection<long> userIds, CancellationToken ct);
}

public sealed record ReviewerInfo(long UserId, string Name);
