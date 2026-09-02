using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Ordering.Domain;

namespace GymStore.Modules.Ordering.Application.Responses;

/// <summary>HTTP response shape for an order. Mirrors the fields the order UI consumes.</summary>
public sealed class OrderResponse
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string OrderStatus { get; set; } = "Pending";
    public string PaymentStatus { get; set; } = "Pending";
    public long ShippingAddressId { get; set; }
    public long BillingAddressId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal? Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
}

public sealed class OrderItemResponse
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? DiscountApplied { get; set; }

    /// <summary>Product summary enriched from Catalog; null when not enriched (e.g. at placement).</summary>
    public OrderProductRef? Product { get; set; }
}

/// <summary>Minimal product info carried on an order line for display (keeps item.product.name working).</summary>
public sealed class OrderProductRef
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}

/// <summary>Maps order aggregates to responses, optionally enriching lines with Catalog product data.</summary>
internal static class OrderResponseFactory
{
    public static OrderResponse ToResponse(Order order, IReadOnlyDictionary<long, ProductSummaryDto>? products = null)
    {
        return new OrderResponse
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderStatus = order.OrderStatus,
            PaymentStatus = order.PaymentStatus,
            ShippingAddressId = order.ShippingAddressId,
            BillingAddressId = order.BillingAddressId,
            Subtotal = order.Subtotal,
            Tax = order.Tax,
            ShippingCost = order.ShippingCost,
            Discount = order.Discount,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Items = order.Items.Select(i => ToItemResponse(i, products)).ToList()
        };
    }

    private static OrderItemResponse ToItemResponse(OrderItem item, IReadOnlyDictionary<long, ProductSummaryDto>? products)
    {
        OrderProductRef? product = null;
        if (products is not null && products.TryGetValue(item.ProductId, out var summary))
        {
            product = new OrderProductRef
            {
                Id = summary.ProductId,
                Name = summary.Name,
                Price = summary.Price,
                ImageUrls = summary.ImageUrls.ToList()
            };
        }

        return new OrderItemResponse
        {
            Id = item.Id,
            OrderId = item.OrderId,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            DiscountApplied = item.DiscountApplied,
            Product = product
        };
    }
}
