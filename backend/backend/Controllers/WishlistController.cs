using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            var wishlist = await _wishlistService.GetWishlistAsync(userId);
            return Ok(wishlist);
        }

        [HttpPost("{userId}/add/{productId}")]
        public async Task<IActionResult> AddToWishlist(int userId, int productId)
        {
            await _wishlistService.AddToWishlistAsync(userId, productId);
            return Ok(new { message = "Product added to wishlist" });
        }

        [HttpDelete("{userId}/remove/{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int userId, int productId)
        {
            await _wishlistService.RemoveFromWishlistAsync(userId, productId);
            return Ok(new { message = "Product removed from wishlist" });
        }
    }
}