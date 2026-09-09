using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Inventory.Application.Abstractions;

namespace GymStore.Modules.Inventory.Application.Features.UpdateStock;

internal sealed class UpdateStockCommandHandler : ICommandHandler<UpdateStockCommand, int>
{
    private readonly IInventoryRepository _repository;

    public UpdateStockCommandHandler(IInventoryRepository repository) => _repository = repository;

    public async Task<int> Handle(UpdateStockCommand command, CancellationToken ct)
    {
        var inventory = await _repository.GetByProductIdAsync(command.ProductId, ct);
        if (inventory is null)
        {
            throw new KeyNotFoundException("Product not found in inventory.");
        }

        inventory.QuantityAvailable += command.Change;
        inventory.LastUpdated = DateTime.UtcNow;

        await _repository.SaveChangesAsync(ct);
        return inventory.QuantityAvailable;
    }
}
