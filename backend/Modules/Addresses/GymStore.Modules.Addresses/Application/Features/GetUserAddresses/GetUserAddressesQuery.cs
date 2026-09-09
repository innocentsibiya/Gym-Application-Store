using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Contracts;

namespace GymStore.Modules.Addresses.Application.Features.GetUserAddresses;

public sealed record GetUserAddressesQuery(long UserId) : IQuery<IReadOnlyList<AddressDto>>;
