using backend.Interfaces;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymItemsController : ControllerBase
    {
        private readonly IGymItems _itemService;

        public GymItemsController(IGymItems itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GymItem>> GetAll()
        {
            List<GymItem> Items = _itemService.GetAllItems();
            return Ok(Items);
        }

        [HttpGet("{id}")]
        public ActionResult<GymItem> GetById(int id)
        {
            GymItem item = _itemService.GetItemById(id);
            if (item == null)
                return NotFound($"Item with ID {id} not found.");

            return Ok(item);
        }

        [HttpGet("category/{category}")]
        public ActionResult<IEnumerable<GymItem>> GetByCategory(string category)
        {
            var items = _itemService.GetItemsByCategory(category);
            return Ok(items);
        }

        [HttpPost]
        public ActionResult<GymItem> Create(GymItem newItem)
        {
            _itemService.CreateItem(newItem);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }
    }
}
