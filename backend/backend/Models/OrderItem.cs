using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class OrderItem
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(Order))]
        public long OrderId { get; set; }

        [Required, ForeignKey(nameof(Product))]
        public long ProductId { get; set; }

        public int Quantity { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountApplied { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
