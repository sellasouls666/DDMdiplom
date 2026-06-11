using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Monitors")]
    public class Monitor
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        public string? PartNumber { get; set; }

        [Display(Name = "Тип монитора")]
        public string? MonitorType { get; set; } // Gaming, LCD/LED, Office, Professional

        [Display(Name = "Цвет корпуса")]
        public string? CabinetColor { get; set; }

        // Дисплей
        [Display(Name = "Размер экрана (дюймы)")]
        public string? ScreenSize { get; set; } // 27"

        [Display(Name = "Тип матрицы")]
        public string? Panel { get; set; } // OLED

        [Display(Name = "Тип дисплея")]
        public string? DisplayType { get; set; } // WQHD

        [Display(Name = "Адаптивная синхронизация")]
        public string? AdaptiveSyncTechnology { get; set; }

        [Display(Name = "Макс. разрешение")]
        public string? MaximumResolution { get; set; } // 2560 x 1440

        [Display(Name = "Разрешение")]
        public string? Resolution { get; set; } // 2560 x 1440 (2K)

        [Display(Name = "Углы обзора")]
        public string? ViewingAngle { get; set; } // 178° (H) / 178° (V)

        [Display(Name = "Соотношение сторон")]
        public string? AspectRatio { get; set; } // 16:9

        [Display(Name = "Яркость SDR (кд/м²)")]
        public int? BrightnessSdr { get; set; }

        [Display(Name = "Яркость HDR (кд/м²)")]
        public int? BrightnessHdr { get; set; }

        [Display(Name = "Контрастность")]
        public string? ContrastRatio { get; set; } // 1500000:1

        [Display(Name = "Время отклика (мс)")]
        public string? ResponseTime { get; set; } // 0.03 ms

        [Display(Name = "Цветовой охват")]
        public string? ColorGamut { get; set; } // ADOBE RGB 98% ...

        [Display(Name = "Количество цветов")]
        public string? DisplayColors { get; set; } // 1.07B, 10 bits

        [Display(Name = "Плотность пикселей")]
        public string? MonitorPixelDensity { get; set; }

        [Display(Name = "Частота обновления (Гц)")]
        public int? RefreshRate { get; set; } // 240Hz

        [Display(Name = "Стандарт HDR")]
        public string? HdrStandard { get; set; }

        [Display(Name = "Изогнутый экран")]
        public string? CurvedSurfaceScreen { get; set; } // Flat Panel

        // Порты
        [Display(Name = "HDMI")]
        public string? HdmiPorts { get; set; } // 2 x HDMI 2.1

        [Display(Name = "DisplayPort")]
        public string? DisplayPort { get; set; } // 1 x DisplayPort 1.4a

        [Display(Name = "Видео порты (подробно)")]
        public string? VideoPorts { get; set; }

        [Display(Name = "Наушники")]
        public string? Headphone { get; set; } // 1x Headphone out

        // Питание
        [Display(Name = "Блок питания")]
        public string? PowerSupply { get; set; }

        // Эргономика
        [Display(Name = "Регулировки подставки")]
        public string? StandAdjustments { get; set; } // Height, Pivot, Swivel, Tilt

        [Display(Name = "Крепление VESA")]
        public string? VesaCompatibility { get; set; } // 100 x 100mm

        // Габариты и вес
        [Display(Name = "Размеры (Д x Ш x В, дюймы)")]
        public string? Dimensions { get; set; } // 24" X 14" X 2.19"

        [Display(Name = "Вес (фунты)")]
        public string? Weight { get; set; } // 12.5 lbs

        [Display(Name = "Поддержка 3D")]
        public string? ThreeDReady { get; set; } // Yes/No

        [Display(Name = "Встроенные динамики")]
        public string? BuiltInSpeakers { get; set; }

        [Display(Name = "Веб-камера")]
        public string? BuiltInWebcam { get; set; }

        [Display(Name = "Поддержка HDCP")]
        public string? HdcpSupport { get; set; }

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
    }
}