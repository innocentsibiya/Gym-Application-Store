using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Payments.Application.Features.GetPaymentByOrder;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Payments.Api;

/// <summary>Thin HTTP adapter for payments. Discovered by the host via AddApplicationPart.</summary>
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public PaymentsController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetPaymentForOrder(long orderId)
    {
        var payment = await _dispatcher.Query(new GetPaymentByOrderQuery(orderId));
        return payment is null ? NotFound() : Ok(payment);
    }
}
