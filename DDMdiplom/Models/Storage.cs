using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("Storages")]
    public class Storage
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        public string? Series { get; set; }

        [Required]
        public string Model { get; set; } = string.Empty;

        [Display(Name = "Тип устройства")]
        public string? DeviceType { get; set; } // HDD, SSD, NVMe и т.д.

        [Display(Name = "Форм-фактор")]
        public string? FormFactor { get; set; } // 3.5", M.2 2280

        [Display(Name = "Ёмкость")]
        public string? Capacity { get; set; } // 8TB, 2TB

        [Display(Name = "Интерфейс")]
        public string? Interface { get; set; } // SATA 6.0Gb/s, PCI-Express 4.0 x4

        // Специфические для HDD
        [Display(Name = "Скорость вращения (RPM)")]
        public int? Rpm { get; set; }

        [Display(Name = "Кэш (MB)")]
        public int? Cache { get; set; }

        [Display(Name = "Технология записи")]
        public string? RecordingTechnology { get; set; } // SMR, CMR

        // Специфические для SSD
        [Display(Name = "Протокол")]
        public string? Protocol { get; set; } // NVMe 2.0, AHCI

        [Display(Name = "Макс. последовательное чтение (MB/s)")]
        public int? MaxSequentialRead { get; set; }

        [Display(Name = "Макс. последовательная запись (MB/s)")]
        public int? MaxSequentialWrite { get; set; }

        [Display(Name = "Радиатор")]
        public string? HeatSink { get; set; } // without HeatSink, with HeatSink

        // Физические параметры
        [Display(Name = "Размеры (Д x Ш x В, мм)")]
        public string? Dimensions { get; set; }

        [Display(Name = "Вес (кг)")]
        [Column(TypeName = "decimal(10,4)")]
        public decimal? Weight { get; set; }

        // Описания
        [Display(Name = "Особенности")]
        public string? Features { get; set; }

        [Display(Name = "Рекомендуемое использование")]
        public string? Usage { get; set; }

        // Дополнительные поля для каталога
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
    }
}