using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("GraphicsCards")]
    public class GraphicsCard
    {
        [Key]
        public int Id { get; set; }

        // Бренд и модель
        [Required]
        public string Brand { get; set; } = string.Empty;

        public string? Series { get; set; }

        [Required]
        public string Model { get; set; } = string.Empty;

        public string? PartNumber { get; set; }

        // Интерфейс
        [Display(Name = "Интерфейс")]
        public string? Interface { get; set; } // PCI Express 5.0

        // Чипсет
        [Display(Name = "Производитель чипсета")]
        public string? ChipsetManufacturer { get; set; } // NVIDIA

        [Display(Name = "Серия GPU")]
        public string? GpuSeries { get; set; } // NVIDIA GeForce RTX 50 Series

        [Display(Name = "GPU")]
        public string? Gpu { get; set; } // GeForce RTX 5070

        [Display(Name = "Boost Clock (MHz)")]
        public int? BoostClock { get; set; } // 2542 MHz

        [Display(Name = "CUDA ядра")]
        public int? CudaCores { get; set; } // 6144 Cores

        // Память
        [Display(Name = "Эффективная частота памяти (Gbps)")]
        public int? EffectiveMemoryClock { get; set; } // 28 Gbps

        [Display(Name = "Объём памяти (GB)")]
        public int? MemorySize { get; set; } // 12GB

        [Display(Name = "Шина памяти (bit)")]
        public int? MemoryInterface { get; set; } // 192-Bit

        [Display(Name = "Тип памяти")]
        public string? MemoryType { get; set; } // GDDR7

        // 3D API
        [Display(Name = "DirectX")]
        public string? DirectX { get; set; } // DirectX 12 Ultimate

        [Display(Name = "OpenGL")]
        public string? OpenGL { get; set; } // OpenGL 4.6

        // Порты
        [Display(Name = "Поддержка нескольких мониторов")]
        public int? MultiMonitorSupport { get; set; } // 4

        [Display(Name = "HDMI")]
        public int? HdmiPorts { get; set; } // 1

        [Display(Name = "DisplayPort")]
        public int? DisplayPorts { get; set; } // 3

        // Детали
        [Display(Name = "Цвет")]
        public string? Color { get; set; } // Black

        [Display(Name = "Макс. разрешение")]
        public string? MaxResolution { get; set; } // 7680 x 4320

        [Display(Name = "Поддержка SLI")]
        public string? SliSupport { get; set; } // No

        [Display(Name = "Готова к VR")]
        public string? VirtualRealityReady { get; set; } // Yes

        [Display(Name = "Охлаждение")]
        public string? Cooler { get; set; } // Double Fans

        [Display(Name = "TDP (Вт)")]
        public int? ThermalDesignPower { get; set; } // 250W

        [Display(Name = "Рекомендуемая мощность БП (Вт)")]
        public int? RecommendedPsuWattage { get; set; } // 650W

        [Display(Name = "Разъём питания")]
        public string? PowerConnector { get; set; } // 1 x 16-Pin

        [Display(Name = "Поддержка HDCP")]
        public string? HdcpReady { get; set; } // Yes

        // Форм-фактор и размеры
        [Display(Name = "Форм-фактор")]
        public string? FormFactor { get; set; } // ATX

        [Display(Name = "Длина карты (мм)")]
        public int? MaxGpuLength { get; set; } // 231 mm

        [Display(Name = "Размеры (Д x В x Ш, мм)")]
        public string? CardDimensions { get; set; } // 231 x 126 x 50 mm

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
    }
}