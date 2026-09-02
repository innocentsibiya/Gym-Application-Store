using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Contracts;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Ordering.Application.Abstractions;
using GymStore.Modules.Ordering.Application.Responses;
using GymStore.Modules.Ordering.Domain;
using Microsoft.Extensions.Logging;

namespace GymStore.Modules.Ordering.Application.Features.PlaceOrder;

/// <summary>
/// Builds an order from the cart (read via Cart's contract), prices each line at the current
/// catalog price (via Catalog's contract), persists it, then clears the cart best-effort.
/// Mirrors the original OrderRepository.PlaceOrderAsync.
/// </summary>
internal sealed class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orders;
    private readonly ICartModuleApi _cart;
    private readonly ICatalogModuleApi _catalog;
    private readonly ILogger<PlaceOrderCommandHandler> _logger;

    public PlaceOrderCommandHandler(
        IOrderRepository orders, ICartModuleApi cart, ICatalogModuleApi catalog, ILogger<PlaceOrderCommandHandler> logger)
    {
        _orders = orders;
        _cart = cart;
        _catalog = catalog;
        _logger = logger;
    }

    public async Task<OrderResponse> Handle(PlaceOrderCommand command, CancellationToken ct)
    {
        var cart = await _cart.GetCartAsync(command.UserId, ct);
        if (cart is null || cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        var productIds = cart.Items.Select(i => i.ProductId).ToList();
        var products = await _catalog.GetProductsByIdsAsync(productIds, ct);

        var order = new Order
        {
            UserId = command.UserId,
            ShippingAddressId = command.ShippingAddressId,
            BillingAddressId = command.BillingAddressId,
            CreatedAt = DateTime.UtcNow,
            Items = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = products.TryGetValue(i.ProductId, out var p) ? p.Price : 0m
            }).ToList()
        };

        await _orders.AddAsync(order, ct);

        // Clear the cart after the order commits (cross-module call; best-effort).
        try
        {
            await _cart.ClearCartAsync(command.UserId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Order {OrderId} was placed but clearing the cart for user {UserId} failed.",
                order.Id, command.UserId);
        }

        // Placement response leaves product refs null, matching the pre-refactor behavior.
        return OrderResponseFactory.ToResponse(order);
    }
}
