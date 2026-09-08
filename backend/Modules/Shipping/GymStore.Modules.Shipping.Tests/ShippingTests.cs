using GymStore.Modules.Shipping.Application.Features.DeliverShipment;
using GymStore.Modules.Shipping.Application.Features.GetShipmentByOrder;
using GymStore.Modules.Shipping.Application.Features.ShipOrder;
using GymStore.Modules.Shipping.Infrastructure.Persistence;
using GymStore.Modules.Shipping.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Shipping.Tests;

public class ShippingTests
{
    private static ShippingDbContext NewContext() =>
        new(new DbContextOptionsBuilder<ShippingDbContext>()
            .UseInMemoryDatabase($"shipping-tests-{Guid.NewGuid()}")
            .Options);

    [Fact]
    public async Task CreateShipment_PersistsPendingShipment()
    {
        await using var ctx = NewContext();
        var api = new ShippingModuleApi(new ShipmentRepository(ctx));

        await api.CreateShipmentAsync(orderId: 42, CancellationToken.None);

        var shipment = await ctx.Shipments.SingleAsync();
        Assert.Equal(42, shipment.OrderId);
        Assert.Equal("Pending", shipment.Status);
        Assert.Null(shipment.ShippedAt);
        Assert.Null(shipment.DeliveredAt);
    }

    [Fact]
    public async Task GetShipmentByOrder_ReturnsShipment_OrNull()
    {
        await using var ctx = NewContext();
        var repo = new ShipmentRepository(ctx);
        await new ShippingModuleApi(repo).CreateShipmentAsync(7, CancellationToken.None);
        var handler = new GetShipmentByOrderQueryHandler(repo);

        var found = await handler.Handle(new GetShipmentByOrderQuery(7), CancellationToken.None);
        Assert.NotNull(found);
        Assert.Equal(7, found!.OrderId);

        var missing = await handler.Handle(new GetShipmentByOrderQuery(999), CancellationToken.None);
        Assert.Null(missing);
    }

    [Fact]
    public async Task ShipOrder_SetsCarrierTrackingAndInTransit()
    {
        await using var ctx = NewContext();
        var repo = new ShipmentRepository(ctx);
        await new ShippingModuleApi(repo).CreateShipmentAsync(7, CancellationToken.None);
        var handler = new ShipOrderCommandHandler(repo);

        var result = await handler.Handle(new ShipOrderCommand(7, "DHL", "ZA123"), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("InTransit", result!.Status);
        Assert.Equal("DHL", result.Carrier);
        Assert.Equal("ZA123", result.TrackingNumber);
        Assert.NotNull(result.ShippedAt);
    }

    [Fact]
    public async Task ShipOrder_NoShipment_ReturnsNull()
    {
        await using var ctx = NewContext();
        var handler = new ShipOrderCommandHandler(new ShipmentRepository(ctx));

        var result = await handler.Handle(new ShipOrderCommand(999, "DHL", "X"), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeliverShipment_SetsDeliveredAndTimestamp()
    {
        await using var ctx = NewContext();
        var repo = new ShipmentRepository(ctx);
        await new ShippingModuleApi(repo).CreateShipmentAsync(7, CancellationToken.None);
        var handler = new DeliverShipmentCommandHandler(repo);

        var result = await handler.Handle(new DeliverShipmentCommand(7), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Delivered", result!.Status);
        Assert.NotNull(result.DeliveredAt);
    }
}
