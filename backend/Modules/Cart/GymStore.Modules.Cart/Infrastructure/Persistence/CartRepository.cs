using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Cart.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Cart.Infrastructure.Persistence;

internal sealed class CartRepository : ICartRepository
{
    private readonly CartDbContext _db;

    public CartRepository(CartDbContext db) => _db = db;

    public Task<Domain.Cart?> GetByUserIdAsync(long userId, CancellationToken ct) =>
        _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId, ct);

    public async Task<Domain.Cart> CreateAsync(long userId, CancellationToken ct)
    {
        var cart = new Domain.Cart { UserId = userId };
        _db.Carts.Add(cart);
        await _db.SaveChangesAsync(ct);
        return cart;
    }

    public async Task<Domain.Cart> AddOrUpdateItemAsync(long userId, long productId, int quantity, CancellationToken ct)
    {
        var cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId, ct);
        if (cart is null)
        {
            cart = new Domain.Cart { UserId = userId };
            _db.Carts.Add(cart);
        }

        var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing is not null)
        {
            existing.Quantity = quantity; // set (not increment) — matches original behavior
        }
        else
        {
            cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });
        }

        await _db.SaveChangesAsync(ct);
        return cart;
    }

    public async Task<Domain.Cart> RemoveItemAsync(long userId, long productId, CancellationToken ct)
    {
        var cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId, ct);
        if (cart is null)
        {
            throw new InvalidOperationException("Cart not found");
        }

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is not null)
        {
            cart.Items.Remove(item);
            _db.CartItems.Remove(item);
        }

        await _db.SaveChangesAsync(ct);
        return cart;
    }

    public async Task ClearAsync(long userId, CancellationToken ct)
    {
        var cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId, ct);
        if (cart is null || cart.Items.Count == 0)
        {
            return;
        }

        _db.CartItems.RemoveRange(cart.Items);
        cart.Items.Clear();
        await _db.SaveChangesAsync(ct);
    }
}
