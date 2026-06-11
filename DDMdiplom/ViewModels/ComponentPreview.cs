namespace DDMdiplom.ViewModels
{
    public class ComponentPreview
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;          // "Процессор", "Видеокарта" и т.д.
        public string ShortSpecs { get; set; } = string.Empty;    // краткая характеристика
        public decimal Price { get; set; }                        // цена компонента (опционально)
    }
}
