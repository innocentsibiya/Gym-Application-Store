using GymStore.Modules.Wishlist.Application.Abstractions;
using GymStore.Modules.Wishlist.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Wishlist.Infrastructure.Persistence;

internal sealed class WishlistRepository : IWishlistRepository
{
    private readonly WishlistDbContext _db;

    public WishlistRepository(WishlistDbContext db) => _db = db;

    public Task<Domain.Wishlist?> GetByUserIdAsync(long userId, CancellationToken ct) =>
        _db.Wishlists.Include(w => w.Items).FirstOrDefaultAsync(w => w.UserId == userId, ct);

    public async Task<Domain.Wishlist> CreateAsync(long userId, CancellationToken ct)
    {
        var wishlist = new Domain.Wishlist { UserId = userId };
        _db.Wishlists.Add(wishlist);
        await _db.SaveChangesAsync(ct);
        return wishlist;
    }

    public async Task<Domain.Wishlist> AddItemAsync(long userId, long productId, CancellationToken ct)
    {
        var wishlist = await GetOrCreateAsync(userId, ct);
        if (wishlist.Items.All(i => i.ProductId != productId))
        {
            wishlist.Items.Add(new WishlistItem { ProductId = productId, WishlistId = wishlist.Id });
            await _db.SaveChangesAsync(ct);
        }

        return wishlist;
    }

    public async Task<Domain.Wishlist> RemoveItemAsync(long userId, long productId, CancellationToken ct)
    {
        var wishlist = await GetOrCreateAsync(userId, ct);
        var item = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is not null)
        {
            wishlist.Items.Remove(item);
            _db.WishlistItems.Remove(item);
            await _db.SaveChangesAsync(ct);
        }

        return wishlist;
    }

    private async Task<Domain.Wishlist> GetOrCreateAsync(long userId, CancellationToken ct) =>
        await GetByUserIdAsync(userId, ct) ?? await CreateAsync(userId, ct);
}
