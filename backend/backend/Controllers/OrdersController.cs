using backend.DTO;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            var order = await _orderService.PlaceOrderAsync(
                request.UserId,
                request.ShippingAddressId,
                request.BillingAddressId
            );
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("user/{userId}/year/{year}")]
        public async Task<IActionResult> GetUserOrdersByYear(int userId, int year)
        {
            var orders = await _orderService.GetUserOrdersByYearAsync(userId, year);
            return Ok(orders);
        }
    }
}