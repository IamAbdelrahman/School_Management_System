using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;

namespace School_Management_System.ViewModel
{
    public class CourseFilterViewModel
    {
        public int? DepartmentID { get; set; }
        public int? ClassID { get; set; }
        public int? TeacherID { get; set; }

        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Classes { get; set; }
        public List<SelectListItem> Teachers { get; set; }

        public List<Course> FilteredCourses { get; set; } = new List<Course>();
    }

}
