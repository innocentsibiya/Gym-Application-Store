using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Wishlist.Application.Abstractions;

namespace GymStore.Modules.Wishlist.Application.Features.RemoveFromWishlist;

internal sealed class RemoveFromWishlistCommandHandler : ICommandHandler<RemoveFromWishlistCommand, int>
{
    private readonly IWishlistRepository _repository;

    public RemoveFromWishlistCommandHandler(IWishlistRepository repository) => _repository = repository;

    public async Task<int> Handle(RemoveFromWishlistCommand command, CancellationToken ct)
    {
        var wishlist = await _repository.RemoveItemAsync(command.UserId, command.ProductId, ct);
        return wishlist.Items.Count;
    }
}
