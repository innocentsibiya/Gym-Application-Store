using GymStore.Modules.Shipping.Domain;

namespace GymStore.Modules.Shipping.Application.Responses;

/// <summary>HTTP response shape for a shipment.</summary>
public sealed class ShipmentResponse
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string Carrier { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string Status { get; set; } = string.Empty;
}

internal static class ShipmentResponseFactory
{
    public static ShipmentResponse ToResponse(Shipment s) => new()
    {
        Id = s.Id,
        OrderId = s.OrderId,
        Carrier = s.Carrier,
        TrackingNumber = s.TrackingNumber,
        ShippedAt = s.ShippedAt,
        DeliveredAt = s.DeliveredAt,
        Status = s.Status
    };
}
