using Microsoft.AspNetCore.Mvc;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    public class CoursesController : Controller
    {
        ICourseRepository courseRepo;
        public CoursesController(ICourseRepository courseRepository)
        {
            this.courseRepo = courseRepository;
        }
        public IActionResult Index()
        {
            var courses = courseRepo.GetAll();
            var courseVM = courses.Select (c => new CourseViewModel
            {
                CourseID = c.CourseID,
                Name = c.Name,
                Description = c.Description,
                TeacherName = c.Teacher?.Name, // Assuming Teacher is a navigation property
                DepartmentName = c.Department?.Name // Assuming Department is a navigation property
            }).ToList();
            return View(courseVM);
        }
    }
}
