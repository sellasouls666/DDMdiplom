using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Cases")]
    public class Case
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
        public string? Type { get; set; } // Mid-Tower

        [Display(Name = "Цвет")]
        public string? Color { get; set; } // Black

        [Display(Name = "Материал")]
        public string? CaseMaterial { get; set; } // Steel Chassis, Tempered Glass Window, ABS plastic

        [Display(Name = "Совместимость с материнскими платами")]
        public string? MotherboardCompatibility { get; set; } // ATX, micro-ATX, mini-ITX, E-ATX

        [Display(Name = "Боковая панель")]
        public string? SidePanel { get; set; } // Yes, Side Tempered Glass

        // Внутренние отсеки
        [Display(Name = "Внутренние отсеки 3.5\"")]
        public string? Internal35Bays { get; set; } // 2x (1x if 2x SSD installed)

        [Display(Name = "Внутренние отсеки 2.5\"")]
        public string? Internal25Bays { get; set; } // 2x (0x if 2x HDD installed)

        [Display(Name = "Слоты расширения")]
        public int? ExpansionSlots { get; set; } // 7

        [Display(Name = "Поддержка вертикальной установки GPU")]
        public string? VerticalGpuSupport { get; set; } // Also support Vertical GPU, with optional bracket

        // Порты на передней панели
        [Display(Name = "Передние порты")]
        public string? FrontPorts { get; set; } // 1 x USB 3.0, 1 x USB-C 3.2 Gen 2, 1 x Audio, 2 x Buttons

        // Система охлаждения
        [Display(Name = "Возможные места для вентиляторов")]
        public string? FanOptions { get; set; } // Front: 3 x 120mm / 3 x 140mm...

        [Display(Name = "Предустановленные вентиляторы")]
        public string? PreInstalledFans { get; set; } // Front - 3 x 140mm / Rear - 1 x 140mm

        [Display(Name = "Возможные места для радиаторов")]
        public string? RadiatorOptions { get; set; } // Front: Up to 240...

        // Максимальные размеры
        [Display(Name = "Макс. длина видеокарты (мм)")]
        public int? MaxGpuLength { get; set; } // 415 mm

        [Display(Name = "Макс. высота кулера CPU (мм)")]
        public int? MaxCpuCoolerHeight { get; set; } // 184 mm

        [Display(Name = "Макс. длина блока питания (мм)")]
        public int? MaxPsuLength { get; set; } // 270 mm

        [Display(Name = "Габариты (Д x Ш x В, мм)")]
        public string? Dimensions { get; set; } // 450 x 230 x 500 mm

        [Display(Name = "Вес (фунты)")]
        public string? Weight { get; set; } // 18.01lbs

        // Комплектация и прочее
        [Display(Name = "Комплектация")]
        public string? PackageContent { get; set; } // XT Pro Chassis + вентиляторы и т.д.

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
    }
}