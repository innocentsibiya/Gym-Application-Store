using backend.Models;

namespace backend.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(int userId, int shippingAddressId, int billingAddressId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<Order>> GetUserOrdersByYearAsync(int userId, int year);
    }
}