using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class CartItem
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(Cart))]
        public long CartId { get; set; }

        [Required, ForeignKey(nameof(Product))]
        public long ProductId { get; set; }

        public int Quantity { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
