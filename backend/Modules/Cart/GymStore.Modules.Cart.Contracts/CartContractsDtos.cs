namespace GymStore.Modules.Cart.Contracts;

/// <summary>Cross-module snapshot of a cart: identity plus lines (ids + quantities only).</summary>
public sealed record CartContentsDto(long CartId, long UserId, IReadOnlyList<CartLineDto> Items);

/// <summary>
/// A single cart line. Intentionally carries no product or pricing data — that is the
/// Catalog module's concern; consumers resolve product details themselves.
/// </summary>
public sealed record CartLineDto(long ProductId, int Quantity);
