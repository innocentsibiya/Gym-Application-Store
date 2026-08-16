using backend.Models;

namespace backend.DTO
{
    public class PlaceOrderRequest
    {
        public int UserId { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }

        public List<OrderItem> Items { get; set; } = new();

        public PaymentDto Payment { get; set; } = new();
    }
}
