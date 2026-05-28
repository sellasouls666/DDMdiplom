using System.ComponentModel.DataAnnotations;

namespace DDMdiplom.ViewModels
{
    // ViewModels/CheckoutViewModel.cs
    public class CheckoutViewModel
    {
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Comment { get; set; }
        public List<CartItemSummaryViewModel> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }

    public class CartItemSummaryViewModel
    {
        public int BuildId { get; set; }          // <-- добавили
        public string BuildName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}