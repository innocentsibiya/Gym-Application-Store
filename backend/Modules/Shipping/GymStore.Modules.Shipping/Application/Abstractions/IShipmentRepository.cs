using GymStore.Modules.Shipping.Domain;

namespace GymStore.Modules.Shipping.Application.Abstractions;

/// <summary>Persistence operations for shipments (module-internal).</summary>
public interface IShipmentRepository
{
    Task<Shipment> AddAsync(Shipment shipment, CancellationToken ct);
    Task<Shipment?> GetByOrderIdAsync(long orderId, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
