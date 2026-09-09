using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Abstractions;
using GymStore.Modules.Addresses.Application.Contracts;

namespace GymStore.Modules.Addresses.Application.Features.GetDefaultAddress;

internal sealed class GetDefaultAddressQueryHandler : IQueryHandler<GetDefaultAddressQuery, AddressDto?>
{
    private readonly IAddressRepository _repository;

    public GetDefaultAddressQueryHandler(IAddressRepository repository) => _repository = repository;

    public async Task<AddressDto?> Handle(GetDefaultAddressQuery query, CancellationToken ct)
    {
        var address = await _repository.GetDefaultAsync(query.UserId, query.Type, ct);
        return address is null ? null : AddressDtoFactory.ToDto(address);
    }
}
