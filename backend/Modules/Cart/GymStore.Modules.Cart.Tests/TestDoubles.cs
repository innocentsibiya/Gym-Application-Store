using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Application.Responses;
using GymStore.Modules.Cart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Cart.Tests;

internal static class TestContext
{
    /// <summary>A fresh in-memory CartDbContext with a unique backing store per call.</summary>
    public static CartDbContext NewCartDbContext() =>
        new(new DbContextOptionsBuilder<CartDbContext>()
            .UseInMemoryDatabase($"cart-tests-{Guid.NewGuid()}")
            .Options);
}

/// <summary>Returns preset product data for the ids the cart asks about.</summary>
internal sealed class FakeProductInfoProvider : IProductInfoProvider
{
    private readonly Dictionary<long, ProductInfo> _products;

    public FakeProductInfoProvider(params ProductInfo[] products) =>
        _products = products.ToDictionary(p => p.ProductId);

    public Task<IReadOnlyDictionary<long, ProductInfo>> GetProductsAsync(
        IReadOnlyCollection<long> productIds, CancellationToken ct)
    {
        IReadOnlyDictionary<long, ProductInfo> result = _products
            .Where(kv => productIds.Contains(kv.Key))
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        return Task.FromResult(result);
    }
}

/// <summary>In-memory <see cref="ICartCache"/> that records eviction calls for assertions.</summary>
internal sealed class InMemoryCartCache : ICartCache
{
    public Dictionary<long, CartResponse> Store { get; } = new();
    public int RemoveCount { get; private set; }

    public Task<CartResponse?> GetAsync(long userId, CancellationToken ct) =>
        Task.FromResult(Store.TryGetValue(userId, out var cart) ? cart : null);

    public Task SetAsync(long userId, CartResponse cart, CancellationToken ct)
    {
        Store[userId] = cart;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(long userId, CancellationToken ct)
    {
        Store.Remove(userId);
        RemoveCount++;
        return Task.CompletedTask;
    }
}
