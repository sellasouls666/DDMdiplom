using System.ComponentModel.DataAnnotations;

namespace DDMdiplom.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [Display(Name = "Полное имя")]
        public string FullName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Некорректный номер телефона")]
        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        [Display(Name = "Адрес доставки")]
        public string? Address { get; set; }
    }
}