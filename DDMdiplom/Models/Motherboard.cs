using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Motherboards")]
    public class Motherboard
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        // Поддержка CPU
        [Display(Name = "Сокет CPU")]
        public string? CpuSocketType { get; set; }

        [Display(Name = "Поддерживаемые типы CPU")]
        public string? CpuType { get; set; }

        // Чипсет
        public string? Chipset { get; set; }

        // Память
        [Display(Name = "Количество слотов памяти")]
        public int? MemorySlots { get; set; }

        [Display(Name = "Тип памяти")]
        public string? MemoryStandard { get; set; }

        [Display(Name = "Максимальный объем памяти (GB)")]
        public int? MaxMemorySize { get; set; }

        [Display(Name = "Поддержка ECC")]
        public string? EccSupported { get; set; }

        // Слоты расширения
        [Display(Name = "PCI Express 5.0 x16")]
        public string? PciExpress50x16 { get; set; }

        [Display(Name = "PCI Express x4")]
        public string? PciExpressX4 { get; set; }

        [Display(Name = "PCI Express x1")]
        public string? PciExpressX1 { get; set; }

        // Хранение данных
        [Display(Name = "SATA порты")]
        public string? SataPorts { get; set; }

        [Display(Name = "M.2 слоты")]
        public string? M2Slots { get; set; }  // можно хранить как JSON или строку с описанием

        // Аудио
        [Display(Name = "Аудио чипсет")]
        public string? AudioChipset { get; set; }

        [Display(Name = "Аудио каналы")]
        public string? AudioChannels { get; set; }

        // Сеть
        [Display(Name = "LAN чипсет")]
        public string? LanChipset { get; set; }

        [Display(Name = "Скорость LAN (Mbps)")]
        public string? MaxLanSpeed { get; set; }

        [Display(Name = "Wi-Fi")]
        public string? WirelessLan { get; set; }

        [Display(Name = "Bluetooth")]
        public string? Bluetooth { get; set; }

        // Порты и разъёмы (можно сгруппировать или хранить как текст)
        [Display(Name = "Задние порты ввода-вывода")]
        public string? RearPanelPorts { get; set; }

        [Display(Name = "Внутренние разъёмы")]
        public string? InternalIoConnectors { get; set; }

        // Физические характеристики
        [Display(Name = "Форм-фактор")]
        public string? FormFactor { get; set; }

        [Display(Name = "Подсветка")]
        public string? LedLighting { get; set; }

        [Display(Name = "Размеры (Д x Ш, дюймы)")]
        public string? Dimensions { get; set; }

        // Питание
        [Display(Name = "Разъёмы питания")]
        public string? PowerConnectors { get; set; }

        // Особенности и ПО
        [Display(Name = "Особенности BIOS")]
        public string? BiosFeature { get; set; }

        [Display(Name = "Программное обеспечение")]
        public string? Software { get; set; }

        [Display(Name = "Ключевые особенности")]
        public string? Features { get; set; }

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
    }
}