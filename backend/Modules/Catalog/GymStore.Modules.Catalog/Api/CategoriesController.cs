using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Features.GetCategories;
using GymStore.Modules.Catalog.Application.Features.GetCategoryWithProducts;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Catalog.Api;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public CategoriesController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _dispatcher.Query(new GetCategoriesQuery());
        return Ok(categories);
    }

    [HttpGet("categories/{id}")]
    public async Task<IActionResult> GetCategoryWithProducts(int id)
    {
        var category = await _dispatcher.Query(new GetCategoryWithProductsQuery(id));
        return category == null ? NotFound() : Ok(category);
    }
}
