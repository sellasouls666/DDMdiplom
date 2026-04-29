using Microsoft.AspNetCore.Identity;

namespace DDMdiplom.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!; //без инициализации свойство Name будет иметь значение null в момент создания объекта
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // дата регистрации
    }
}
