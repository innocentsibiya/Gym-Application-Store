using GymStore.Modules.Cart.Application.Responses;

namespace GymStore.Modules.Cart.Application.Abstractions;

/// <summary>Cache-aside store for rendered cart responses (backed by Redis in production).</summary>
public interface ICartCache
{
    Task<CartResponse?> GetAsync(long userId, CancellationToken ct);
    Task SetAsync(long userId, CartResponse cart, CancellationToken ct);
    Task RemoveAsync(long userId, CancellationToken ct);
}
