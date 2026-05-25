public class BuildComponent
{
    public int Id { get; set; }                 // ID из базы
    public Guid InstanceId { get; set; } = Guid.NewGuid(); // уникальный для каждого экземпляра
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int? Tdp { get; set; }
    public int ModuleCount { get; set; } = 1;
    public int? MemorySlots { get; set; }
    public int? SataPorts { get; set; }
    public string StorageInterface { get; set; }   // "SATA" или "M.2"
    public int? M2Slots { get; set; }
    // поля для совместимости
    public string? CpuSocketType { get; set; }          // сокет материнской платы
    public string? Speed { get; set; }                 // скорость/тип памяти (например "DDR5 6400")
    public string? CpuSocketCompatibility { get; set; } // для воздушного кулера
    public string? BlockCompatibility { get; set; }    // для водяного охлаждения
    public int? MaxGpuLength { get; set; }  // максимальная длина видеокарты, поддерживаемая корпусом
    public string? ProcessorSocket { get; set; }       // сокет выбранного процессора
    public string? ProcessorMemoryTypes { get; set; }  // поддерживаемые типы памяти процессора
    public string? CaseMotherboardFormFactors { get; set; } // совместимые форм‑факторы корпуса
    public string? MbMemoryStandard { get; set; } // стандарт памяти материнской платы
    public int? MaxPsuLength { get; set; }  // максимальная длина БП, поддерживаемая корпусом (из Case)
    public int? GpuLength { get; set; }          // длина выбранной видеокарты (из GraphicsCard.MaxGpuLength)
    public int? PsuLength { get; set; }         // длина выбранного БП (из PowerSupply.MaxPsuLength)
    public string? MbFormFactor { get; set; }   // форм-фактор выбранной материнской платы (из Motherboard.FormFactor)
}