using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Shipping.Application.Responses;

namespace GymStore.Modules.Shipping.Application.Features.GetShipmentByOrder;

public sealed record GetShipmentByOrderQuery(long OrderId) : IQuery<ShipmentResponse?>;
