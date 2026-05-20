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
}