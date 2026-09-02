using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Application.Responses;
using GymStore.Modules.Catalog.Domain;
using GymStore.Modules.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Catalog.Tests;

internal static class TestSupport
{
    /// <summary>A CatalogDbContext on a fresh in-memory store, seeded with a small catalog.</summary>
    public static CatalogDbContext NewSeededContext()
    {
        var ctx = new CatalogDbContext(new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase($"catalog-tests-{Guid.NewGuid()}")
            .Options);

        var strength = new Category { Id = 1, Name = "Strength", Slug = "strength" };
        var dumbbells = new Category { Id = 4, Name = "Dumbbells", Slug = "dumbbells", ParentCategoryId = 1 };
        ctx.Categories.AddRange(strength, dumbbells);

        ctx.Products.AddRange(
            new Product
            {
                Id = 1, CategoryId = 4, Name = "Dumbbell Pro", Slug = "dumbbell-pro",
                Description = "A solid dumbbell", Brand = "BrandA", Price = 60m, SKU = "DB-1",
                StockQuantity = 10, IsActive = true,
                Images = { new ProductImage { Id = 1, ProductId = 1, ImageUrl = "db1-main.jpg" },
                           new ProductImage { Id = 2, ProductId = 1, ImageUrl = "db1-2.jpg" } }
            },
            new Product
            {
                Id = 2, CategoryId = 4, Name = "Kettlebell Max", Slug = "kettlebell-max",
                Description = "Cast iron kettlebell", Brand = "BrandB", Price = 75m, SKU = "KB-1",
                StockQuantity = 5, IsActive = true,
                Images = { new ProductImage { Id = 3, ProductId = 2, ImageUrl = "kb1-main.jpg" } }
            },
            new Product
            {
                Id = 3, CategoryId = 4, Name = "Retired Bar", Slug = "retired-bar",
                Description = "Discontinued", Brand = "BrandA", Price = 20m, SKU = "RB-1",
                StockQuantity = 0, IsActive = false
            });

        ctx.SaveChanges();
        return ctx;
    }
}

/// <summary>In-memory <see cref="IProductCache"/> for handler tests.</summary>
internal sealed class InMemoryProductCache : IProductCache
{
    public IReadOnlyList<ProductResponse>? All { get; private set; }
    public Dictionary<long, ProductResponse> ById { get; } = new();

    public Task<IReadOnlyList<ProductResponse>?> GetAllAsync(CancellationToken ct) => Task.FromResult(All);
    public Task SetAllAsync(IReadOnlyList<ProductResponse> products, CancellationToken ct) { All = products; return Task.CompletedTask; }

    public Task<ProductResponse?> GetByIdAsync(long id, CancellationToken ct) =>
        Task.FromResult(ById.TryGetValue(id, out var p) ? p : null);
    public Task SetByIdAsync(long id, ProductResponse product, CancellationToken ct) { ById[id] = product; return Task.CompletedTask; }
}
