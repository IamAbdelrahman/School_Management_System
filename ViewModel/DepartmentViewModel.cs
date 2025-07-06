using School_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{

    public class DepartmentViewModel
    {
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int TeacherCount { get; set; }
        public int CourseCount { get; set; }
    }
}
