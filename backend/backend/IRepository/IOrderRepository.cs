using backend.Models;

namespace backend.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> PlaceOrderAsync(int userId, int shippingAddressId, int billingAddressId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<Order>> GetUserOrdersByYearAsync(int userId, int year);

    }
}