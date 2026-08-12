using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Preference
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public long UserId { get; set; }

        [MaxLength(20)]
        public string Theme { get; set; } = "light";

        public int ItemsPerPage { get; set; } = 12;

        [MaxLength(20)]
        public string SortOrder { get; set; } = "newest";

        [MaxLength(10)]
        public string Currency { get; set; } = "ZAR";

        [MaxLength(10)]
        public string Language { get; set; } = "en";

        public bool Notifications { get; set; } = true;

        public User User { get; set; }
    }
}