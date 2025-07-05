using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
