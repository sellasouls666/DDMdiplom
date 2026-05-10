using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Memory")]
    public class Memory
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        public string? Series { get; set; }

        [Required]
        public string Model { get; set; } = string.Empty;

        // Ёмкость
        [Display(Name = "Объём (ГБ)")]
        public string? Capacity { get; set; } // "32GB (2 x 16GB)"

        // Тип
        [Display(Name = "Тип")]
        public string? Type { get; set; } // "288-Pin PC RAM"

        // Скорость
        [Display(Name = "Скорость")]
        public string? Speed { get; set; } // "DDR5 6400 (PC5 51200)"

        [Display(Name = "CAS Latency")]
        public string? CasLatency { get; set; } // "CL36"

        [Display(Name = "Тайминги")]
        public string? Timing { get; set; } // "36-48-48-104"

        [Display(Name = "Напряжение (В)")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Voltage { get; set; }

        [Display(Name = "Многоканальный комплект")]
        public string? MultiChannelKit { get; set; } // "Dual Channel Kit"

        [Display(Name = "Профили BIOS")]
        public string? BiosProfiles { get; set; } // "AMD EXPO / Intel XMP 3.0"

        [Display(Name = "Цвет")]
        public string? Color { get; set; }

        [Display(Name = "Радиатор")]
        public string? HeatSpreader { get; set; } // "Yes"

        [Display(Name = "Подсветка")]
        public string? LedColor { get; set; } // "RGB"

        // Текстовые описания
        [Display(Name = "Особенности")]
        public string? Features { get; set; }

        [Display(Name = "Рекомендуемое использование")]
        public string? RecommendUse { get; set; }

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
    }
}