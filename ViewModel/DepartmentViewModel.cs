using School_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{

    public class DepartmentViewModel
    {
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Department name must be at least 5 characters")]
        [RegularExpression(@"^[A-Za-z\- ]+$", ErrorMessage = "Department name can contain only letters, hyphens (-), and spaces")]
        public string Name { get; set; }

        public string Courses { get; set; }
        public string? Teachers { get; set; }

        public List<int> SelectedCourseIDs { get; set; } 
        public List<int> SelectedTeacherIDs { get; set; } 

        public IEnumerable<Course> AllCourses { get; set; }
        public IEnumerable<Teacher> AllTeachers { get; set; }
    }
}
