using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class ProductImage
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(Product))]
        public long ProductId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? AltText { get; set; }

        public bool IsPrimary { get; set; } = false;

        public Product Product { get; set; }
    }
}
