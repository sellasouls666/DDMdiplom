using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Builds")]
    public class Build
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BuildItem> Items { get; set; } = new List<BuildItem>();

        // Вычисляемые свойства (не хранятся в БД) – вы будете заполнять их при загрузке
        [NotMapped]
        public decimal TotalPrice { get; set; }
        [NotMapped]
        public int ComponentCount { get; set; }
    }
}