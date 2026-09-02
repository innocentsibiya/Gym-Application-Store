namespace GymStore.Modules.Ordering.Domain;

/// <summary>
/// The Order aggregate root, owned by the Ordering module. User and Address are referenced by
/// id only (other modules own them); the aggregate itself holds Items and Shipments. Payment is
/// owned by the Payments module and recorded via its contract at checkout.
/// </summary>
public class Order
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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public List<OrderItem> Items { get; set; } = new();
    public List<Shipment> Shipments { get; set; } = new();
}
