using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Abstractions;

namespace GymStore.Modules.Addresses.Application.Features.UpdateAddress;

internal sealed class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand, bool>
{
    private readonly IAddressRepository _repository;
    private readonly IAddressCache _cache;

    public UpdateAddressCommandHandler(IAddressRepository repository, IAddressCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateAddressCommand command, CancellationToken ct)
    {
        var dto = command.Address;
        var address = await _repository.GetByIdAsync(dto.Id, ct);
        if (address is null)
        {
            throw new KeyNotFoundException("Address not found.");
        }

        if (dto.IsDefault)
        {
            var userAddresses = await _repository.GetByUserIdAsync(dto.UserId, ct);
            foreach (var addr in userAddresses.Where(a => a.AddressType == dto.AddressType && a.Id != dto.Id))
            {
                addr.IsDefault = false;
            }
        }

        address.Street = dto.Street;
        address.City = dto.City;
        address.Province = dto.Province;
        address.PostalCode = dto.PostalCode;
        address.Country = dto.Country;
        address.AddressType = dto.AddressType;
        address.IsDefault = dto.IsDefault;

        await _repository.SaveChangesAsync(ct);
        await _cache.RemoveAsync(dto.UserId, ct);
        return true;
    }
}
