namespace GymStore.Modules.Ordering.Domain;

/// <summary>Shipment record for an order (part of the aggregate; not yet exercised by any flow).</summary>
public class Shipment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string Carrier { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string Status { get; set; } = "InTransit";
}
