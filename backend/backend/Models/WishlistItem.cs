using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class WishlistItem
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(Wishlist))]
        public long WishlistId { get; set; }

        [Required, ForeignKey(nameof(Product))]
        public long ProductId { get; set; }

        public Wishlist Wishlist { get; set; }
        public Product Product { get; set; }
    }
}
