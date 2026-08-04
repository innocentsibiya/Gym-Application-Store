using backend.Data;
using backend.IRepository;
using backend.Models;
using GymStore.Modules.Cart.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly GymStoreContext _context;
        private readonly ICartModuleApi _cartApi;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(GymStoreContext context, ICartModuleApi cartApi, ILogger<OrderRepository> logger)
            : base(context)
        {
            _context = context;
            _cartApi = cartApi;
            _logger = logger;
        }

        public async Task<Order> PlaceOrderAsync(int userId, int shippingAddressId, int billingAddressId)
        {
            // Cart contents come from the Cart module via its public contract — Ordering no
            // longer reads the Carts/CartItems tables directly.
            var cart = await _cartApi.GetCartAsync(userId);
            if (cart is null || cart.Items.Count == 0)
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            // Pricing is Catalog data; resolve current unit prices (matches prior behavior).
            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            var prices = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Price);

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
                    UnitPrice = prices[i.ProductId]
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear the cart after the order commits. This is a cross-module call (separate
            // DbContext) so it can't share the order's transaction; treat it as best-effort.
            try
            {
                await _cartApi.ClearCartAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Order {OrderId} was placed but clearing the cart for user {UserId} failed.",
                    order.Id, userId);
            }

            return order;
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
