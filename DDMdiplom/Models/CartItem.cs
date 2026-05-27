using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("CartItems")]
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ComponentsJson { get; set; } = "[]";
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; } = 1;
    }
}