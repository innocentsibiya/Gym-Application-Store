using GymStore.Modules.Reviews.Application.Abstractions;
using GymStore.Modules.Reviews.Application.Features.AddReview;
using GymStore.Modules.Reviews.Application.Features.GetProductReviews;
using GymStore.Modules.Reviews.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Reviews.Tests;

public class ReviewsTests
{
    private static ReviewsDbContext NewContext() =>
        new(new DbContextOptionsBuilder<ReviewsDbContext>()
            .UseInMemoryDatabase($"reviews-tests-{Guid.NewGuid()}")
            .Options);

    [Fact]
    public async Task AddReview_PersistsReview_AndReturnsId()
    {
        await using var ctx = NewContext();
        var handler = new AddReviewCommandHandler(new ReviewRepository(ctx));

        var id = await handler.Handle(new AddReviewCommand(1, 5, "Great product", 4), CancellationToken.None);

        Assert.True(id > 0);
        var review = await ctx.Reviews.SingleAsync();
        Assert.Equal(1, review.UserId);
        Assert.Equal(5, review.ProductId);
        Assert.Equal(4, review.Rating);
        Assert.Equal("Great product", review.Comment);
        Assert.NotEqual(default, review.CreatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public async Task AddReview_InvalidRating_Throws(int rating)
    {
        await using var ctx = NewContext();
        var handler = new AddReviewCommandHandler(new ReviewRepository(ctx));

        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(new AddReviewCommand(1, 5, "x", rating), CancellationToken.None));

        Assert.Empty(await ctx.Reviews.ToListAsync());
    }

    [Fact]
    public async Task GetProductReviews_ReturnsOnlyThatProduct_EnrichedWithReviewerName()
    {
        await using var ctx = NewContext();
        var repo = new ReviewRepository(ctx);
        var add = new AddReviewCommandHandler(repo);
        await add.Handle(new AddReviewCommand(1, 5, "For product 5", 5), CancellationToken.None);
        await add.Handle(new AddReviewCommand(2, 5, "Also product 5", 3), CancellationToken.None);
        await add.Handle(new AddReviewCommand(1, 9, "Different product", 4), CancellationToken.None);

        var reviewers = new FakeReviewerInfoProvider(
            new ReviewerInfo(1, "Ada Lovelace"),
            new ReviewerInfo(2, "Alan Turing"));
        var handler = new GetProductReviewsQueryHandler(repo, reviewers);

        var result = await handler.Handle(new GetProductReviewsQuery(5), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result, r => Assert.Equal(5, r.ProductId));
        Assert.Equal("Ada Lovelace", result.Single(r => r.UserId == 1).ReviewerName);
        Assert.Equal("Alan Turing", result.Single(r => r.UserId == 2).ReviewerName);
    }

    private sealed class FakeReviewerInfoProvider : IReviewerInfoProvider
    {
        private readonly Dictionary<long, ReviewerInfo> _reviewers;

        public FakeReviewerInfoProvider(params ReviewerInfo[] reviewers) =>
            _reviewers = reviewers.ToDictionary(r => r.UserId);

        public Task<IReadOnlyDictionary<long, ReviewerInfo>> GetReviewersAsync(
            IReadOnlyCollection<long> userIds, CancellationToken ct)
        {
            IReadOnlyDictionary<long, ReviewerInfo> result = _reviewers
                .Where(kv => userIds.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            return Task.FromResult(result);
        }
    }
}
