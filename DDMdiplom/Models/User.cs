using System.ComponentModel.DataAnnotations;

namespace DDMdiplom.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
