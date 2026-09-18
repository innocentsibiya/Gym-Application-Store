using GymStore.Modules.Shipping.Application.Abstractions;
using GymStore.Modules.Shipping.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Shipping.Infrastructure.Persistence;

internal sealed class ShipmentRepository : IShipmentRepository
{
    private readonly ShippingDbContext _db;

    public ShipmentRepository(ShippingDbContext db) => _db = db;

    public async Task<Shipment> AddAsync(Shipment shipment, CancellationToken ct)
    {
        _db.Shipments.Add(shipment);
        await _db.SaveChangesAsync(ct);
        return shipment;
    }

    // Tracked so callers can mutate and persist via SaveChangesAsync.
    public Task<Shipment?> GetByOrderIdAsync(long orderId, CancellationToken ct) =>
        _db.Shipments.FirstOrDefaultAsync(s => s.OrderId == orderId, ct);

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
