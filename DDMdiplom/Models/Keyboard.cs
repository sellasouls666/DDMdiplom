using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Keyboards")]
    public class Keyboard
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        public string? Name { get; set; }          // G413 SE
        public string? Series { get; set; }
        public string? Model { get; set; }          // 920-010433, 920-013610

        [Display(Name = "Тип клавиатуры")]
        public string? Type { get; set; }           // "Gaming Keyboard", "Media Keyboard with Touchpad", "Office"

        // Подключение
        [Display(Name = "Интерфейс")]
        public string? KeyboardInterface { get; set; } // USB 2.0, USB Type-C

        [Display(Name = "Тип подключения")]
        public string? ConnectionType { get; set; }    // "Wired", "Wireless", "Bluetooth"

        // Дизайн
        [Display(Name = "Стиль")]
        public string? DesignStyle { get; set; }       // Standard, Modern

        [Display(Name = "Раскладка")]
        public string? Layout { get; set; }            // Full-size

        [Display(Name = "Цвет")]
        public string? KeyboardColor { get; set; }     // Black

        // Механические характеристики
        [Display(Name = "Механическая")]
        public bool? IsMechanical { get; set; }

        [Display(Name = "Переключатель")]
        public string? KeySwitch { get; set; }         // Logitech Tactile

        [Display(Name = "Тип переключателя")]
        public string? KeySwitchType { get; set; }     // Tactile, Linear, Clicky

        [Display(Name = "Количество функциональных клавиш")]
        public int? FunctionKeysCount { get; set; }    // 1

        // Мышь / тачпад (для гибридных устройств)
        [Display(Name = "Мышь в комплекте")]
        public string? MouseIncluded { get; set; }     // "No", "Yes (Touchpad)"

        [Display(Name = "Интерфейс мыши")]
        public string? MouseInterface { get; set; }    // USB

        [Display(Name = "Метод отслеживания мыши")]
        public string? TrackingMethod { get; set; }    // TouchPad

        [Display(Name = "Количество кнопок мыши")]
        public int? MouseButtons { get; set; }         // 2

        [Display(Name = "Ориентация для правой руки")]
        public string? HandOrientation { get; set; }   // Right

        // Беспроводные особенности
        [Display(Name = "Особенности")]
        public string? Features { get; set; }          // Bluetooth Low Energy, Logi Bolt USB receiver

        [Display(Name = "Источник питания")]
        public string? PowerSource { get; set; }       // Battery Powered

        // Системные требования
        [Display(Name = "Системные требования")]
        public string? SystemRequirement { get; set; } // "1 PS/2 Port; CD-ROM Drive"

        // Габариты и вес
        [Display(Name = "Размеры")]
        public string? Dimensions { get; set; }        // "20'"
        [Display(Name = "Вес")]
        public string? Weight { get; set; }

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
    }
}