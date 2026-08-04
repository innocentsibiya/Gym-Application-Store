namespace GymStore.Modules.Cart.Application.Responses;

/// <summary>
/// HTTP response shape for the cart. Property names/casing are kept identical to the
/// pre-refactor <c>CartDto</c>/<c>CartItemDto</c> so the API contract is unchanged.
/// </summary>
public sealed class CartResponse
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public List<CartItemResponse> Items { get; set; } = new();
}

public sealed class CartItemResponse
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
}
