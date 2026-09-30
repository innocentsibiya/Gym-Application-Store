using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Identity.Application.Contracts;
using GymStore.Modules.Identity.Application.Features.Login;
using GymStore.Modules.Identity.Application.Features.Register;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Identity.Api;

/// <summary>
/// Thin HTTP adapter for auth. Routes/success shapes match the original controller (the frontend
/// depends on them). Login now returns a single generic 401 for any auth failure (anti-enumeration).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public AuthController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _dispatcher.Send(new RegisterCommand(request));
        if (!result.Success)
        {
            return Conflict(new { result.Message });
        }

        return CreatedAtAction(nameof(Register), new { request.Email }, new { result.Message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _dispatcher.Send(new LoginCommand(request));
        if (!result.Success)
        {
            // Generic 401 whether the email is unknown or the password is wrong.
            return Unauthorized(new { result.Message });
        }

        return Ok(new
        {
            result.Message,
            result.Token,
            User = new
            {
                result.User!.Id,
                result.User.FirstName,
                result.User.LastName,
                result.User.Email,
                result.User.Role
            }
        });
    }
}
