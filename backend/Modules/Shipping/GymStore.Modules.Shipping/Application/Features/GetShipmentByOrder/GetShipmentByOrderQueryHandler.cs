using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Shipping.Application.Abstractions;
using GymStore.Modules.Shipping.Application.Responses;

namespace GymStore.Modules.Shipping.Application.Features.GetShipmentByOrder;

internal sealed class GetShipmentByOrderQueryHandler : IQueryHandler<GetShipmentByOrderQuery, ShipmentResponse?>
{
    private readonly IShipmentRepository _repository;

    public GetShipmentByOrderQueryHandler(IShipmentRepository repository) => _repository = repository;

    public async Task<ShipmentResponse?> Handle(GetShipmentByOrderQuery query, CancellationToken ct)
    {
        var shipment = await _repository.GetByOrderIdAsync(query.OrderId, ct);
        return shipment is null ? null : ShipmentResponseFactory.ToResponse(shipment);
    }
}
