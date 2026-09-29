using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Abstractions;
using GymStore.Modules.Addresses.Application.Contracts;

namespace GymStore.Modules.Addresses.Application.Features.GetUserAddresses;

/// <summary>Cache-aside list of a user's addresses (mirrors the original AddressService).</summary>
internal sealed class GetUserAddressesQueryHandler
    : IQueryHandler<GetUserAddressesQuery, IReadOnlyList<AddressDto>>
{
    private readonly IAddressRepository _repository;
    private readonly IAddressCache _cache;

    public GetUserAddressesQueryHandler(IAddressRepository repository, IAddressCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<AddressDto>> Handle(GetUserAddressesQuery query, CancellationToken ct)
    {
        var cached = await _cache.GetAsync(query.UserId, ct);
        if (cached is not null)
        {
            return cached;
        }

        var addresses = await _repository.GetByUserIdAsync(query.UserId, ct);
        var dtos = addresses.Select(AddressDtoFactory.ToDto).ToList();

        await _cache.SetAsync(query.UserId, dtos, ct);
        return dtos;
    }
}
