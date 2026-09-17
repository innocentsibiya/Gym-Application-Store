using GymStore.Modules.Payments.Application.Features.GetPaymentByOrder;
using GymStore.Modules.Payments.Infrastructure.Persistence;
using GymStore.Modules.Payments.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Payments.Tests;

public class PaymentsTests
{
    private static PaymentDbContext NewContext() =>
        new(new DbContextOptionsBuilder<PaymentDbContext>()
            .UseInMemoryDatabase($"payments-tests-{Guid.NewGuid()}")
            .Options);

    [Fact]
    public async Task CreatePayment_PersistsPaidPayment_WithNoCardData()
    {
        await using var ctx = NewContext();
        var api = new PaymentModuleApi(new PaymentRepository(ctx));

        await api.CreatePaymentAsync(orderId: 42, method: "eft", amount: 345m, CancellationToken.None);

        var payment = await ctx.Payments.SingleAsync();
        Assert.Equal(42, payment.OrderId);
        Assert.Equal("eft", payment.PaymentMethod);
        Assert.Equal(345m, payment.Amount);
        Assert.Equal("Paid", payment.PaymentStatus);
        Assert.NotNull(payment.PaidAt);
        Assert.False(string.IsNullOrWhiteSpace(payment.TransactionId));
    }

    [Fact]
    public async Task CreatePayment_BlankMethod_DefaultsToCard()
    {
        await using var ctx = NewContext();
        var api = new PaymentModuleApi(new PaymentRepository(ctx));

        await api.CreatePaymentAsync(1, "  ", 10m, CancellationToken.None);

        var payment = await ctx.Payments.SingleAsync();
        Assert.Equal("Card", payment.PaymentMethod);
    }

    [Fact]
    public async Task GetPaymentByOrder_ReturnsPayment_OrNull()
    {
        await using var ctx = NewContext();
        var repo = new PaymentRepository(ctx);
        await new PaymentModuleApi(repo).CreatePaymentAsync(7, "card", 99.50m, CancellationToken.None);

        var handler = new GetPaymentByOrderQueryHandler(repo);

        var found = await handler.Handle(new GetPaymentByOrderQuery(7), CancellationToken.None);
        Assert.NotNull(found);
        Assert.Equal(7, found!.OrderId);
        Assert.Equal(99.50m, found.Amount);
        Assert.Equal("card", found.PaymentMethod);

        var missing = await handler.Handle(new GetPaymentByOrderQuery(999), CancellationToken.None);
        Assert.Null(missing);
    }
}
