using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Inventory.Application.Abstractions;

namespace GymStore.Modules.Inventory.Application.Features.GetStock;

internal sealed class GetStockQueryHandler : IQueryHandler<GetStockQuery, int>
{
    private readonly IInventoryRepository _repository;

    public GetStockQueryHandler(IInventoryRepository repository) => _repository = repository;

    public async Task<int> Handle(GetStockQuery query, CancellationToken ct)
    {
        var inventory = await _repository.GetByProductIdAsync(query.ProductId, ct);
        return inventory?.QuantityAvailable ?? 0;
    }
}
