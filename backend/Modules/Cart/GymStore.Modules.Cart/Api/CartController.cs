using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Application.Features.AddItem;
using GymStore.Modules.Cart.Application.Features.GetCart;
using GymStore.Modules.Cart.Application.Features.RemoveItem;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Cart.Api;

/// <summary>
/// Thin HTTP adapter for the Cart module. Routes and payloads are identical to the original
/// controller; each action just dispatches a command/query. Discovered by the host via
/// AddApplicationPart.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public CartController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(int userId, CancellationToken ct)
    {
        var cart = await _dispatcher.Query(new GetCartQuery(userId), ct);
        return Ok(cart);
    }

    [HttpPost("{userId}/add/{productId}")]
    public async Task<IActionResult> AddToCart(int userId, int productId, int quantity, CancellationToken ct)
    {
        var cart = await _dispatcher.Send(new AddItemToCartCommand(userId, productId, quantity), ct);
        return Ok(cart);
    }

    [HttpDelete("{userId}/remove/{productId}")]
    public async Task<IActionResult> RemoveFromCart(int userId, int productId, CancellationToken ct)
    {
        await _dispatcher.Send(new RemoveItemFromCartCommand(userId, productId), ct);
        return Ok(new { message = "Product removed from cart" });
    }
}
