using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("PowerSupplies")]
    public class PowerSupply
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        public string? Series { get; set; }

        [Required]
        public string Model { get; set; } = string.Empty;

        public string? PartNumber { get; set; }

        [Display(Name = "Цвет")]
        public string? Color { get; set; }

        [Display(Name = "Тип")]
        public string? Type { get; set; } // ATX 3.1 Compatible

        [Display(Name = "Максимальная мощность (Вт)")]
        public int? MaximumPower { get; set; } // 850

        [Display(Name = "Вентилятор")]
        public string? Fans { get; set; } // 1 x 120 mm

        [Display(Name = "Основной разъём")]
        public string? MainConnector { get; set; } // 24Pin

        [Display(Name = "Длина блока питания (мм)")]
        public int? MaxPsuLength { get; set; } // 140

        [Display(Name = "Модульный")]
        public string? Modular { get; set; } // Full Modular

        [Display(Name = "Энергоэффективность")]
        public string? EnergyEfficient { get; set; } // Cybenetics Gold

        [Display(Name = "Входное напряжение (В)")]
        public string? InputVoltage { get; set; } // 100 - 240

        [Display(Name = "Частотный диапазон (Гц)")]
        public string? InputFrequencyRange { get; set; } // 47 - 63

        [Display(Name = "Входной ток")]
        public string? InputCurrent { get; set; } // +3.3V@20A, +5V@20A...

        [Display(Name = "Разъёмы")]
        public string? Connectors { get; set; } // 1 x 12VHPWR (12+4) PCIe ...

        // Дополнительные поля
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
    }
}