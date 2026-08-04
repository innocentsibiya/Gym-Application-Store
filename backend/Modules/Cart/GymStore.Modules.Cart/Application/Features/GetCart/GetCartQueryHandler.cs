using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Application.Responses;

namespace GymStore.Modules.Cart.Application.Features.GetCart;

/// <summary>
/// Cache-aside read of the cart. On a cache miss, loads (or lazily creates) the cart,
/// renders it, and repopulates the cache — mirroring the original CartService.GetCartAsync.
/// </summary>
internal sealed class GetCartQueryHandler : IQueryHandler<GetCartQuery, CartResponse>
{
    private readonly ICartRepository _repository;
    private readonly ICartCache _cache;
    private readonly IProductInfoProvider _productInfo;

    public GetCartQueryHandler(ICartRepository repository, ICartCache cache, IProductInfoProvider productInfo)
    {
        _repository = repository;
        _cache = cache;
        _productInfo = productInfo;
    }

    public async Task<CartResponse> Handle(GetCartQuery query, CancellationToken ct)
    {
        var cached = await _cache.GetAsync(query.UserId, ct);
        if (cached is not null)
        {
            return cached;
        }

        var cart = await _repository.GetByUserIdAsync(query.UserId, ct)
                   ?? await _repository.CreateAsync(query.UserId, ct);

        var response = await CartResponseFactory.CreateAsync(cart, _productInfo, ct);
        await _cache.SetAsync(query.UserId, response, ct);
        return response;
    }
}
