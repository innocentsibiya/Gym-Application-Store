namespace GymStore.Modules.Wishlist.Application.Abstractions;

/// <summary>Persistence operations for the Wishlist aggregate (module-internal).</summary>
public interface IWishlistRepository
{
    Task<Domain.Wishlist?> GetByUserIdAsync(long userId, CancellationToken ct);
    Task<Domain.Wishlist> CreateAsync(long userId, CancellationToken ct);
    Task<Domain.Wishlist> AddItemAsync(long userId, long productId, CancellationToken ct);
    Task<Domain.Wishlist> RemoveItemAsync(long userId, long productId, CancellationToken ct);
}
