using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("UpsDevices")]
    public class Ups
    {
        [Key]
        public int Id { get; set; }

        // Основные характеристики
        [Required]
        public string Brand { get; set; } = string.Empty;

        public string? Series { get; set; }

        [Required]
        public string Model { get; set; } = string.Empty;

        // Входные параметры
        [Display(Name = "Версия (напряжение)")]
        public string? InternationalVersion { get; set; } // 120V

        [Display(Name = "Диапазон входного напряжения (Vac)")]
        public string? InputVoltageRange { get; set; } // 88-144 Vac

        [Display(Name = "Входная частота (Гц)")]
        public string? InputFrequency { get; set; } // 57 - 63 Hz

        [Display(Name = "Входной разъём")]
        public string? InputConnection { get; set; } // NEMA 5-15P

        // Выходные параметры
        [Display(Name = "Мощность (VA)")]
        public int? VaRating { get; set; } // 1500 VA

        [Display(Name = "Мощность (Вт)")]
        public int? Watts { get; set; } // 1000 Watts

        [Display(Name = "Выходное напряжение (Vac)")]
        public string? OutputVoltage { get; set; } // 120Vac +/- 5%

        [Display(Name = "Выходная частота (Гц)")]
        public string? OutputFrequency { get; set; } // 60 Hz +/- 1%

        [Display(Name = "Количество розеток")]
        public int? Outlets { get; set; } // 12

        [Display(Name = "Тип розеток")]
        public string? OutletType { get; set; } // NEMA 5-15R

        // Батарея
        [Display(Name = "Тип батареи")]
        public string? BatteryType { get; set; } // Sealed Lead Acid

        [Display(Name = "Время работы при половинной нагрузке")]
        public string? BatteryRunTimeHalfLoad { get; set; } // 10 Minutes

        [Display(Name = "Время работы при полной нагрузке")]
        public string? BatteryRunTimeFullLoad { get; set; } // 2.5 Minutes

        [Display(Name = "Время зарядки (часов)")]
        public int? BatteryRechargeTime { get; set; } // 8 Hour(s)

        [Display(Name = "Заменяемая батарея (модели)")]
        public string? BatteryReplaceable { get; set; } // RB1290X2 ...

        // Сигналы и интерфейсы
        [Display(Name = "Типы сигналов")]
        public string? Alarms { get; set; } // Battery Mode, Low Battery...

        [Display(Name = "Интерфейсные порты")]
        public string? InterfacePort { get; set; } // USB, Serial

        [Display(Name = "ПО для управления")]
        public string? ManagementSoftware { get; set; } // PowerPanel Business Edition

        [Display(Name = "Сертификаты")]
        public string? Approvals { get; set; }

        // Защита от перенапряжения
        [Display(Name = "Защита линии данных")]
        public string? DataLineProtection { get; set; } // Network Protection RJ45...

        [Display(Name = "Энергоёмкость защиты (Джоули)")]
        public int? SurgeEnergyRating { get; set; } // 1445 Joules

        [Display(Name = "Тип защиты")]
        public string? Protection { get; set; } // Internal circuitry limiting / Circuit breaker

        // Особенности
        [Display(Name = "Особенности")]
        public string? Features { get; set; }

        // Габариты и вес
        [Display(Name = "Размеры (Д x Ш x В, дюймы)")]
        public string? Dimensions { get; set; } // 11.00" x 3.90" x 14.00"

        [Display(Name = "Вес (фунты)")]
        public string? Weight { get; set; } // 24.90 lbs.

        // Дополнительные поля
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
    }
}