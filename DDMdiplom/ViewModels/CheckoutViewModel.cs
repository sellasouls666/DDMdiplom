using System.ComponentModel.DataAnnotations;

namespace DDMdiplom.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Укажите полное имя")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        [Phone(ErrorMessage = "Введите корректный номер")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите адрес доставки")]
        public string Address { get; set; }

        public string? Comment { get; set; }

        public List<CartItemSummaryViewModel> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }

    public class CartItemSummaryViewModel
    {
        public int BuildId { get; set; }
        public string BuildName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}