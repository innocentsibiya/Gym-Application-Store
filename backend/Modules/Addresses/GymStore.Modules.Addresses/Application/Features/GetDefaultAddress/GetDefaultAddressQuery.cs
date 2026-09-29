using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Contracts;

namespace GymStore.Modules.Addresses.Application.Features.GetDefaultAddress;

public sealed record GetDefaultAddressQuery(long UserId, string Type) : IQuery<AddressDto?>;
