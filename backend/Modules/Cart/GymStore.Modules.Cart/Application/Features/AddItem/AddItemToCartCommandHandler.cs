using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Application.Responses;

namespace GymStore.Modules.Cart.Application.Features.AddItem;

internal sealed class AddItemToCartCommandHandler : ICommandHandler<AddItemToCartCommand, CartResponse>
{
    private readonly ICartRepository _repository;
    private readonly ICartCache _cache;
    private readonly IProductInfoProvider _productInfo;

    public AddItemToCartCommandHandler(ICartRepository repository, ICartCache cache, IProductInfoProvider productInfo)
    {
        _repository = repository;
        _cache = cache;
        _productInfo = productInfo;
    }

    public async Task<CartResponse> Handle(AddItemToCartCommand command, CancellationToken ct)
    {
        var cart = await _repository.AddOrUpdateItemAsync(command.UserId, command.ProductId, command.Quantity, ct);
        var response = await CartResponseFactory.CreateAsync(cart, _productInfo, ct);
        await _cache.SetAsync(command.UserId, response, ct);
        return response;
    }
}
