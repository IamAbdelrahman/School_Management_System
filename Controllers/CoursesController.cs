using Microsoft.AspNetCore.Mvc;
using School_Management_System.Models;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    public class CoursesController : Controller
    {
        ICourseRepository courseRepo;
        ITeacherRepository teacherRepo;
        IDepartmentRepository departmentRepo;
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
                TeacherName = c.Teacher?.Name, 
                DepartmentName = c.Department?.Name 
            }).ToList();
            return View(courseVM);
        }
        public IActionResult Details(int id)
        {
            var course = courseRepo.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseVM = new CourseViewModel
            {
                CourseID = course.CourseID,
                Name = course.Name,
                Description = course.Description,
                TeacherName = course.Teacher?.Name, 
                DepartmentName = course.Department?.Name 
            };
            return View(courseVM);
        }
        public IActionResult Reseed()
        {
            try
            {
                courseRepo.ReseedTable("Courses", 11);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }
        public IActionResult Create()
        {
            ViewBag.Teachers = teacherRepo.GetAll().Select(t => new { t.TeacherID, t.Name }).ToList();
            ViewBag.Departments = departmentRepo.GetAll().Select(d => new { d.DepartmentID, d.Name }).ToList();
            return View();
        }
        public IActionResult Edit(int id)
        {
            var course = courseRepo.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseVM = new CourseViewModel
            {
                CourseID = course.CourseID,
                Name = course.Name,
                Description = course.Description,
                TeacherName = course.Teacher?.Name, // Assuming Teacher is a navigation property
                DepartmentName = course.Department?.Name // Assuming Department is a navigation property
            };
            return View(courseVM);
        }
        //public IActionResult SaveChanges(CourseViewModel courseVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var course = new Course
        //        {
        //            CourseID = courseVM.CourseID,
        //            Name = courseVM.Name,
        //            Description = courseVM.Description,
        //            TeacherID = courseVM.TeacherName != null ? courseRepo.GetTeacherIdByName(courseVM.TeacherName) : null, // Assuming a method to get TeacherID by name
        //            DepartmentID = courseVM.DepartmentName != null ? courseRepo.GetDepartmentIdByName(courseVM.DepartmentName) : null // Assuming a method to get DepartmentID by name
        //        };
        //        if (course.CourseID > 0)
        //        {
        //            courseRepo.Update(course);
        //        }
        //        else
        //        {
        //            courseRepo.Add(course);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View("Create", courseVM);
        //}
    }
}
