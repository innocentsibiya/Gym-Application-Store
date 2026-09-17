using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Ordering.Api.Requests;
using GymStore.Modules.Ordering.Application.Features.GetUserOrders;
using GymStore.Modules.Ordering.Application.Features.GetUserOrdersByYear;
using GymStore.Modules.Ordering.Application.Features.PlaceOrder;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Ordering.Api;

/// <summary>
/// Thin HTTP adapter for ordering. Routes and payloads are identical to the original controller;
/// each action dispatches a command/query. Discovered by the host via AddApplicationPart.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public OrdersController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpPost("place")]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        var order = await _dispatcher.Send(
            new PlaceOrderCommand(request.UserId, request.ShippingAddressId, request.BillingAddressId, request.Method));
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserOrders(int userId)
    {
        var orders = await _dispatcher.Query(new GetUserOrdersQuery(userId));
        return Ok(orders);
    }

    [HttpGet("user/{userId}/year/{year}")]
    public async Task<IActionResult> GetUserOrdersByYear(int userId, int year)
    {
        var orders = await _dispatcher.Query(new GetUserOrdersByYearQuery(userId, year));
        return Ok(orders);
    }
}
