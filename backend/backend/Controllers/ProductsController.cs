using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/controller")]
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
        public async Task<IActionResult> GetProductsById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product == null ? NotFound("No products available.") : Ok(product);
        }
    }
}