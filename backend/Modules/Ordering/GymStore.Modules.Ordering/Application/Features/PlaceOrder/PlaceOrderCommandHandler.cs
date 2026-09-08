using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Contracts;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Ordering.Application.Abstractions;
using GymStore.Modules.Ordering.Application.Responses;
using GymStore.Modules.Ordering.Domain;
using GymStore.Modules.Payments.Contracts;
using GymStore.Modules.Shipping.Contracts;
using Microsoft.Extensions.Logging;

namespace GymStore.Modules.Ordering.Application.Features.PlaceOrder;

/// <summary>
/// Builds an order from the cart (read via Cart's contract), prices each line at the current
/// catalog price (via Catalog's contract), persists it with computed totals, then records a
/// payment (Payments' contract) and clears the cart — both best-effort.
/// </summary>
internal sealed class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orders;
    private readonly ICartModuleApi _cart;
    private readonly ICatalogModuleApi _catalog;
    private readonly IPaymentModuleApi _payments;
    private readonly IShippingModuleApi _shipping;
    private readonly ILogger<PlaceOrderCommandHandler> _logger;

    public PlaceOrderCommandHandler(
        IOrderRepository orders,
        ICartModuleApi cart,
        ICatalogModuleApi catalog,
        IPaymentModuleApi payments,
        IShippingModuleApi shipping,
        ILogger<PlaceOrderCommandHandler> logger)
    {
        _orders = orders;
        _cart = cart;
        _catalog = catalog;
        _payments = payments;
        _shipping = shipping;
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

        var items = cart.Items.Select(i => new OrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = products.TryGetValue(i.ProductId, out var p) ? p.Price : 0m
        }).ToList();

        var amount = items.Sum(i => i.UnitPrice * i.Quantity);

        var order = new Order
        {
            UserId = command.UserId,
            ShippingAddressId = command.ShippingAddressId,
            BillingAddressId = command.BillingAddressId,
            CreatedAt = DateTime.UtcNow,
            Subtotal = amount,
            TotalAmount = amount,
            Items = items
        };

        await _orders.AddAsync(order, ct);

        // Record the payment for the order (cross-module call; best-effort).
        try
        {
            await _payments.CreatePaymentAsync(order.Id, command.Method, amount, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Order {OrderId} was placed but recording its payment failed.", order.Id);
        }

        // Create a pending shipment for the order (cross-module call; best-effort).
        try
        {
            await _shipping.CreateShipmentAsync(order.Id, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Order {OrderId} was placed but creating its shipment failed.", order.Id);
        }

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
