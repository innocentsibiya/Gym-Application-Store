using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Review
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public long UserId { get; set; }

        [Required, ForeignKey(nameof(Product))]
        public long ProductId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; } = false;

        public User User { get; set; }
        public Product Product { get; set; }
    }
}
