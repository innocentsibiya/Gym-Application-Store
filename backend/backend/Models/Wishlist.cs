using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Wishlist
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public long UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }
}
