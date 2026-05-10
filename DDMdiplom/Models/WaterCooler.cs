using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("WaterCoolers")]
    public class WaterCooler
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Display(Name = "Тип")]
        public string? Type { get; set; } // AIO

        // Блок (водоблок)
        [Display(Name = "Совместимость с сокетами (общая)")]
        public string? BlockCompatibility { get; set; }

        [Display(Name = "Совместимость с AMD")]
        public string? BlockCompatibilityAmd { get; set; }

        [Display(Name = "Совместимость с Intel")]
        public string? BlockCompatibilityIntel { get; set; }

        [Display(Name = "Размеры блока (мм)")]
        public string? BlockDimensions { get; set; } // 70.9 x 69.3 x 56.98 mm

        // Помпа
        [Display(Name = "Скорость помпы (RPM)")]
        public string? PumpSpeed { get; set; } // 3800 RPM ± 300 RPM

        [Display(Name = "Уровень шума помпы (дБА)")]
        public string? PumpNoise { get; set; } // 20 dBA (avg.)

        // Радиатор
        [Display(Name = "Размер радиатора (мм)")]
        public string? RadiatorSize { get; set; } // 360 mm

        [Display(Name = "Материал радиатора")]
        public string? RadiatorMaterial { get; set; } // Aluminum

        // Вентиляторы
        [Display(Name = "Количество вентиляторов")]
        public int? FanCount { get; set; } // 3

        [Display(Name = "Размер вентиляторов (мм)")]
        public string? FanSize { get; set; } // 120 mm

        [Display(Name = "Габариты вентилятора (мм)")]
        public string? FanDimensions { get; set; } // 120 x 120 x 25 mm

        [Display(Name = "Тип подшипника")]
        public string? BearingType { get; set; } // Rifle Bearing

        [Display(Name = "Скорость вентиляторов (RPM)")]
        public string? FanRpm { get; set; } // 500~2000 ± 10% RPM

        [Display(Name = "Воздушный поток (CFM)")]
        public string? FanAirFlow { get; set; } // 62.6 CFM

        [Display(Name = "Уровень шума вентиляторов (дБА)")]
        public string? FanNoise { get; set; } // 31.1 dBA

        [Display(Name = "Статическое давление (mmH2O)")]
        public string? Pressure { get; set; } // 2.36 mmH2O

        // Цвет и подсветка
        public string? Color { get; set; } // Black
        [Display(Name = "Цвет подсветки")]
        public string? LedColor { get; set; } // ARGB

        // Трубки
        [Display(Name = "Длина трубок (мм)")]
        public string? TubeDimensions { get; set; } // 390 mm / 15.35 inches
        [Display(Name = "Материал трубок")]
        public string? TubeMaterial { get; set; } // EPDM

        // Дополнительные поля
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
    }
}