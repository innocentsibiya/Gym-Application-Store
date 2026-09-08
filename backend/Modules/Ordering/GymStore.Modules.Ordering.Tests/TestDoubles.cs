using GymStore.Modules.Cart.Contracts;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Ordering.Infrastructure.Persistence;
using GymStore.Modules.Payments.Contracts;
using GymStore.Modules.Shipping.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Ordering.Tests;

internal static class TestContext
{
    public static OrderingDbContext NewOrderingDbContext() =>
        new(new DbContextOptionsBuilder<OrderingDbContext>()
            .UseInMemoryDatabase($"ordering-tests-{Guid.NewGuid()}")
            .Options);
}

/// <summary>Fake Cart contract: returns a preset cart and records cart clears.</summary>
internal sealed class FakeCartModuleApi : ICartModuleApi
{
    private readonly CartContentsDto? _cart;
    public int ClearCount { get; private set; }

    public FakeCartModuleApi(CartContentsDto? cart) => _cart = cart;

    public Task<CartContentsDto?> GetCartAsync(long userId, CancellationToken ct = default) =>
        Task.FromResult(_cart);

    public Task ClearCartAsync(long userId, CancellationToken ct = default)
    {
        ClearCount++;
        return Task.CompletedTask;
    }
}

/// <summary>Fake Payments contract: records the payment it was asked to create.</summary>
internal sealed class FakePaymentModuleApi : IPaymentModuleApi
{
    public int CreateCount { get; private set; }
    public long OrderId { get; private set; }
    public string? Method { get; private set; }
    public decimal Amount { get; private set; }

    public Task CreatePaymentAsync(long orderId, string method, decimal amount, CancellationToken ct = default)
    {
        CreateCount++;
        OrderId = orderId;
        Method = method;
        Amount = amount;
        return Task.CompletedTask;
    }
}

/// <summary>Fake Shipping contract: records the shipment it was asked to create.</summary>
internal sealed class FakeShippingModuleApi : IShippingModuleApi
{
    public int CreateCount { get; private set; }
    public long OrderId { get; private set; }

    public Task CreateShipmentAsync(long orderId, CancellationToken ct = default)
    {
        CreateCount++;
        OrderId = orderId;
        return Task.CompletedTask;
    }
}

/// <summary>Fake Catalog contract: returns preset product summaries for the requested ids.</summary>
internal sealed class FakeCatalogModuleApi : ICatalogModuleApi
{
    private readonly Dictionary<long, ProductSummaryDto> _products;

    public FakeCatalogModuleApi(params ProductSummaryDto[] products) =>
        _products = products.ToDictionary(p => p.ProductId);

    public Task<IReadOnlyDictionary<long, ProductSummaryDto>> GetProductsByIdsAsync(
        IReadOnlyCollection<long> productIds, CancellationToken ct = default)
    {
        IReadOnlyDictionary<long, ProductSummaryDto> result = _products
            .Where(kv => productIds.Contains(kv.Key))
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        return Task.FromResult(result);
    }
}
