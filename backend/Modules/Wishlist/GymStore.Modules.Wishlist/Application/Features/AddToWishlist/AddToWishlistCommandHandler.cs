using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Wishlist.Application.Abstractions;

namespace GymStore.Modules.Wishlist.Application.Features.AddToWishlist;

internal sealed class AddToWishlistCommandHandler : ICommandHandler<AddToWishlistCommand, int>
{
    private readonly IWishlistRepository _repository;

    public AddToWishlistCommandHandler(IWishlistRepository repository) => _repository = repository;

    public async Task<int> Handle(AddToWishlistCommand command, CancellationToken ct)
    {
        var wishlist = await _repository.AddItemAsync(command.UserId, command.ProductId, ct);
        return wishlist.Items.Count;
    }
}
