using backend.Data;
using backend.DTO;
using backend.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly GymStoreContext _context;

        public CartRepository(GymStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CartDto?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Items = c.Items.Select(i => new CartItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        ImageUrls = i.Product.Images.Select(img => img.ImageUrl).ToList(),
                        Price = i.Product.Price,
                        Quantity = i.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CartDto> AddItemAsync(int userId, int productId, int quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _context.Carts.AddAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
                existingItem.Quantity = quantity;
            else
                cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });

            await _context.SaveChangesAsync();
            return await GetCartByUserIdAsync(userId) ?? throw new Exception("Cart not found after update");
        }

        public async Task<CartDto> RemoveItemAsync(int userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new Exception("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
                _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();
            return await GetCartByUserIdAsync(userId) ?? throw new Exception("Cart not found after removal");
        }
    }
}