using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Wishlist.Application.Responses;

namespace GymStore.Modules.Wishlist.Application.Features.GetWishlist;

public sealed record GetWishlistQuery(long UserId) : IQuery<WishlistResponse>;
