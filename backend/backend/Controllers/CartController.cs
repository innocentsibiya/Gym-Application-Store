using backend.Interfaces;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        // Inject the CartService into the controller
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Get the cart items
        [HttpGet]
        public ActionResult<List<CartItem>> GetCart()
        {
            var cartItems = _cartService.GetCart();
            return Ok(cartItems);
        }

        // Add an item to the cart
        [HttpPost]
        public ActionResult AddToCart([FromBody] CartItem newItem)
        {
            var updatedCart = _cartService.AddToCart(newItem);
            return Ok(updatedCart);
        }

        // Remove an item from the cart
        [HttpDelete("{id}")]
        public ActionResult RemoveFromCart(int id)
        {
            var updatedCart = _cartService.RemoveFromCart(id);
            if (updatedCart == null)
            {
                return NotFound("Item not found in cart");
            }
            return Ok(updatedCart);
        }
    }
}
