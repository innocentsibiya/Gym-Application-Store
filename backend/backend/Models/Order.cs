using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Order
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public long UserId { get; set; }

        [Required, MaxLength(50)]
        public string OrderStatus { get; set; } = "Pending";

        [Required, MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending";

        [Required, ForeignKey("ShippingAddress")]
        public long ShippingAddressId { get; set; }

        [Required, ForeignKey("BillingAddress")]
        public long BillingAddressId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Discount { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
