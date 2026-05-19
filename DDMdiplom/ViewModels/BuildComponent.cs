public class BuildComponent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int? Tdp { get; set; }
    public int ModuleCount { get; set; } = 1;
    public int? SataPorts { get; set; }      // для материнской платы
    public int? MemorySlots { get; set; }    // для материнской платы
}