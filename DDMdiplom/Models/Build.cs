using DDMdiplom.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DDMdiplom.Models
{
    public class Build
    {
        [Key]
        public int Id { get; set; }

        // Теперь UserId может быть null — системные сборки
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BuildItem> Items { get; set; } = new List<BuildItem>();

        // Новые поля
        public bool IsSystem { get; set; } = false;          // по умолчанию 0 (не системная)
        public string? BuildType { get; set; }               // тип сборки (например, "Gaming", "Office", "Workstation")

        [NotMapped]
        public decimal TotalPrice { get; set; }
        [NotMapped]
        public int ComponentCount { get; set; }
    }
}