
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public string Status { get; set; } = "Новый"; // Например: Новый, В обработке, Отправлен, Доставлен, Отменен

        // Навигационное свойство для элементов заказа
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Comment { get; set; }
    }
}
