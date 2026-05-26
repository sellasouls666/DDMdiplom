using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>JSON-массив компонентов (List<BuildComponent>)</summary>
        [Required]
        public string ComponentsJson { get; set; } = "[]";

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}