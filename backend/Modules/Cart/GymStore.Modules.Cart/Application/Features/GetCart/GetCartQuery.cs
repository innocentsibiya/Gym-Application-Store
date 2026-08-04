using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Application.Responses;

namespace GymStore.Modules.Cart.Application.Features.GetCart;

public sealed record GetCartQuery(long UserId) : IQuery<CartResponse>;
