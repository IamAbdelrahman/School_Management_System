using Microsoft.AspNetCore.Mvc;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository departmentRepo;
        private readonly ICourseRepository courseRepo;

        public DepartmentsController(IDepartmentRepository departmentRepo, ICourseRepository courseRepo)
        {
            this.departmentRepo = departmentRepo;
            this.courseRepo = courseRepo;
        }

        public IActionResult Index()
        {
            var courses = courseRepo.GetAll();
            var departments = departmentRepo.GetAll();
            var departmentVM = departments.Select(d => new DepartmentViewModel
            {
                DepartmentID = d.DepartmentID,
                Name = d.Name,
                //CoursesNames = string.Join(", ", d.Courses.Select(c => c.Name)), // ["Math", "Science"] >> "Math, Science"
                CoursesNames = string.Join(", ", d.Courses.Select(c => c.Name)),
                TeacherName = d.Teachers.FirstOrDefault()?.Name, // Assuming each department has at least one teacher
                StudensCount = 0

            }).ToList();

            return View(departmentVM);
        }
    }
}
