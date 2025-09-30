using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Supplier
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ContactName { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}
