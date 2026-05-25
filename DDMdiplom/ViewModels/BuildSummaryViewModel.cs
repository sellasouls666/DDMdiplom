namespace DDMdiplom.ViewModels
{
    public class BuildSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ComponentCount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ComponentPreview> Previews { get; set; } = new();
        public DateTime? UpdatedAt { get; set; }
    }
}
