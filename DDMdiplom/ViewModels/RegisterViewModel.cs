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
        public string Phone { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
