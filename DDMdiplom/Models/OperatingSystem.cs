using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("OperatingSystems")]
    public class OperatingSystem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty; // Microsoft

        [Required]
        public string Name { get; set; } = string.Empty; // Windows 11 Home

        public string? Model { get; set; } // KW9-00633

        [Display(Name = "Версия ОС")]
        public string? OperatingSystems { get; set; } // Windows 11

        [Display(Name = "Разрядность")]
        public string? BitVersion { get; set; } // 64-bit

        [Display(Name = "Редакция")]
        public string? Version { get; set; } // Home

        [Display(Name = "Системные требования")]
        public string? SystemRequirements { get; set; }

        [Display(Name = "Тип поставки")]
        public string? Packaging { get; set; } // OEM (Comes with DVD disc)

        [Display(Name = "Дисклеймер")]
        public string? Disclaimer { get; set; }

        // Дополнительные поля
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
    }
}