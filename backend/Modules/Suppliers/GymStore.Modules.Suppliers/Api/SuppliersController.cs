using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Suppliers.Application.Features.GetSupplierById;
using GymStore.Modules.Suppliers.Application.Features.GetSuppliers;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Suppliers.Api;

/// <summary>Thin HTTP adapter for suppliers. Discovered by the host via AddApplicationPart.</summary>
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public SuppliersController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers()
    {
        var suppliers = await _dispatcher.Query(new GetSuppliersQuery());
        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplier(int id)
    {
        var supplier = await _dispatcher.Query(new GetSupplierByIdQuery(id));
        return supplier is null ? NotFound() : Ok(supplier);
    }
}
