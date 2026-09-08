using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Shipping.Application.Responses;

namespace GymStore.Modules.Shipping.Application.Features.ShipOrder;

/// <summary>Marks an order's shipment as shipped (in transit) with a carrier and tracking number.</summary>
public sealed record ShipOrderCommand(long OrderId, string Carrier, string? TrackingNumber)
    : ICommand<ShipmentResponse?>;
