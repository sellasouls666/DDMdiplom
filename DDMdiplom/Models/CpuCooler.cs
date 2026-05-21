using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("CpuCoolers")]
    public class CpuCooler
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        public string? Series { get; set; }

        [Required]
        public string Model { get; set; } = string.Empty;

        [Display(Name = "Тип")]
        public string? Type { get; set; } // Fan & Heatsinks

        [Display(Name = "Размер вентилятора")]
        public string? FanSize { get; set; } // 80mm

        [Display(Name = "Совместимость с сокетами CPU")]
        public string? CpuSocketCompatibility { get; set; } // AMD Socket AM4 / AM3 / AM2 / FM2 / FM1

        [Display(Name = "Тип подшипника")]
        public string? BearingType { get; set; } // Dual ball bearing

        [Display(Name = "Скорость вращения (RPM)")]
        public string? Rpm { get; set; } // 800 - 3,000 RPM PWM

        [Display(Name = "Воздушный поток (CFM)")]
        public string? AirFlow { get; set; } // 34.33 CFM (Max.)

        [Display(Name = "Уровень шума (дБА)")]
        public string? NoiseLevel { get; set; } // 33 dBA (Max.)

        [Display(Name = "Материал радиатора")]
        public string? HeatsinkMaterial { get; set; } // Copper heat pipes with aluminum fins

        [Display(Name = "Ориентация вентилятора")]
        public string? FanMountingTypesToHeatsink { get; set; } // Horizontal

        // Физические параметры
        [Display(Name = "Макс. высота кулера (мм)")]
        public int? MaxCpuCoolerHeight { get; set; } // 54 mm

        [Display(Name = "Габариты вентилятора (Д x Ш x В, мм)")]
        public string? FanDimensions { get; set; } // 80.00 x 80.00 x 15.00 mm

        [Display(Name = "Габариты радиатора (Д x Ш x В, мм)")]
        public string? HeatsinkDimensions { get; set; } // 54.00 x 107.00 x 82.00 mm

        // Особенности и примечания
        [Display(Name = "Особенности")]
        public string? Features { get; set; }

        [Display(Name = "Примечание")]
        public string? Remark { get; set; }

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }

        public string? Color { get; set; }
    }
}