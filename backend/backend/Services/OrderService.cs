using backend.Interfaces;
using backend.IRepository;
using backend.Models;
using backend.Repository;

namespace backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> PlaceOrderAsync(int userId, int shippingAddressId, int billingAddressId)
        {
            return await _orderRepository.PlaceOrderAsync(userId, shippingAddressId, billingAddressId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _orderRepository.GetUserOrdersAsync(userId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersByYearAsync(int userId, int year)
        {
            return await _orderRepository.GetUserOrdersByYearAsync(userId, year);
        }
    }
}