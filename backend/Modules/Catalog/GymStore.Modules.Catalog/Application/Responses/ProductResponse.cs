using GymStore.Modules.Catalog.Domain;

namespace GymStore.Modules.Catalog.Application.Responses;

/// <summary>
/// HTTP response shape for a product. Property names/casing are identical to the pre-refactor
/// <c>ProductDto</c> so the API contract (and the frontend Product interface) is unchanged.
/// </summary>
public sealed class ProductResponse
{
    public long Id { get; set; }
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string SKU { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal? Weight { get; set; }
    public string? Dimensions { get; set; }
    public bool IsActive { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}

/// <summary>Maps a Catalog <see cref="Product"/> to its API response.</summary>
internal static class ProductResponseFactory
{
    public static ProductResponse ToResponse(Product p) => new()
    {
        Id = p.Id,
        CategoryId = p.CategoryId,
        CategoryName = p.Category?.Name ?? string.Empty,
        Name = p.Name,
        Slug = p.Slug,
        Description = p.Description,
        Brand = p.Brand,
        Price = p.Price,
        DiscountPrice = p.DiscountPrice,
        SKU = p.SKU,
        StockQuantity = p.StockQuantity,
        Weight = p.Weight,
        Dimensions = p.Dimensions,
        IsActive = p.IsActive,
        ImageUrls = p.Images.Select(img => img.ImageUrl).ToList()
    };
}
