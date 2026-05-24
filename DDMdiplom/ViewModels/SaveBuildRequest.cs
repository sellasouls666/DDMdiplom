namespace DDMdiplom.ViewModels
{
    public class SaveBuildRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<BuildComponent> Components { get; set; } = new();
        public int? BuildId { get; set; } // null – новая, иначе обновление
    }
}
