using GymStore.Modules.Reviews.Domain;

namespace GymStore.Modules.Reviews.Application.Responses;

/// <summary>
/// HTTP response for a review. Carries a safe <see cref="ReviewerName"/> only — never the user
/// entity or any secret (the pre-refactor endpoint leaked the reviewer's password hash).
/// </summary>
public sealed class ReviewResponse
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public long ProductId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsApproved { get; set; }
}

internal static class ReviewResponseFactory
{
    public static ReviewResponse ToResponse(Review r, string reviewerName) => new()
    {
        Id = r.Id,
        UserId = r.UserId,
        ReviewerName = reviewerName,
        ProductId = r.ProductId,
        Rating = r.Rating,
        Comment = r.Comment,
        CreatedAt = r.CreatedAt,
        UpdatedAt = r.UpdatedAt,
        IsApproved = r.IsApproved
    };
}
