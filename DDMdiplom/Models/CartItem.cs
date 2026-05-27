using DDMdiplom.Models;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CartItems")]
public class CartItem
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public int BuildId { get; set; }          // внешний ключ на Builds
    public Build? Build { get; set; }         // навигационное свойство

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public int Quantity { get; set; } = 1;
}