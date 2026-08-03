using GymStore.Modules.Cart.Application.Abstractions;

namespace GymStore.Modules.Cart.Application.Responses;

/// <summary>
/// Builds a <see cref="CartResponse"/> from a cart aggregate, enriching each line with
/// product data via <see cref="IProductInfoProvider"/>. Centralizes the mapping shared by
/// all three cart features.
/// </summary>
internal static class CartResponseFactory
{
    public static async Task<CartResponse> CreateAsync(
        Domain.Cart cart, IProductInfoProvider productInfo, CancellationToken ct)
    {
        if (cart.Items.Count == 0)
        {
            return new CartResponse { Id = cart.Id, UserId = cart.UserId };
        }

        var productIds = cart.Items.Select(i => i.ProductId).Distinct().ToArray();
        var products = await productInfo.GetProductsAsync(productIds, ct);

        var items = cart.Items.Select(item =>
        {
            products.TryGetValue(item.ProductId, out var info);
            return new CartItemResponse
            {
                ProductId = item.ProductId,
                ProductName = info?.Name ?? string.Empty,
                ImageUrls = info?.ImageUrls.ToList() ?? new List<string>(),
                Price = info?.Price ?? 0m,
                Quantity = item.Quantity
            };
        }).ToList();

        return new CartResponse { Id = cart.Id, UserId = cart.UserId, Items = items };
    }
}
