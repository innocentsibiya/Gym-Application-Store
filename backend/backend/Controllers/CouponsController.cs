using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet("validate/{code}/{amount}")]
        public async Task<IActionResult> ValidateCoupon(string code, decimal amount)
        {
            var coupon = await _couponService.ValidateCouponAsync(code, amount);
            return coupon == null ? NotFound("Invalid or expired coupon") : Ok(coupon);
        }
    }
}