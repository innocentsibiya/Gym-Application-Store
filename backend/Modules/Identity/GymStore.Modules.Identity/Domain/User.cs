namespace GymStore.Modules.Identity.Domain;

/// <summary>
/// A user account, owned by the Identity module. <see cref="PasswordHash"/> is an opaque,
/// salted credential blob — never a plaintext or reversible value.
/// </summary>
public class User
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "Customer";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
