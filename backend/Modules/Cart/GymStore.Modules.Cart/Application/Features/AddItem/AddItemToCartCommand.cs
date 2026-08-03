using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Application.Responses;

namespace GymStore.Modules.Cart.Application.Features.AddItem;

/// <summary>Sets the quantity of a product in the cart (creating the cart if needed).</summary>
public sealed record AddItemToCartCommand(long UserId, long ProductId, int Quantity) : ICommand<CartResponse>;
