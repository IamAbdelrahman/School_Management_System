using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository departmentRepo;
        private readonly ICourseRepository courseRepo;
        private readonly ITeacherRepository teacherRepo;

        public DepartmentsController(IDepartmentRepository departmentRepo, ICourseRepository courseRepo,
            ITeacherRepository teacherRepo)
        {
            this.departmentRepo = departmentRepo;
            this.courseRepo = courseRepo;
            this.teacherRepo=teacherRepo;
        }

        public IActionResult Index()
        {
            var courses = courseRepo.GetAll();
            var departments = departmentRepo.GetAll();
            var departmentVM = departments.Select(d => new DepartmentViewModel
            {
                DepartmentID = d.DepartmentID,
                Name = d.Name,
                Courses = string.Join(", ", d.Courses.Select(c => c.Name)),
                Teachers = string.Join("- ", d.Teachers.Select(c => c.Name))
            }).ToList();

            return View(departmentVM);
        }

        public IActionResult Details(int id)
        {
            var department = departmentRepo.GetById(id);
            if (department == null) return NotFound();
            var departmentVM = new DepartmentViewModel
            {
                DepartmentID = department.DepartmentID,
                Name = department.Name,
                Courses = string.Join("- ", department.Courses.Select(c => c.Name)),
                Teachers = string.Join("- ", department.Teachers.Select(c => c.Name))
            };
            return View(departmentVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new DepartmentViewModel
            {
                AllCourses = courseRepo.GetAll(),
                AllTeachers = teacherRepo.GetAll()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllCourses = courseRepo.GetAll();
                model.AllTeachers = teacherRepo.GetAll();
                return View(model);
            }
            var maxId = departmentRepo.GetAll().Any()
                ? departmentRepo.GetAll().Max(c => c.DepartmentID)
                : 0;
            var dept = new Department
            {
                DepartmentID = maxId + 1,
                Name = model.Name,
                Courses = courseRepo.GetAll().Where(c => model.SelectedCourseIDs.Contains(c.CourseID)).ToList(),
                Teachers = teacherRepo.GetAll().Where(t => model.SelectedTeacherIDs.Contains(t.TeacherID)).ToList()
            };

            departmentRepo.Add(dept);
            departmentRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dept = departmentRepo.GetById(id);

            if (dept == null)
                return NotFound();

            var viewModel = new DepartmentViewModel
            {
                DepartmentID = dept.DepartmentID,
                Name = dept.Name,
                AllCourses = courseRepo.GetAll(),
                AllTeachers = teacherRepo.GetAll(),
                SelectedCourseIDs = dept.Courses.Select(c => c.CourseID).ToList(),
                SelectedTeacherIDs = dept.Teachers.Select(t => t.TeacherID).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllCourses = courseRepo.GetAll();
                model.AllTeachers = teacherRepo.GetAll();
                return View(model);
            }

            var dept = departmentRepo.GetById(model.DepartmentID);

            if (dept == null)
                return NotFound();

            dept.Name = model.Name;

            // Update Courses
            dept.Courses.Clear();
            var selectedCourses = courseRepo.GetAll().Where(c => model.SelectedCourseIDs.Contains(c.CourseID)).ToList();
            foreach (var course in selectedCourses)
            {
                dept.Courses.Add(course);
            }

            // Update Teachers
            dept.Teachers.Clear();
            var selectedTeachers = teacherRepo.GetAll().Where(t => model.SelectedTeacherIDs.Contains(t.TeacherID)).ToList();
            foreach (var teacher in selectedTeachers)
            {
                dept.Teachers.Add(teacher);
            }

            departmentRepo.Update(dept);
            departmentRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var dept = departmentRepo.GetById(id);

            if (dept == null)
                return NotFound();

            var viewModel = new DepartmentViewModel
            {
                DepartmentID = dept.DepartmentID,
                Name = dept.Name,
                Courses = string.Join(", ", dept.Courses.Select(c => c.Name)),
                Teachers = string.Join(", ", dept.Teachers.Select(t => t.Name))
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var dept = departmentRepo.GetById(id);

            if (dept == null)
                return NotFound();

            if (dept.Courses.Any() || dept.Teachers.Any())
            {
                TempData["Error"] = "Cannot delete department because it has assigned teachers or courses.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            departmentRepo.Delete(id);
            departmentRepo.SaveChanges();
            TempData["Success"] = "Department deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
