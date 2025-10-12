using backend.DTO;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
                return Conflict(new { result.Message });

            return CreatedAtAction(nameof(Register), new { dto.Email }, new { result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(new { result.Message });
                if (result.Message.Contains("Invalid"))
                    return Unauthorized(new { result.Message });

                return BadRequest(new { result.Message });
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
}