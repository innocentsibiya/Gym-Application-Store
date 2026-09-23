using GymStore.BuildingBlocks.Cqrs;

namespace GymStore.Modules.Inventory.Application.Features.UpdateStock;

/// <summary>Adjusts a product's stock by <paramref name="Change"/>. Returns the new quantity.</summary>
public sealed record UpdateStockCommand(long ProductId, int Change) : ICommand<int>;
