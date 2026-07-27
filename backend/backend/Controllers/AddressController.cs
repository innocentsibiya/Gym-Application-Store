using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAddresses(long userId)
        {
            var addresses = await _addressService.GetUserAddressesAsync(userId);
            return Ok(addresses);
        }

        [HttpGet("{userId}/default/{type}")]
        public async Task<IActionResult> GetDefaultAddress(long userId, string type)
        {
            var address = await _addressService.GetDefaultAddressAsync(userId, type);
            if (address == null) return NotFound();
            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(Address address)
        {
            await _addressService.AddAddressAsync(address);
            return CreatedAtAction(nameof(GetUserAddresses), new { userId = address.UserId }, address);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAddress(long id)
        {
            await _addressService.RemoveAddressAsync(id);
            return NoContent();
        }
    }
}