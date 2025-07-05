using School_Management_System.Models;

namespace School_Management_System.ViewModel
{

    public class DepartmentViewModel
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }

        public string Courses { get; set; }
        public string? Teachers { get; set; }

        public List<int> SelectedCourseIDs { get; set; } = new();
        public List<int> SelectedTeacherIDs { get; set; } = new();

        public IEnumerable<Course> AllCourses { get; set; }
        public IEnumerable<Teacher> AllTeachers { get; set; }
    }
}
