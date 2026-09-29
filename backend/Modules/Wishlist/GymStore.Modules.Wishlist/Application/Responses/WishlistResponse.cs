using GymStore.Modules.Catalog.Contracts;

namespace GymStore.Modules.Wishlist.Application.Responses;

/// <summary>
/// HTTP response for a wishlist. Items are enriched with product data from Catalog; the
/// pre-refactor endpoint returned the raw Wishlist entity with embedded Product objects.
/// </summary>
public sealed class WishlistResponse
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public List<WishlistItemResponse> Items { get; set; } = new();
}

public sealed class WishlistItemResponse
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}

internal static class WishlistResponseFactory
{
    public static WishlistResponse Create(Domain.Wishlist wishlist, IReadOnlyDictionary<long, ProductSummaryDto> products)
    {
        var items = wishlist.Items.Select(i =>
        {
            products.TryGetValue(i.ProductId, out var p);
            return new WishlistItemResponse
            {
                ProductId = i.ProductId,
                ProductName = p?.Name ?? string.Empty,
                Price = p?.Price ?? 0m,
                ImageUrls = p?.ImageUrls.ToList() ?? new List<string>()
            };
        }).ToList();

        return new WishlistResponse { Id = wishlist.Id, UserId = wishlist.UserId, Items = items };
    }
}
