using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{

    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Course Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 100 characters")]
        public string? Title { get; set; }


        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Macimum Length is 100 characters")]
        public string? Description { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Teacher")]
        public int? TeacherId { get; set; }

        // Navigation properties for display
        public string? DepartmentName { get; set; }
        public string? TeacherName { get; set; }
        public List<StudentViewModel>? EnrolledStudents { get; set; }
    }
}