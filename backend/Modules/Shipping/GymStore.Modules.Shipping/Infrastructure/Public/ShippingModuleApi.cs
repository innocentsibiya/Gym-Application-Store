using GymStore.Modules.Shipping.Application.Abstractions;
using GymStore.Modules.Shipping.Contracts;
using GymStore.Modules.Shipping.Domain;

namespace GymStore.Modules.Shipping.Infrastructure.Public;

/// <summary>
/// Implements the cross-module contract. Creates a pending shipment for an order — carrier and
/// tracking are placeholders until the order is actually shipped via the module's endpoints.
/// </summary>
internal sealed class ShippingModuleApi : IShippingModuleApi
{
    private readonly IShipmentRepository _repository;

    public ShippingModuleApi(IShipmentRepository repository) => _repository = repository;

    public async Task CreateShipmentAsync(long orderId, CancellationToken ct = default)
    {
        var shipment = new Shipment
        {
            OrderId = orderId,
            Status = "Pending",
            Carrier = "Pending" // Carrier is NOT NULL; set for real when the order ships.
        };

        await _repository.AddAsync(shipment, ct);
    }
}
