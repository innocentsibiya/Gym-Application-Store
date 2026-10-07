using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Preferences.Application.Contracts;
using GymStore.Modules.Preferences.Application.Features.GetPreferences;
using GymStore.Modules.Preferences.Application.Features.SavePreferences;
using Microsoft.AspNetCore.Mvc;

namespace GymStore.Modules.Preferences.Api;

/// <summary>
/// Thin HTTP adapter for preferences. Routes/payloads match the original controller (the frontend
/// depends on them). Route is "api/Preference" (from the controller name).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PreferenceController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public PreferenceController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPreferences(long userId)
    {
        var prefs = await _dispatcher.Query(new GetPreferencesQuery(userId));
        return Ok(prefs);
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> SavePreferences(long userId, [FromBody] PreferenceDto prefs)
    {
        var updated = await _dispatcher.Send(new SavePreferencesCommand(userId, prefs));
        return Ok(updated);
    }
}
