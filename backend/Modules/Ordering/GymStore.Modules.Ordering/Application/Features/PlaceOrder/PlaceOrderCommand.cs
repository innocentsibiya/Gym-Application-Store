using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Ordering.Application.Responses;

namespace GymStore.Modules.Ordering.Application.Features.PlaceOrder;

/// <summary>Places an order from the user's current cart. Request Items/Payment are intentionally
/// not part of this command — the order is built from the cart, matching existing behavior.</summary>
public sealed record PlaceOrderCommand(long UserId, long ShippingAddressId, long BillingAddressId)
    : ICommand<OrderResponse>;
