using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Reviews.Application.Responses;

namespace GymStore.Modules.Reviews.Application.Features.GetProductReviews;

public sealed record GetProductReviewsQuery(long ProductId) : IQuery<IReadOnlyList<ReviewResponse>>;
