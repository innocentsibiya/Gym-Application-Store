using GymStore.Modules.Addresses.Application.Abstractions;
using GymStore.Modules.Addresses.Application.Contracts;
using GymStore.Modules.Addresses.Application.Features.AddAddress;
using GymStore.Modules.Addresses.Application.Features.GetUserAddresses;
using GymStore.Modules.Addresses.Application.Features.UpdateAddress;
using GymStore.Modules.Addresses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Addresses.Tests;

public class AddressesTests
{
    private static AddressesDbContext NewContext() =>
        new(new DbContextOptionsBuilder<AddressesDbContext>()
            .UseInMemoryDatabase($"addresses-tests-{Guid.NewGuid()}")
            .Options);

    private static AddressDto Sample(bool isDefault = false, string type = "Shipping") => new()
    {
        UserId = 1, Street = "1 Main St", City = "Cape Town", Province = "WC",
        PostalCode = "8000", Country = "ZA", AddressType = type, IsDefault = isDefault
    };

    [Fact]
    public async Task GetUserAddresses_CachesAndReturnsDtos()
    {
        await using var ctx = NewContext();
        var repo = new AddressRepository(ctx);
        var cache = new InMemoryAddressCache();
        await new AddAddressCommandHandler(repo, cache).Handle(new AddAddressCommand(Sample()), CancellationToken.None);

        var handler = new GetUserAddressesQueryHandler(repo, cache);
        var result = await handler.Handle(new GetUserAddressesQuery(1), CancellationToken.None);

        var dto = Assert.Single(result);
        Assert.Equal("Cape Town", dto.City);
        Assert.True(cache.Store.ContainsKey(1)); // cached
    }

    [Fact]
    public async Task AddAddress_Duplicate_Throws()
    {
        await using var ctx = NewContext();
        var repo = new AddressRepository(ctx);
        var cache = new InMemoryAddressCache();
        var handler = new AddAddressCommandHandler(repo, cache);
        await handler.Handle(new AddAddressCommand(Sample()), CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new AddAddressCommand(Sample()), CancellationToken.None));
    }

    [Fact]
    public async Task AddAddress_NewDefault_ClearsPreviousDefaultOfSameType()
    {
        await using var ctx = NewContext();
        var repo = new AddressRepository(ctx);
        var cache = new InMemoryAddressCache();
        var handler = new AddAddressCommandHandler(repo, cache);

        await handler.Handle(new AddAddressCommand(Sample(isDefault: true)), CancellationToken.None);
        var second = Sample(isDefault: true);
        second.Street = "2 Other Rd";
        await handler.Handle(new AddAddressCommand(second), CancellationToken.None);

        var all = await repo.GetByUserIdAsync(1, CancellationToken.None);
        Assert.Equal(2, all.Count);
        Assert.Single(all, a => a.IsDefault); // exactly one default remains
        Assert.Equal("2 Other Rd", all.Single(a => a.IsDefault).Street);
        Assert.True(cache.RemoveCount >= 2); // cache invalidated on each add
    }

    [Fact]
    public async Task UpdateAddress_Missing_ThrowsKeyNotFound()
    {
        await using var ctx = NewContext();
        var handler = new UpdateAddressCommandHandler(new AddressRepository(ctx), new InMemoryAddressCache());
        var dto = Sample();
        dto.Id = 999;

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new UpdateAddressCommand(dto), CancellationToken.None));
    }

    private sealed class InMemoryAddressCache : IAddressCache
    {
        public Dictionary<long, IReadOnlyList<AddressDto>> Store { get; } = new();
        public int RemoveCount { get; private set; }

        public Task<IReadOnlyList<AddressDto>?> GetAsync(long userId, CancellationToken ct) =>
            Task.FromResult(Store.TryGetValue(userId, out var v) ? v : null);

        public Task SetAsync(long userId, IReadOnlyList<AddressDto> addresses, CancellationToken ct)
        {
            Store[userId] = addresses;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(long userId, CancellationToken ct)
        {
            Store.Remove(userId);
            RemoveCount++;
            return Task.CompletedTask;
        }
    }
}
