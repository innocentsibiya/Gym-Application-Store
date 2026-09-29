using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Contracts;

namespace GymStore.Modules.Addresses.Application.Features.AddAddress;

/// <summary>Adds a new address for a user. Returns the new address id.</summary>
public sealed record AddAddressCommand(AddressDto Address) : ICommand<long>;
