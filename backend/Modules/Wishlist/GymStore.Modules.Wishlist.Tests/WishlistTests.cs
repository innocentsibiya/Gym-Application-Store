using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Wishlist.Application.Features.AddToWishlist;
using GymStore.Modules.Wishlist.Application.Features.GetWishlist;
using GymStore.Modules.Wishlist.Application.Features.RemoveFromWishlist;
using GymStore.Modules.Wishlist.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Wishlist.Tests;

public class WishlistTests
{
    private static WishlistDbContext NewContext() =>
        new(new DbContextOptionsBuilder<WishlistDbContext>()
            .UseInMemoryDatabase($"wishlist-tests-{Guid.NewGuid()}")
            .Options);

    private static readonly ProductSummaryDto Dumbbell =
        new(1, "Dumbbell", 60m, new[] { "db-main.jpg", "db-2.jpg" });

    [Fact]
    public async Task GetWishlist_WhenNoneExists_CreatesEmptyOne()
    {
        await using var ctx = NewContext();
        var handler = new GetWishlistQueryHandler(new WishlistRepository(ctx), new FakeCatalogModuleApi());

        var result = await handler.Handle(new GetWishlistQuery(1), CancellationToken.None);

        Assert.Equal(1, result.UserId);
        Assert.Empty(result.Items);
        Assert.True(result.Id > 0); // persisted
    }

    [Fact]
    public async Task AddThenGet_EnrichesItemsFromCatalog_NoDuplicates()
    {
        await using var ctx = NewContext();
        var repo = new WishlistRepository(ctx);
        var add = new AddToWishlistCommandHandler(repo);

        Assert.Equal(1, await add.Handle(new AddToWishlistCommand(1, 1), CancellationToken.None));
        // Adding the same product again is a no-op.
        Assert.Equal(1, await add.Handle(new AddToWishlistCommand(1, 1), CancellationToken.None));

        var get = new GetWishlistQueryHandler(repo, new FakeCatalogModuleApi(Dumbbell));
        var result = await get.Handle(new GetWishlistQuery(1), CancellationToken.None);

        var item = Assert.Single(result.Items);
        Assert.Equal(1, item.ProductId);
        Assert.Equal("Dumbbell", item.ProductName);
        Assert.Equal(60m, item.Price);
        Assert.Equal(new[] { "db-main.jpg", "db-2.jpg" }, item.ImageUrls);
    }

    [Fact]
    public async Task Remove_TakesItemOut()
    {
        await using var ctx = NewContext();
        var repo = new WishlistRepository(ctx);
        await new AddToWishlistCommandHandler(repo).Handle(new AddToWishlistCommand(1, 1), CancellationToken.None);
        await new AddToWishlistCommandHandler(repo).Handle(new AddToWishlistCommand(1, 6), CancellationToken.None);

        var count = await new RemoveFromWishlistCommandHandler(repo)
            .Handle(new RemoveFromWishlistCommand(1, 1), CancellationToken.None);

        Assert.Equal(1, count);
        var remaining = await repo.GetByUserIdAsync(1, CancellationToken.None);
        Assert.Equal(6, remaining!.Items.Single().ProductId);
    }

    private sealed class FakeCatalogModuleApi : ICatalogModuleApi
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
}
