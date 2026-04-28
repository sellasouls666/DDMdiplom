using System.ComponentModel.DataAnnotations;

namespace DDMdiplom.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно.")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите действительный адрес электронной почты.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно.")]
        public string Password { get; set; }
    }
}
