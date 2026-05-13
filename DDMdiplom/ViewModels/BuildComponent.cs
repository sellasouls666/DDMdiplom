public class BuildComponent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int? Tdp { get; set; } // для процессора, видеокарты и т.д.
}