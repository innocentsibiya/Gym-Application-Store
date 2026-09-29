using GymStore.BuildingBlocks.Cqrs;

namespace GymStore.Modules.Wishlist.Application.Features.RemoveFromWishlist;

/// <summary>Removes a product from the user's wishlist (no-op if absent). Returns the item count.</summary>
public sealed record RemoveFromWishlistCommand(long UserId, long ProductId) : ICommand<int>;
