using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Application.Responses;

namespace GymStore.Modules.Cart.Application.Features.RemoveItem;

internal sealed class RemoveItemFromCartCommandHandler : ICommandHandler<RemoveItemFromCartCommand, CartResponse>
{
    private readonly ICartRepository _repository;
    private readonly ICartCache _cache;
    private readonly IProductInfoProvider _productInfo;

    public RemoveItemFromCartCommandHandler(ICartRepository repository, ICartCache cache, IProductInfoProvider productInfo)
    {
        _repository = repository;
        _cache = cache;
        _productInfo = productInfo;
    }

    public async Task<CartResponse> Handle(RemoveItemFromCartCommand command, CancellationToken ct)
    {
        var cart = await _repository.RemoveItemAsync(command.UserId, command.ProductId, ct);
        var response = await CartResponseFactory.CreateAsync(cart, _productInfo, ct);
        await _cache.SetAsync(command.UserId, response, ct);
        return response;
    }
}
