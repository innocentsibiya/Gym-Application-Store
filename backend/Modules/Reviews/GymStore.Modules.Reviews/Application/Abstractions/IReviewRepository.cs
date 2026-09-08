using GymStore.Modules.Reviews.Domain;

namespace GymStore.Modules.Reviews.Application.Abstractions;

/// <summary>Persistence operations for reviews (module-internal).</summary>
public interface IReviewRepository
{
    Task<Review> AddAsync(Review review, CancellationToken ct);
    Task<IReadOnlyList<Review>> GetByProductIdAsync(long productId, CancellationToken ct);
}
