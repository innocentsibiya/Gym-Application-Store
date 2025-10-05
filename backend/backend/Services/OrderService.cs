using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly GymStoreContext _context;
        public OrderService(GymStoreContext context) => _context = context;

        public async Task<Order> PlaceOrderAsync(int userId, int shippingAddressId, int billingAddressId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Items).ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.Items.Any())
                    throw new InvalidOperationException("Cart is empty.");

                var order = new Order
                {
                    UserId = userId,
                    ShippingAddressId = shippingAddressId,
                    BillingAddressId = billingAddressId,
                    CreatedAt = DateTime.UtcNow,
                    Items = cart.Items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.Product.Price
                    }).ToList()
                };

                _context.Orders.Add(order);

                // Clear cart
                _context.CartItems.RemoveRange(cart.Items);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}