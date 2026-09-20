using GymStore.BuildingBlocks.Cqrs;

namespace GymStore.Modules.Reviews.Application.Features.AddReview;

/// <summary>Adds a product review. Returns the new review id.</summary>
public sealed record AddReviewCommand(long UserId, long ProductId, string? Content, int Rating)
    : ICommand<long>;
