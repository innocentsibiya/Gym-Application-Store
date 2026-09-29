using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Wishlist.Application.Features.AddToWishlist;
using GymStore.Modules.Wishlist.Application.Features.GetWishlist;
using GymStore.Modules.Wishlist.Application.Features.RemoveFromWishlist;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Wishlist.Api;

/// <summary>
/// Thin HTTP adapter for the Wishlist module. Routes and payloads match the original controller;
/// each action dispatches a command/query. Discovered by the host via AddApplicationPart.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WishlistController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public WishlistController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetWishlist(int userId)
    {
        var wishlist = await _dispatcher.Query(new GetWishlistQuery(userId));
        return Ok(wishlist);
    }

    [HttpPost("{userId}/add/{productId}")]
    public async Task<IActionResult> AddToWishlist(int userId, int productId)
    {
        await _dispatcher.Send(new AddToWishlistCommand(userId, productId));
        return Ok(new { message = "Product added to wishlist" });
    }

    [HttpDelete("{userId}/remove/{productId}")]
    public async Task<IActionResult> RemoveFromWishlist(int userId, int productId)
    {
        await _dispatcher.Send(new RemoveFromWishlistCommand(userId, productId));
        return Ok(new { message = "Product removed from wishlist" });
    }
}
