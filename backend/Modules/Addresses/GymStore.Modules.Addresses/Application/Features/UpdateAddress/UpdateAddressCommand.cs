using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Contracts;

namespace GymStore.Modules.Addresses.Application.Features.UpdateAddress;

/// <summary>Updates an existing address.</summary>
public sealed record UpdateAddressCommand(AddressDto Address) : ICommand<bool>;
