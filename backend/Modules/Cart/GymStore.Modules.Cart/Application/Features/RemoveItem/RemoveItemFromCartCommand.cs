using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Application.Responses;

namespace GymStore.Modules.Cart.Application.Features.RemoveItem;

public sealed record RemoveItemFromCartCommand(long UserId, long ProductId) : ICommand<CartResponse>;
