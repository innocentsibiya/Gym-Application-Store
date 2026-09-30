using System.ComponentModel.DataAnnotations;

namespace GymStore.Modules.Identity.Application.Contracts;

/// <summary>Registration request (validation preserved from the original RegisterDto).</summary>
public sealed class RegisterRequest
{
    [Required, MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(255)] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    [Phone] public string? PhoneNumber { get; set; }
}

/// <summary>Login request (validation preserved from the original LoginDto).</summary>
public sealed class LoginRequest
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

/// <summary>Internal result of an auth operation. <see cref="User"/> never contains the hash.</summary>
public sealed class AuthResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? Token { get; init; }
    public UserInfo? User { get; init; }

    public static AuthResult Fail(string message) => new() { Success = false, Message = message };
}

/// <summary>Non-sensitive user projection returned to clients on login.</summary>
public sealed record UserInfo(long Id, string FirstName, string LastName, string Email, string Role);
