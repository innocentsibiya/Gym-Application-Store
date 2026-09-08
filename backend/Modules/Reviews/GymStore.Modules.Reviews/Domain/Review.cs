namespace GymStore.Modules.Reviews.Domain;

/// <summary>
/// A product review, owned by the Reviews module. References the reviewer and product by id
/// only (User and Catalog are other concerns).
/// </summary>
public class Review
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsApproved { get; set; }
}
