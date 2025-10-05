using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetStock(int productId)
        {
            var stock = await _inventoryService.GetStockLevelAsync(productId);
            return Ok(new { productId, stock });
        }

        [HttpPost("update/{productId}")]
        public async Task<IActionResult> UpdateStock(int productId, int change)
        {
            await _inventoryService.UpdateStockAsync(productId, change);
            return Ok(new { message = "Stock updated" });
        }
    }
}