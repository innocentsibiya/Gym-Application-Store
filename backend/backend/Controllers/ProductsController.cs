using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts() { 
            var products = await _productService.GetProductsAsync();

            if (products == null || !products.Any())
                return NotFound("No products available.");

            return Ok(products);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetProductsById(long id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product == null ? NotFound("No products available.") : Ok(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult> Search([FromQuery] string term,[FromQuery] int page = 1,[FromQuery] int pageSize = 9)
        {
            try
            {
                var (products, totalCount) = await _productService.SearchAsync(term, page, pageSize);
                return Ok(new { products, totalCount, page, pageSize });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Search failed", Details = ex.Message });
            }
        }
    }
}