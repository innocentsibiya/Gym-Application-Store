using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Payment
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(Order))]
        public long OrderId { get; set; }

        [Required, MaxLength(50)]
        public string PaymentMethod { get; set; } = "CreditCard";

        [Required, MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending";

        [MaxLength(100)]
        public string? TransactionId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Order Order { get; set; }
    }
}
