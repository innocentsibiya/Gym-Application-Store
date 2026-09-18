using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Shipping.Application.Abstractions;
using GymStore.Modules.Shipping.Application.Responses;

namespace GymStore.Modules.Shipping.Application.Features.DeliverShipment;

internal sealed class DeliverShipmentCommandHandler : ICommandHandler<DeliverShipmentCommand, ShipmentResponse?>
{
    private readonly IShipmentRepository _repository;

    public DeliverShipmentCommandHandler(IShipmentRepository repository) => _repository = repository;

    public async Task<ShipmentResponse?> Handle(DeliverShipmentCommand command, CancellationToken ct)
    {
        var shipment = await _repository.GetByOrderIdAsync(command.OrderId, ct);
        if (shipment is null)
        {
            return null;
        }

        shipment.DeliveredAt = DateTime.UtcNow;
        shipment.Status = "Delivered";

        await _repository.SaveChangesAsync(ct);
        return ShipmentResponseFactory.ToResponse(shipment);
    }
}
