using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Contracts;
using GymStore.Modules.Addresses.Application.Features.AddAddress;
using GymStore.Modules.Addresses.Application.Features.GetDefaultAddress;
using GymStore.Modules.Addresses.Application.Features.GetUserAddresses;
using GymStore.Modules.Addresses.Application.Features.RemoveAddress;
using GymStore.Modules.Addresses.Application.Features.UpdateAddress;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Addresses.Api;

/// <summary>
/// Thin HTTP adapter for addresses. Routes/payloads match the original controller (the frontend
/// depends on this contract); each action dispatches a command/query. Discovered by the host via
/// AddApplicationPart. Route is "api/Address" (from the controller name).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public AddressController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserAddresses(long userId)
    {
        var addresses = await _dispatcher.Query(new GetUserAddressesQuery(userId));
        return Ok(addresses);
    }

    [HttpGet("{userId}/default/{type}")]
    public async Task<IActionResult> GetDefaultAddress(long userId, string type)
    {
        var address = await _dispatcher.Query(new GetDefaultAddressQuery(userId, type));
        return address == null ? NotFound() : Ok(address);
    }

    [HttpPost]
    public async Task<IActionResult> AddAddress([FromBody] AddressDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _dispatcher.Send(new AddAddressCommand(dto));
        return Ok(new { Message = "Address added successfully." });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(long id, [FromBody] AddressDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _dispatcher.Send(new UpdateAddressCommand(dto));
        return Ok(new { Message = "Address updated successfully." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAddress(long id)
    {
        await _dispatcher.Send(new RemoveAddressCommand(id));
        return NoContent();
    }
}
