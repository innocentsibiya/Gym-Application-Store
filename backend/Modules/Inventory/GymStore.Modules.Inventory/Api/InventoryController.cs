using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Inventory.Application.Features.GetStock;
using GymStore.Modules.Inventory.Application.Features.UpdateStock;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Inventory.Api;

/// <summary>
/// Thin HTTP adapter for inventory. Routes and payloads match the original controller; each
/// action dispatches a command/query. Discovered by the host via AddApplicationPart.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public InventoryController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetStock(int productId)
    {
        var stock = await _dispatcher.Query(new GetStockQuery(productId));
        return Ok(new { productId, stock });
    }

    [HttpPost("update/{productId}")]
    public async Task<IActionResult> UpdateStock(int productId, int change)
    {
        await _dispatcher.Send(new UpdateStockCommand(productId, change));
        return Ok(new { message = "Stock updated" });
    }
}
