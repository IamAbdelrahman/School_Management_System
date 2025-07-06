using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{
    //public class CourseViewModel
    //{
    //    public int CourseID { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //    public string? TeacherName { get; set; }
    //    public string? DepartmentName { get; set; }

    //}

    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        // Navigation properties for display
        public string DepartmentName { get; set; }
        public string TeacherName { get; set; }
        public List<StudentViewModel> EnrolledStudents { get; set; }
    }
}

