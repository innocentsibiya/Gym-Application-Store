using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Abstractions;

namespace GymStore.Modules.Addresses.Application.Features.AddAddress;

internal sealed class AddAddressCommandHandler : ICommandHandler<AddAddressCommand, long>
{
    private readonly IAddressRepository _repository;
    private readonly IAddressCache _cache;

    public AddAddressCommandHandler(IAddressRepository repository, IAddressCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<long> Handle(AddAddressCommand command, CancellationToken ct)
    {
        var dto = command.Address;
        var existing = await _repository.GetByUserIdAsync(dto.UserId, ct);

        if (existing.Any(a =>
                a.Street == dto.Street &&
                a.City == dto.City &&
                a.Province == dto.Province &&
                a.PostalCode == dto.PostalCode &&
                a.Country == dto.Country &&
                a.AddressType == dto.AddressType))
        {
            throw new InvalidOperationException("Address already exists.");
        }

        // A new default address clears the default flag on the user's other addresses of that type.
        if (dto.IsDefault)
        {
            foreach (var addr in existing.Where(a => a.AddressType == dto.AddressType))
            {
                addr.IsDefault = false;
            }
        }

        var address = new Domain.Address
        {
            UserId = dto.UserId,
            Street = dto.Street,
            City = dto.City,
            Province = dto.Province,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            AddressType = dto.AddressType,
            IsDefault = dto.IsDefault
        };

        await _repository.AddAsync(address, ct);
        await _repository.SaveChangesAsync(ct);
        await _cache.RemoveAsync(dto.UserId, ct);

        return address.Id;
    }
}
