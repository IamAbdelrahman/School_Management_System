using School_Management_System.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System.ViewModel
{
    public class EnrollmentViewModel
    {
        public int EnrollmentID { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateOnly? EnrollmentDate { get; set; }

        /*ForeignKeys*/
        [ForeignKey("Student")]
        [Display(Name = "Student")]
        public int? StudentID { get; set; }
        public Student? Student { get; set; }
        public List<Student>? Students { get; set; }


        /*ForeignKeys*/
        [ForeignKey("Course")]
        [Display(Name = "Course")]
        public int? CourseID { get; set; }
        public Course? Course { get; set; }
        public List<Course>? Courses { get; set; }

    }
}
