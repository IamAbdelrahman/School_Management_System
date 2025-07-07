using School_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{

    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Department Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Department name must be between 3 and 30 characters")]
        public string Name { get; set; }
        [Display(Name = "Teacher")]
        public int? TeacherId { get; set; }
        [Display(Name = "Course")]
        public int? CourseId { get; set; }
        public int TeacherCount { get; set; }
        public int CourseCount { get; set; }
    }
}
