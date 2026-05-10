using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Mice")]
    public class Mouse
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Model { get; set; }

        [Display(Name = "Тип мыши")]
        public string? Type { get; set; } // Gaming, Office, Wireless

        // Подключение
        [Display(Name = "Тип подключения")]
        public string? ConnectionType { get; set; } // Wired, Wireless, Bluetooth, Dual

        [Display(Name = "Интерфейс")]
        public string? Interface { get; set; } // USB, USB-C, USB & Bluetooth

        // Эргономика
        [Display(Name = "Стиль хвата")]
        public string? GripStyle { get; set; } // Palm / Claw, Fingertip

        [Display(Name = "Метод отслеживания")]
        public string? TrackingMethod { get; set; } // Optical, Laser

        [Display(Name = "Максимальное разрешение (DPI)")]
        public int? MaxDpi { get; set; } // 25600 dpi

        [Display(Name = "Ориентация для руки")]
        public string? HandOrientation { get; set; } // Right Hand, Left Hand, Ambidextrous

        [Display(Name = "Количество кнопок")]
        public int? ButtonsCount { get; set; }

        [Display(Name = "Тип прокрутки")]
        public string? ScrollingCapability { get; set; } // Tilt Wheel, Free-spin, Wheel

        [Display(Name = "Регулируемый вес")]
        public string? AdjustableWeight { get; set; } // 5 x 3.6g

        [Display(Name = "Цвет")]
        public string? Color { get; set; }

        // Подсветка
        [Display(Name = "RGB подсветка")]
        public string? Lighting { get; set; } // Lightsync RGB

        // Беспроводные особенности
        [Display(Name = "Источник питания")]
        public string? PowerSource { get; set; } // 1 x AA Battery, Built-in rechargeable

        [Display(Name = "Время работы от батареи")]
        public string? BatteryLife { get; set; } // Up to 24 Months

        [Display(Name = "Дальность беспроводной связи")]
        public string? WirelessRange { get; set; } // Up to 10.1m

        [Display(Name = "Версия")]
        public string? Version { get; set; } // Graphite Lift

        // Системные требования
        [Display(Name = "Поддерживаемые ОС")]
        public string? SupportedOperatingSystems { get; set; }

        [Display(Name = "Системные требования")]
        public string? SystemRequirement { get; set; }

        // Особенности
        [Display(Name = "Особенности")]
        public string? Features { get; set; }

        // Комплектация
        [Display(Name = "Комплектация")]
        public string? PackageContents { get; set; }

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
    }
}