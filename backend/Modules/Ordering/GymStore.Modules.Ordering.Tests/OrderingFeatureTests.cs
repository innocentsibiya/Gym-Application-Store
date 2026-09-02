using GymStore.Modules.Cart.Contracts;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Ordering.Application.Features.GetUserOrders;
using GymStore.Modules.Ordering.Application.Features.GetUserOrdersByYear;
using GymStore.Modules.Ordering.Application.Features.PlaceOrder;
using GymStore.Modules.Ordering.Domain;
using GymStore.Modules.Ordering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace GymStore.Modules.Ordering.Tests;

public class OrderingFeatureTests
{
    private static readonly ProductSummaryDto Dumbbell =
        new(1, "Dumbbell", 60m, new[] { "db-main.jpg" });
    private static readonly ProductSummaryDto Kettlebell =
        new(6, "Kettlebell", 75m, new[] { "kb-main.jpg" });

    [Fact]
    public async Task PlaceOrder_BuildsOrderFromCart_PricesFromCatalog_AndClearsCart()
    {
        await using var ctx = TestContext.NewOrderingDbContext();
        var cart = new CartContentsDto(10, 1, new[] { new CartLineDto(1, 2), new CartLineDto(6, 3) });
        var cartApi = new FakeCartModuleApi(cart);
        var paymentApi = new FakePaymentModuleApi();
        var handler = new PlaceOrderCommandHandler(
            new OrderRepository(ctx), cartApi, new FakeCatalogModuleApi(Dumbbell, Kettlebell),
            paymentApi, NullLogger<PlaceOrderCommandHandler>.Instance);

        var result = await handler.Handle(new PlaceOrderCommand(1, 5, 5, "eft"), CancellationToken.None);

        Assert.Equal(1, result.UserId);
        Assert.Equal(5, result.ShippingAddressId);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(60m, result.Items.Single(i => i.ProductId == 1).UnitPrice);
        Assert.Equal(75m, result.Items.Single(i => i.ProductId == 6).UnitPrice);
        Assert.Equal(345m, result.TotalAmount); // 60*2 + 75*3 — totals now computed
        Assert.Equal(1, cartApi.ClearCount); // cart cleared after placement

        // Payment recorded for the order with the computed amount and chosen method.
        Assert.Equal(1, paymentApi.CreateCount);
        Assert.Equal(345m, paymentApi.Amount);
        Assert.Equal("eft", paymentApi.Method);

        // Order was persisted with its lines and total.
        var persisted = await ctx.Orders.Include(o => o.Items).SingleAsync();
        Assert.Equal(1, persisted.UserId);
        Assert.Equal(2, persisted.Items.Count);
        Assert.Equal(345m, persisted.TotalAmount);
    }

    [Fact]
    public async Task PlaceOrder_EmptyCart_Throws_AndDoesNotClear()
    {
        await using var ctx = TestContext.NewOrderingDbContext();
        var cartApi = new FakeCartModuleApi(new CartContentsDto(10, 1, Array.Empty<CartLineDto>()));
        var handler = new PlaceOrderCommandHandler(
            new OrderRepository(ctx), cartApi, new FakeCatalogModuleApi(),
            new FakePaymentModuleApi(), NullLogger<PlaceOrderCommandHandler>.Instance);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new PlaceOrderCommand(1, 5, 5), CancellationToken.None));

        Assert.Equal(0, cartApi.ClearCount);
        Assert.Empty(await ctx.Orders.ToListAsync());
    }

    [Fact]
    public async Task PlaceOrder_NullCart_Throws()
    {
        await using var ctx = TestContext.NewOrderingDbContext();
        var handler = new PlaceOrderCommandHandler(
            new OrderRepository(ctx), new FakeCartModuleApi(null), new FakeCatalogModuleApi(),
            new FakePaymentModuleApi(), NullLogger<PlaceOrderCommandHandler>.Instance);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new PlaceOrderCommand(1, 5, 5), CancellationToken.None));
    }

    [Fact]
    public async Task GetUserOrders_EnrichesLinesWithProductName()
    {
        await using var ctx = TestContext.NewOrderingDbContext();
        ctx.Orders.Add(new Order
        {
            Id = 1, UserId = 1, CreatedAt = new DateTime(2026, 3, 1),
            Items = { new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 60m } }
        });
        await ctx.SaveChangesAsync();

        var handler = new GetUserOrdersQueryHandler(new OrderRepository(ctx), new FakeCatalogModuleApi(Dumbbell));
        var result = await handler.Handle(new GetUserOrdersQuery(1), CancellationToken.None);

        var order = Assert.Single(result);
        var item = Assert.Single(order.Items);
        Assert.NotNull(item.Product);
        Assert.Equal("Dumbbell", item.Product!.Name); // item.product.name preserved for the UI
        Assert.Equal(60m, item.UnitPrice);
    }

    [Fact]
    public async Task GetUserOrdersByYear_FiltersByYear()
    {
        await using var ctx = TestContext.NewOrderingDbContext();
        ctx.Orders.AddRange(
            new Order { Id = 1, UserId = 1, CreatedAt = new DateTime(2025, 12, 31),
                Items = { new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 60m } } },
            new Order { Id = 2, UserId = 1, CreatedAt = new DateTime(2026, 1, 1),
                Items = { new OrderItem { Id = 2, OrderId = 2, ProductId = 6, Quantity = 1, UnitPrice = 75m } } });
        await ctx.SaveChangesAsync();

        var handler = new GetUserOrdersByYearQueryHandler(new OrderRepository(ctx), new FakeCatalogModuleApi(Dumbbell, Kettlebell));
        var result = await handler.Handle(new GetUserOrdersByYearQuery(1, 2026), CancellationToken.None);

        var order = Assert.Single(result);
        Assert.Equal(2, order.Id);
    }
}
