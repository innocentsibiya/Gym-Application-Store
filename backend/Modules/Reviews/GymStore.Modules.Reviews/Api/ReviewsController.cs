using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Reviews.Application.Features.AddReview;
using GymStore.Modules.Reviews.Application.Features.GetProductReviews;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Reviews.Api;

/// <summary>
/// Thin HTTP adapter for reviews. Routes and payloads match the original controller; each
/// action dispatches a command/query. Discovered by the host via AddApplicationPart.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public ReviewsController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpPost("{userId}/{productId}")]
    public async Task<IActionResult> AddReview(int userId, int productId, string content, int rating)
    {
        await _dispatcher.Send(new AddReviewCommand(userId, productId, content, rating));
        return Ok(new { message = "Review added" });
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetReviews(int productId)
    {
        var reviews = await _dispatcher.Query(new GetProductReviewsQuery(productId));
        return Ok(reviews);
    }
}
