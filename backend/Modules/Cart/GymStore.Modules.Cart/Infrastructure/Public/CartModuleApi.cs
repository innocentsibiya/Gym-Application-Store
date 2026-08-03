using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Contracts;

namespace GymStore.Modules.Cart.Infrastructure.Public;

/// <summary>
/// Implements the cross-module contract. Reads come straight from the repository
/// (authoritative DB state, not the cache) so callers like Ordering never act on stale data.
/// </summary>
internal sealed class CartModuleApi : ICartModuleApi
{
    private readonly ICartRepository _repository;
    private readonly ICartCache _cache;

    public CartModuleApi(ICartRepository repository, ICartCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<CartContentsDto?> GetCartAsync(long userId, CancellationToken ct = default)
    {
        var cart = await _repository.GetByUserIdAsync(userId, ct);
        if (cart is null)
        {
            return null;
        }

        var lines = cart.Items
            .Select(i => new CartLineDto(i.ProductId, i.Quantity))
            .ToList();

        return new CartContentsDto(cart.Id, cart.UserId, lines);
    }

    public async Task ClearCartAsync(long userId, CancellationToken ct = default)
    {
        await _repository.ClearAsync(userId, ct);
        await _cache.RemoveAsync(userId, ct); // keep the cached cart consistent after checkout
    }
}
