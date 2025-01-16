using Microsoft.AspNetCore.Identity;

namespace MotorX.DataService.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
