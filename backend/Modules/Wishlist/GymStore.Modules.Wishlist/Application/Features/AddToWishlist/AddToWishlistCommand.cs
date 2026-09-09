using GymStore.BuildingBlocks.Cqrs;

namespace GymStore.Modules.Wishlist.Application.Features.AddToWishlist;

/// <summary>Adds a product to the user's wishlist (no-op if already present). Returns the item count.</summary>
public sealed record AddToWishlistCommand(long UserId, long ProductId) : ICommand<int>;
