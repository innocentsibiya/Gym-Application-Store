using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreferenceController : ControllerBase
    {
        private readonly IPreferenceService _service;

        public PreferenceController(IPreferenceService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Preference>> GetPreferences(long userId)
        {
            var prefs = await _service.GetPreferencesAsync(userId);
            return Ok(prefs);
        }

        [HttpPost("user/{userId}")]
        public async Task<ActionResult<Preference>> SavePreferences(long userId, [FromBody] Preference prefs)
        {
            prefs.UserId = userId;
            var updated = await _service.SavePreferencesAsync(prefs);
            return Ok(updated);
        }
    }
}