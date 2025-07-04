using Microsoft.AspNetCore.Identity;

namespace School_Management_System.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
