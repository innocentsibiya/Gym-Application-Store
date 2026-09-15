using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Features.GetAllProducts;
using GymStore.Modules.Catalog.Application.Features.GetProductById;
using GymStore.Modules.Catalog.Application.Features.SearchProducts;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Catalog.Api;

/// <summary>
/// Thin HTTP adapter for products. Routes and payloads are identical to the original
/// controller; each action dispatches a query. Discovered by the host via AddApplicationPart.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public ProductsController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _dispatcher.Query(new GetAllProductsQuery());

        if (products is null || products.Count == 0)
        {
            return NotFound("No products available.");
        }

        return Ok(products);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetProductsById(long id)
    {
        var product = await _dispatcher.Query(new GetProductByIdQuery(id));
        return product == null ? NotFound("No products available.") : Ok(product);
    }

    [HttpGet("search")]
    public async Task<ActionResult> Search([FromQuery] string term, [FromQuery] int page = 1, [FromQuery] int pageSize = 9)
    {
        try
        {
            var result = await _dispatcher.Query(new SearchProductsQuery(term, page, pageSize));
            return Ok(new { products = result.Products, totalCount = result.TotalCount, page, pageSize });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Search failed", Details = ex.Message });
        }
    }
}
