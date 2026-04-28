using System.ComponentModel.DataAnnotations;

namespace DDMdiplom.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно.")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите действительный адрес электронной почты.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Пожалуйста, введите действительный номер телефона.")]
        [StringLength (11)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Слабый пароль. Пожалуйста, создайте надежный пароль, соответствующий приведенным ниже требованиям.")]
        public string Password { get; set; }
    }
}
