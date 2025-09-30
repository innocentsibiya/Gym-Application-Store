using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Cart
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public long UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
