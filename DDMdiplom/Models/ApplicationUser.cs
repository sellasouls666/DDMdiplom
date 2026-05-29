using Microsoft.AspNetCore.Identity;

namespace DDMdiplom.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
    }
}