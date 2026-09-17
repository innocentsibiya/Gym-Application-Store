namespace GymStore.Modules.Ordering.Api.Requests;

/// <summary>
/// Body for POST /api/Orders/place. Only the ids are used; the order lines are taken from the
/// user's cart (the old request also carried Items/Payment, which were ignored).
/// </summary>
public sealed class PlaceOrderRequest
{
    public int UserId { get; set; }
    public int ShippingAddressId { get; set; }
    public int BillingAddressId { get; set; }

    /// <summary>Chosen payment method (e.g. "card"/"eft"). Optional; defaults to "Card".</summary>
    public string Method { get; set; } = "Card";
}
