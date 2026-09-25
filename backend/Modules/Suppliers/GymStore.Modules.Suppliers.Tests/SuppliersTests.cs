using GymStore.Modules.Suppliers.Application.Features.GetSupplierById;
using GymStore.Modules.Suppliers.Application.Features.GetSuppliers;
using GymStore.Modules.Suppliers.Domain;
using GymStore.Modules.Suppliers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Suppliers.Tests;

public class SuppliersTests
{
    private static SuppliersDbContext NewContext(params Supplier[] seed)
    {
        var ctx = new SuppliersDbContext(new DbContextOptionsBuilder<SuppliersDbContext>()
            .UseInMemoryDatabase($"suppliers-tests-{Guid.NewGuid()}")
            .Options);
        if (seed.Length > 0)
        {
            ctx.Suppliers.AddRange(seed);
            ctx.SaveChanges();
        }
        return ctx;
    }

    [Fact]
    public async Task GetSuppliers_ReturnsAll_AsResponses()
    {
        await using var ctx = NewContext(
            new Supplier { Id = 1, Name = "Acme", Email = "a@acme.test" },
            new Supplier { Id = 2, Name = "Globex", ContactName = "Jane" });
        var handler = new GetSuppliersQueryHandler(new SupplierRepository(ctx));

        var result = await handler.Handle(new GetSuppliersQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Acme", result.Single(s => s.Id == 1).Name);
        Assert.Equal("Jane", result.Single(s => s.Id == 2).ContactName);
    }

    [Fact]
    public async Task GetSupplierById_ReturnsSupplier_OrNull()
    {
        await using var ctx = NewContext(new Supplier { Id = 1, Name = "Acme" });
        var handler = new GetSupplierByIdQueryHandler(new SupplierRepository(ctx));

        var found = await handler.Handle(new GetSupplierByIdQuery(1), CancellationToken.None);
        Assert.NotNull(found);
        Assert.Equal("Acme", found!.Name);

        var missing = await handler.Handle(new GetSupplierByIdQuery(999), CancellationToken.None);
        Assert.Null(missing);
    }
}
