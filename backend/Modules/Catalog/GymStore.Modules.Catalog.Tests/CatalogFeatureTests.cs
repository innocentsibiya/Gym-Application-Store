using GymStore.Modules.Catalog.Application.Features.GetAllProducts;
using GymStore.Modules.Catalog.Application.Features.GetCategories;
using GymStore.Modules.Catalog.Application.Features.GetCategoryWithProducts;
using GymStore.Modules.Catalog.Application.Features.GetProductById;
using GymStore.Modules.Catalog.Application.Features.SearchProducts;
using GymStore.Modules.Catalog.Application.Responses;
using GymStore.Modules.Catalog.Infrastructure.Persistence;
using GymStore.Modules.Catalog.Infrastructure.Public;

namespace GymStore.Modules.Catalog.Tests;

public class CatalogFeatureTests
{
    [Fact]
    public async Task GetAllProducts_ReturnsActiveOnly_EnrichedAndCached()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var cache = new InMemoryProductCache();
        var handler = new GetAllProductsQueryHandler(new ProductRepository(ctx), cache);

        var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count); // the inactive product is excluded
        var db = result.Single(p => p.Id == 1);
        Assert.Equal("Dumbbell Pro", db.Name);
        Assert.Equal("Dumbbells", db.CategoryName);
        Assert.Equal(60m, db.Price);
        Assert.Equal(new[] { "db1-main.jpg", "db1-2.jpg" }, db.ImageUrls);
        Assert.NotNull(cache.All); // cached
    }

    [Fact]
    public async Task GetAllProducts_CacheHit_ReturnsCachedList()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var cache = new InMemoryProductCache();
        var cached = new List<ProductResponse> { new() { Id = 99, Name = "Cached" } };
        await cache.SetAllAsync(cached, CancellationToken.None);
        var handler = new GetAllProductsQueryHandler(new ProductRepository(ctx), cache);

        var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        Assert.Same(cached, result);
    }

    [Fact]
    public async Task GetProductById_Found_ReturnsResponse()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var handler = new GetProductByIdQueryHandler(new ProductRepository(ctx), new InMemoryProductCache());

        var result = await handler.Handle(new GetProductByIdQuery(2), CancellationToken.None);

        Assert.Equal("Kettlebell Max", result.Name);
        Assert.Equal(75m, result.Price);
        Assert.Single(result.ImageUrls);
    }

    [Fact]
    public async Task GetProductById_NotFound_ThrowsKeyNotFound()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var handler = new GetProductByIdQueryHandler(new ProductRepository(ctx), new InMemoryProductCache());

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new GetProductByIdQuery(999), CancellationToken.None));
    }

    [Fact]
    public async Task GetProductById_InactiveProduct_ThrowsKeyNotFound()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var handler = new GetProductByIdQueryHandler(new ProductRepository(ctx), new InMemoryProductCache());

        // Product 3 exists but IsActive == false, so the listing query treats it as absent.
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new GetProductByIdQuery(3), CancellationToken.None));
    }

    [Fact]
    public async Task SearchProducts_FiltersByTerm_AndPages()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var handler = new SearchProductsQueryHandler(new ProductRepository(ctx));

        var byName = await handler.Handle(new SearchProductsQuery("kettlebell", 1, 10), CancellationToken.None);
        Assert.Equal(1, byName.TotalCount);
        Assert.Equal("Kettlebell Max", byName.Products.Single().Name);

        // Both active products match brand/term "brand"; page size 1 returns first of 2.
        var paged = await handler.Handle(new SearchProductsQuery("brand", 1, 1), CancellationToken.None);
        Assert.Equal(2, paged.TotalCount);
        Assert.Single(paged.Products);
    }

    [Fact]
    public async Task GetCategories_ReturnsCategoriesWithSubcategories()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var handler = new GetCategoriesQueryHandler(new CategoryRepository(ctx));

        var result = await handler.Handle(new GetCategoriesQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        var strength = result.Single(c => c.Id == 1);
        Assert.Contains(strength.SubCategories, c => c.Id == 4);
    }

    [Fact]
    public async Task GetCategoryWithProducts_ReturnsProducts_OrNull()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var handler = new GetCategoryWithProductsQueryHandler(new CategoryRepository(ctx));

        var found = await handler.Handle(new GetCategoryWithProductsQuery(4), CancellationToken.None);
        Assert.NotNull(found);
        Assert.Equal(3, found!.Products.Count); // all products in category 4 (incl. inactive)

        var missing = await handler.Handle(new GetCategoryWithProductsQuery(999), CancellationToken.None);
        Assert.Null(missing);
    }

    [Fact]
    public async Task CatalogModuleApi_GetProductsByIds_ReturnsSummaries()
    {
        await using var ctx = TestSupport.NewSeededContext();
        var api = new CatalogModuleApi(new ProductRepository(ctx));

        var result = await api.GetProductsByIdsAsync(new long[] { 1, 2 }, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Dumbbell Pro", result[1].Name);
        Assert.Equal(60m, result[1].Price);
        Assert.Equal(2, result[1].ImageUrls.Count);
        Assert.Equal("Kettlebell Max", result[2].Name);
    }
}
