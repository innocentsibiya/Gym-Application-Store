using GymStore.Modules.Inventory.Application.Features.GetStock;
using GymStore.Modules.Inventory.Application.Features.UpdateStock;
using GymStore.Modules.Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Inventory.Tests;

public class InventoryTests
{
    private static InventoryDbContext NewContext(params Domain.Inventory[] seed)
    {
        var ctx = new InventoryDbContext(new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase($"inventory-tests-{Guid.NewGuid()}")
            .Options);
        if (seed.Length > 0)
        {
            ctx.Inventories.AddRange(seed);
            ctx.SaveChanges();
        }
        return ctx;
    }

    [Fact]
    public async Task GetStock_ExistingProduct_ReturnsQuantity()
    {
        await using var ctx = NewContext(new Domain.Inventory { Id = 1, ProductId = 5, QuantityAvailable = 25 });
        var handler = new GetStockQueryHandler(new InventoryRepository(ctx));

        var stock = await handler.Handle(new GetStockQuery(5), CancellationToken.None);

        Assert.Equal(25, stock);
    }

    [Fact]
    public async Task GetStock_MissingProduct_ReturnsZero()
    {
        await using var ctx = NewContext();
        var handler = new GetStockQueryHandler(new InventoryRepository(ctx));

        var stock = await handler.Handle(new GetStockQuery(999), CancellationToken.None);

        Assert.Equal(0, stock);
    }

    [Fact]
    public async Task UpdateStock_AdjustsQuantity_AndTimestamp()
    {
        await using var ctx = NewContext(new Domain.Inventory
        {
            Id = 1, ProductId = 5, QuantityAvailable = 25, LastUpdated = DateTime.UtcNow.AddDays(-1)
        });
        var repo = new InventoryRepository(ctx);
        var handler = new UpdateStockCommandHandler(repo);

        var newQty = await handler.Handle(new UpdateStockCommand(5, -3), CancellationToken.None);

        Assert.Equal(22, newQty);
        var persisted = await ctx.Inventories.SingleAsync();
        Assert.Equal(22, persisted.QuantityAvailable);
        Assert.True(persisted.LastUpdated > DateTime.UtcNow.AddMinutes(-1));
    }

    [Fact]
    public async Task UpdateStock_MissingProduct_ThrowsKeyNotFound()
    {
        await using var ctx = NewContext();
        var handler = new UpdateStockCommandHandler(new InventoryRepository(ctx));

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new UpdateStockCommand(999, 5), CancellationToken.None));
    }
}
