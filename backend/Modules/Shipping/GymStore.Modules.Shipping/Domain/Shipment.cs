namespace GymStore.Modules.Shipping.Domain;

/// <summary>
/// A shipment for an order, owned by the Shipping module. Created "Pending" at checkout, then
/// advanced through "InTransit" (shipped) and "Delivered".
/// </summary>
public class Shipment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string Carrier { get; set; } = "Pending";
    public string? TrackingNumber { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string Status { get; set; } = "Pending";
}
