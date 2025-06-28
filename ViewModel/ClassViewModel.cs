using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{
    public class ClassViewModel
    {
        public int ClassID { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        [StringLength(30)]
        public string? Name { get; set; }

        [Range(1, 12, ErrorMessage = "Grade level must be between 1 and 12")]
        public int? GradeLevel { get; set; }

        [Display(Name = "Assigned Teacher")]
        public int? TeacherID { get; set; }

        public List<SelectListItem>? Teachers { get; set; }

        public List<Student>? Students { get; set; }

        public string? TeacherRole { get; set; }
        public string? TeacherName { get; set; }
    }
}
