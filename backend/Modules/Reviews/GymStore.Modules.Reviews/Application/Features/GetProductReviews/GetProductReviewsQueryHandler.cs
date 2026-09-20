using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Reviews.Application.Abstractions;
using GymStore.Modules.Reviews.Application.Responses;

namespace GymStore.Modules.Reviews.Application.Features.GetProductReviews;

/// <summary>Returns a product's reviews, each enriched with a safe reviewer name via the port.</summary>
internal sealed class GetProductReviewsQueryHandler
    : IQueryHandler<GetProductReviewsQuery, IReadOnlyList<ReviewResponse>>
{
    private readonly IReviewRepository _repository;
    private readonly IReviewerInfoProvider _reviewers;

    public GetProductReviewsQueryHandler(IReviewRepository repository, IReviewerInfoProvider reviewers)
    {
        _repository = repository;
        _reviewers = reviewers;
    }

    public async Task<IReadOnlyList<ReviewResponse>> Handle(GetProductReviewsQuery query, CancellationToken ct)
    {
        var reviews = await _repository.GetByProductIdAsync(query.ProductId, ct);
        if (reviews.Count == 0)
        {
            return Array.Empty<ReviewResponse>();
        }

        var userIds = reviews.Select(r => r.UserId).Distinct().ToArray();
        var reviewers = await _reviewers.GetReviewersAsync(userIds, ct);

        return reviews.Select(r =>
        {
            reviewers.TryGetValue(r.UserId, out var info);
            return ReviewResponseFactory.ToResponse(r, info?.Name ?? string.Empty);
        }).ToList();
    }
}
