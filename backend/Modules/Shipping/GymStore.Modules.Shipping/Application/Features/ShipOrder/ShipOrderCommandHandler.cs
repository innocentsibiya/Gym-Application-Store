using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Shipping.Application.Abstractions;
using GymStore.Modules.Shipping.Application.Responses;

namespace GymStore.Modules.Shipping.Application.Features.ShipOrder;

internal sealed class ShipOrderCommandHandler : ICommandHandler<ShipOrderCommand, ShipmentResponse?>
{
    private readonly IShipmentRepository _repository;

    public ShipOrderCommandHandler(IShipmentRepository repository) => _repository = repository;

    public async Task<ShipmentResponse?> Handle(ShipOrderCommand command, CancellationToken ct)
    {
        var shipment = await _repository.GetByOrderIdAsync(command.OrderId, ct);
        if (shipment is null)
        {
            return null;
        }

        shipment.Carrier = string.IsNullOrWhiteSpace(command.Carrier) ? shipment.Carrier : command.Carrier;
        shipment.TrackingNumber = command.TrackingNumber;
        shipment.ShippedAt = DateTime.UtcNow;
        shipment.Status = "InTransit";

        await _repository.SaveChangesAsync(ct);
        return ShipmentResponseFactory.ToResponse(shipment);
    }
}
