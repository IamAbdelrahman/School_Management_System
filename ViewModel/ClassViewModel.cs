using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{
    public class ClassViewModel
    {
        public int ClassID { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Class name must be at least 5 characters")]
        [RegularExpression(@"^[A-Za-z\- ]+$", ErrorMessage = "Class name can contain only letters, hyphens (-), and spaces")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Grade level is required")]
        [Range(1, 12, ErrorMessage = "Grade level must be between 1 and 12")]
        public int? GradeLevel { get; set; }

        [Required(ErrorMessage = "Assigned teacher is required")]
        [Display(Name = "Assigned Teacher")]
        public int? TeacherID { get; set; }

        public List<SelectListItem>? Teachers { get; set; }

        public List<Student>? Students { get; set; }

        public string? TeacherRole { get; set; }
        public string? TeacherName { get; set; }
    }
}
