using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Processors")]
    public class Processor
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        [Display(Name = "Тип процессора")]
        public string? ProcessorsType { get; set; }

        public string? Series { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Model { get; set; }

        // Технические детали
        [Display(Name = "Сокет")]
        public string? CpuSocketType { get; set; }

        [Display(Name = "Кодовое имя")]
        public string? CoreName { get; set; }

        [Display(Name = "Количество ядер")]
        public int? Cores { get; set; }

        [Display(Name = "Количество потоков")]
        public int? Threads { get; set; }

        [Display(Name = "Базовая частота (GHz)")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal? OperatingFrequency { get; set; }

        [Display(Name = "Макс. турбо частота (GHz)")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal? MaxTurboFrequency { get; set; }

        [Display(Name = "L1 кэш")]
        public string? L1Cache { get; set; }

        [Display(Name = "L2 кэш")]
        public string? L2Cache { get; set; }

        [Display(Name = "L3 кэш")]
        public string? L3Cache { get; set; }

        [Display(Name = "Техпроцесс")]
        public string? ManufacturingTech { get; set; }

        [Display(Name = "Набор инструкций")]
        public string? InstructionSet { get; set; }

        // Память
        [Display(Name = "Поддерживаемые типы памяти")]
        public string? MemoryTypes { get; set; } // можно хранить как строку "DDR5 5600, DDR5 6000"

        [Display(Name = "Каналов памяти")]
        public int? MemoryChannel { get; set; }

        [Display(Name = "Максимальный объем памяти")]
        public string? MaxMemorySize { get; set; }

        // Графика
        [Display(Name = "Встроенная графика")]
        public string? IntegratedGraphics { get; set; }

        [Display(Name = "Частота графики (MHz)")]
        public int? GraphicsBaseFrequency { get; set; }

        // PCI Express
        [Display(Name = "Версия PCIe")]
        public string? PciExpressRevision { get; set; }

        [Display(Name = "Количество линий PCIe")]
        public string? MaxPciExpressLanes { get; set; }

        // Энергопотребление и охлаждение
        [Display(Name = "TDP (Вт)")]
        public int? ThermalDesignPower { get; set; }

        [Display(Name = "Охлаждение в комплекте")]
        public string? CoolingDevice { get; set; }

        // Совместимость
        [Display(Name = "Совместимые чипсеты")]
        public string? CompatibleChipsets { get; set; }

        [Display(Name = "Поддерживаемые ОС")]
        public string? SupportedOperatingSystems { get; set; }

        [Display(Name = "Технологии")]
        public string? AdvancedTechnologies { get; set; }

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
    }
}