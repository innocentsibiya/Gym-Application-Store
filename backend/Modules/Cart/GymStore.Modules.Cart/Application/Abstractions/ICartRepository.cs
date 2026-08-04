using GymStore.Modules.Cart.Domain;

namespace GymStore.Modules.Cart.Application.Abstractions;

/// <summary>Persistence operations for the Cart aggregate (module-internal).</summary>
public interface ICartRepository
{
    Task<Domain.Cart?> GetByUserIdAsync(long userId, CancellationToken ct);
    Task<Domain.Cart> CreateAsync(long userId, CancellationToken ct);
    Task<Domain.Cart> AddOrUpdateItemAsync(long userId, long productId, int quantity, CancellationToken ct);
    Task<Domain.Cart> RemoveItemAsync(long userId, long productId, CancellationToken ct);
    Task ClearAsync(long userId, CancellationToken ct);
}
