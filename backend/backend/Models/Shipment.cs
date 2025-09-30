using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Shipment
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(Order))]
        public long OrderId { get; set; }

        [Required, MaxLength(100)]
        public string Carrier { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "InTransit";

        public Order Order { get; set; }
    }
}
