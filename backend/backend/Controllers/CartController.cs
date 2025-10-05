using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("{userId}/add/{productId}")]
        public async Task<IActionResult> AddToCart(int userId, int productId, int quantity)
        {
            await _cartService.AddToCartAsync(userId, productId, quantity);
            return Ok(new { message = "Product added to cart" });
        }

        [HttpDelete("{userId}/remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            await _cartService.RemoveFromCartAsync(userId, productId);
            return Ok(new { message = "Product removed from cart" });
        }
    }
}
