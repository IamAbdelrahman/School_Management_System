using System.ComponentModel.DataAnnotations;
using System;
namespace School_Management_System.ViewModel
{
    public class TeacherViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Hire Date")]
        public DateOnly? HireDate { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
    }
}
