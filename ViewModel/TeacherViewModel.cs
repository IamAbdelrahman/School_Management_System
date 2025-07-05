using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
namespace School_Management_System.ViewModel
{
    public class TeacherViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Phone number is not valid")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hire date is required")]
        [Display(Name = "Hire Date")]
        public DateOnly? HireDate { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        // For dropdown
        public List<SelectListItem>? Departments { get; set; }

        // Optional: for class/course count preview in Index or Delete
        public int ClassCount { get; set; }
        public int CourseCount { get; set; }

        public List<string> ClassNames { get; set; } = new();
        public List<string> CourseNames { get; set; } = new();
    }
}
