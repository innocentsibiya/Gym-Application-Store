using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Shipping.Application.Features.DeliverShipment;
using GymStore.Modules.Shipping.Application.Features.GetShipmentByOrder;
using GymStore.Modules.Shipping.Application.Features.ShipOrder;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Shipping.Api;

/// <summary>Thin HTTP adapter for shipments. Discovered by the host via AddApplicationPart.</summary>
[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public ShipmentsController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetForOrder(long orderId)
    {
        var shipment = await _dispatcher.Query(new GetShipmentByOrderQuery(orderId));
        return shipment is null ? NotFound() : Ok(shipment);
    }

    [HttpPut("order/{orderId}/ship")]
    public async Task<IActionResult> Ship(long orderId, [FromBody] ShipRequest request)
    {
        var shipment = await _dispatcher.Send(
            new ShipOrderCommand(orderId, request.Carrier, request.TrackingNumber));
        return shipment is null ? NotFound() : Ok(shipment);
    }

    [HttpPut("order/{orderId}/deliver")]
    public async Task<IActionResult> Deliver(long orderId)
    {
        var shipment = await _dispatcher.Send(new DeliverShipmentCommand(orderId));
        return shipment is null ? NotFound() : Ok(shipment);
    }
}

/// <summary>Body for the ship action.</summary>
public sealed class ShipRequest
{
    public string Carrier { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
}
