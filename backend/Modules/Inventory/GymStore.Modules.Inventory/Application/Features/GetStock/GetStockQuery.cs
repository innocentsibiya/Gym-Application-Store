using GymStore.BuildingBlocks.Cqrs;

namespace GymStore.Modules.Inventory.Application.Features.GetStock;

/// <summary>Returns the available stock for a product (0 if it has no inventory record).</summary>
public sealed record GetStockQuery(long ProductId) : IQuery<int>;
