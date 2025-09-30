using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Inventory
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(Product))]
        public long ProductId { get; set; }

        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }

        [ForeignKey(nameof(Supplier))]
        public long? SupplierId { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public Product Product { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
