using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Application.Features.AddItem;
using GymStore.Modules.Cart.Application.Features.GetCart;
using GymStore.Modules.Cart.Application.Features.RemoveItem;
using GymStore.Modules.Cart.Application.Responses;
using GymStore.Modules.Cart.Infrastructure.Persistence;
using GymStore.Modules.Cart.Infrastructure.Public;

namespace GymStore.Modules.Cart.Tests;

public class CartFeatureTests
{
    private static readonly ProductInfo Dumbbell =
        new(1, "Dumbbell", 60m, new[] { "dumbbell-main.jpg", "dumbbell-2.jpg" });
    private static readonly ProductInfo Kettlebell =
        new(6, "Kettlebell", 75m, new[] { "kettlebell-main.jpg" });

    [Fact]
    public async Task GetCart_WhenNoCartExists_CreatesEmptyCartAndCachesIt()
    {
        await using var ctx = TestContext.NewCartDbContext();
        var repo = new CartRepository(ctx);
        var cache = new InMemoryCartCache();
        var handler = new GetCartQueryHandler(repo, cache, new FakeProductInfoProvider());

        var result = await handler.Handle(new GetCartQuery(1), CancellationToken.None);

        Assert.Equal(1, result.UserId);
        Assert.Empty(result.Items);
        Assert.True(result.Id > 0, "a cart row should have been created and persisted");
        Assert.True(cache.Store.ContainsKey(1), "the result should have been cached");
    }

    [Fact]
    public async Task GetCart_WhenCacheHit_ReturnsCachedCopy()
    {
        await using var ctx = TestContext.NewCartDbContext();
        var repo = new CartRepository(ctx);
        var cache = new InMemoryCartCache();
        var products = new FakeProductInfoProvider(Dumbbell);

        // Arrange: adding an item populates the cache as a side effect.
        var add = new AddItemToCartCommandHandler(repo, cache, products);
        await add.Handle(new AddItemToCartCommand(1, 1, 2), CancellationToken.None);
        var cachedBefore = cache.Store[1];

        var get = new GetCartQueryHandler(repo, cache, products);
        var result = await get.Handle(new GetCartQuery(1), CancellationToken.None);

        Assert.Same(cachedBefore, result); // served from cache, not rebuilt
    }

    [Fact]
    public async Task AddItem_NewItem_IsEnrichedWithCatalogDataAndCached()
    {
        await using var ctx = TestContext.NewCartDbContext();
        var repo = new CartRepository(ctx);
        var cache = new InMemoryCartCache();
        var handler = new AddItemToCartCommandHandler(repo, cache, new FakeProductInfoProvider(Dumbbell));

        var result = await handler.Handle(new AddItemToCartCommand(1, 1, 2), CancellationToken.None);

        var item = Assert.Single(result.Items);
        Assert.Equal(1, item.ProductId);
        Assert.Equal("Dumbbell", item.ProductName);
        Assert.Equal(60m, item.Price);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(120m, item.TotalPrice);
        Assert.Equal(new[] { "dumbbell-main.jpg", "dumbbell-2.jpg" }, item.ImageUrls);
        Assert.True(cache.Store.ContainsKey(1));
    }

    [Fact]
    public async Task AddItem_ExistingItem_SetsQuantityRatherThanIncrementing()
    {
        await using var ctx = TestContext.NewCartDbContext();
        var repo = new CartRepository(ctx);
        var cache = new InMemoryCartCache();
        var handler = new AddItemToCartCommandHandler(repo, cache, new FakeProductInfoProvider(Dumbbell));

        await handler.Handle(new AddItemToCartCommand(1, 1, 2), CancellationToken.None);
        var result = await handler.Handle(new AddItemToCartCommand(1, 1, 5), CancellationToken.None);

        var item = Assert.Single(result.Items);
        Assert.Equal(5, item.Quantity); // set to 5, not 2 + 5
    }

    [Fact]
    public async Task RemoveItem_RemovesOnlyTheTargetLine()
    {
        await using var ctx = TestContext.NewCartDbContext();
        var repo = new CartRepository(ctx);
        var cache = new InMemoryCartCache();
        var products = new FakeProductInfoProvider(Dumbbell, Kettlebell);
        var add = new AddItemToCartCommandHandler(repo, cache, products);
        var remove = new RemoveItemFromCartCommandHandler(repo, cache, products);

        await add.Handle(new AddItemToCartCommand(1, 1, 2), CancellationToken.None);
        await add.Handle(new AddItemToCartCommand(1, 6, 3), CancellationToken.None);

        var result = await remove.Handle(new RemoveItemFromCartCommand(1, 1), CancellationToken.None);

        var item = Assert.Single(result.Items);
        Assert.Equal(6, item.ProductId);
    }

    [Fact]
    public async Task CartModuleApi_ClearCart_EmptiesCartAndEvictsCache()
    {
        await using var ctx = TestContext.NewCartDbContext();
        var repo = new CartRepository(ctx);
        var cache = new InMemoryCartCache();
        await repo.AddOrUpdateItemAsync(1, 1, 2, CancellationToken.None);
        await cache.SetAsync(1, new CartResponse { UserId = 1 }, CancellationToken.None);

        var api = new CartModuleApi(repo, cache);
        await api.ClearCartAsync(1, CancellationToken.None);

        var contents = await api.GetCartAsync(1, CancellationToken.None);
        Assert.NotNull(contents);
        Assert.Empty(contents!.Items);
        Assert.False(cache.Store.ContainsKey(1));
        Assert.Equal(1, cache.RemoveCount);
    }

    [Fact]
    public async Task CartModuleApi_GetCart_ReturnsIdsAndQuantities()
    {
        await using var ctx = TestContext.NewCartDbContext();
        var repo = new CartRepository(ctx);
        await repo.AddOrUpdateItemAsync(1, 1, 2, CancellationToken.None);
        await repo.AddOrUpdateItemAsync(1, 6, 3, CancellationToken.None);

        var api = new CartModuleApi(repo, new InMemoryCartCache());
        var contents = await api.GetCartAsync(1, CancellationToken.None);

        Assert.NotNull(contents);
        Assert.Equal(1, contents!.UserId);
        Assert.Equal(2, contents.Items.Count);
        Assert.Contains(contents.Items, l => l.ProductId == 1 && l.Quantity == 2);
        Assert.Contains(contents.Items, l => l.ProductId == 6 && l.Quantity == 3);
    }
}
