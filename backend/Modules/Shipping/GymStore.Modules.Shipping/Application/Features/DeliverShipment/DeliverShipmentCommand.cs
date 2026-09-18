using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Shipping.Application.Responses;

namespace GymStore.Modules.Shipping.Application.Features.DeliverShipment;

/// <summary>Marks an order's shipment as delivered.</summary>
public sealed record DeliverShipmentCommand(long OrderId) : ICommand<ShipmentResponse?>;
