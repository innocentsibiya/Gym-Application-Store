using GymStore.BuildingBlocks.Cqrs;

namespace GymStore.Modules.Addresses.Application.Features.RemoveAddress;

/// <summary>Removes an address by id (no-op if it doesn't exist). Returns whether it was removed.</summary>
public sealed record RemoveAddressCommand(long Id) : ICommand<bool>;
