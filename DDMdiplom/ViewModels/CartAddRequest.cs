using System.Collections.Generic;

namespace DDMdiplom.ViewModels
{
    public class CartAddRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<BuildComponent> Components { get; set; } = new();
    }
}