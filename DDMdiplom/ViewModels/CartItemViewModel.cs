using System.Collections.Generic;

namespace DDMdiplom.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ComponentCount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ComponentPreview> Previews { get; set; } = new();
        public DateTime AddedAt { get; set; }
    }
}