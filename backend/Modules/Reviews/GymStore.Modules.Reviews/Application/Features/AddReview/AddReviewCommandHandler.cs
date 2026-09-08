using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Reviews.Application.Abstractions;
using GymStore.Modules.Reviews.Domain;

namespace GymStore.Modules.Reviews.Application.Features.AddReview;

internal sealed class AddReviewCommandHandler : ICommandHandler<AddReviewCommand, long>
{
    private readonly IReviewRepository _repository;

    public AddReviewCommandHandler(IReviewRepository repository) => _repository = repository;

    public async Task<long> Handle(AddReviewCommand command, CancellationToken ct)
    {
        if (command.Rating < 1 || command.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5.");
        }

        var review = new Review
        {
            UserId = command.UserId,
            ProductId = command.ProductId,
            Comment = command.Content,
            Rating = command.Rating,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(review, ct);
        return review.Id;
    }
}
