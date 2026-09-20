using GymStore.Modules.Reviews.Application.Abstractions;
using GymStore.Modules.Reviews.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Reviews.Infrastructure.Persistence;

internal sealed class ReviewRepository : IReviewRepository
{
    private readonly ReviewsDbContext _db;

    public ReviewRepository(ReviewsDbContext db) => _db = db;

    public async Task<Review> AddAsync(Review review, CancellationToken ct)
    {
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync(ct);
        return review;
    }

    public async Task<IReadOnlyList<Review>> GetByProductIdAsync(long productId, CancellationToken ct) =>
        await _db.Reviews
            .AsNoTracking()
            .Where(r => r.ProductId == productId)
            .ToListAsync(ct);
}
