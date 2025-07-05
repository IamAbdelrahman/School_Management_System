using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    //[Authorize(Roles = "Admin,Teacher")]
    public class TeachersController : Controller
    {
        private readonly ITeacherRepository _teacherRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private const int PageSize = 5;

        public TeachersController(ITeacherRepository teacherRepo, IDepartmentRepository departmentRepo)
        {
            _teacherRepo = teacherRepo;
            _departmentRepo = departmentRepo;
        }

        public IActionResult Index(string? searchTerm, int pageNumber = 1)
        {
            var totalCount = _teacherRepo.CountTeachers(searchTerm);
            var teachers = _teacherRepo.GetPaginatedTeachers(pageNumber, PageSize, searchTerm);

            var viewModels = teachers.Select(t => new TeacherViewModel
            {
                Id = t.TeacherID,
                Name = t.Name,
                Phone = t.Phone,
                Email = t.Email,
                HireDate = t.HireDate,
                Role = t.Role,
                DepartmentName = t.Department?.Name,
                ClassCount = t.Classes?.Count ?? 0,
                CourseCount = t.Courses?.Count ?? 0
            }).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            return View(viewModels);
        }

        public IActionResult Details(int id)
        {
            var teacher = _teacherRepo.GetById(id);
            if (teacher == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacher.TeacherID,
                Name = teacher.Name,
                Phone = teacher.Phone,
                Email = teacher.Email,
                HireDate = teacher.HireDate,
                Role = teacher.Role,
                DepartmentName = teacher.Department?.Name,
                ClassCount = teacher.Classes?.Count ?? 0,
                CourseCount = teacher.Courses?.Count ?? 0
            };

            // Send class/course names
            ViewBag.Classes = teacher.Classes?.Select(c => c.Name).ToList();
            ViewBag.Courses = teacher.Courses?.Select(c => c.Name).ToList();

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new TeacherViewModel
            {
                Departments = _departmentRepo.GetAll()
                    .Select(d => new SelectListItem { Value = d.DepartmentID.ToString(), Text = d.Name }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeacherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Departments = _departmentRepo.GetAll()
                    .Select(d => new SelectListItem { Value = d.DepartmentID.ToString(), Text = d.Name }).ToList();
                return View(viewModel);
            }
            var maxId = _teacherRepo.GetAll().Any()
                     ? _teacherRepo.GetAll().Max(t => t.TeacherID)
                     : 0;
            var teacher = new Teacher
            {
                TeacherID = maxId +1,
                Name = viewModel.Name,
                Phone = viewModel.Phone,
                Email = viewModel.Email,
                HireDate = viewModel.HireDate,
                Role = viewModel.Role,
                DepartmentID = viewModel.DepartmentId
            };

            _teacherRepo.Add(teacher);
            _teacherRepo.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var teacher = _teacherRepo.GetById(id);
            if (teacher == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacher.TeacherID,
                Name = teacher.Name,
                Phone = teacher.Phone,
                Email = teacher.Email,
                HireDate = teacher.HireDate,
                Role = teacher.Role,
                DepartmentId = teacher.DepartmentID ?? 0,
                Departments = _departmentRepo.GetAll()
                    .Select(d => new SelectListItem { Value = d.DepartmentID.ToString(), Text = d.Name }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeacherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Departments = _departmentRepo.GetAll()
                    .Select(d => new SelectListItem { Value = d.DepartmentID.ToString(), Text = d.Name }).ToList();
                return View(viewModel);
            }

            var teacher = _teacherRepo.GetById(viewModel.Id);
            if (teacher == null) return NotFound();

            teacher.Name = viewModel.Name;
            teacher.Phone = viewModel.Phone;
            teacher.Email = viewModel.Email;
            teacher.HireDate = viewModel.HireDate;
            teacher.Role = viewModel.Role;
            teacher.DepartmentID = viewModel.DepartmentId;

            _teacherRepo.Update(teacher);
            _teacherRepo.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var teacher = _teacherRepo.GetById(id);
            if (teacher == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacher.TeacherID,
                Name = teacher.Name,
                DepartmentName = teacher.Department?.Name,
                ClassCount = teacher.Classes?.Count ?? 0,
                CourseCount = teacher.Courses?.Count ?? 0
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var teacher = _teacherRepo.GetById(id);
            if (teacher == null) return NotFound();

            if (teacher.Classes.Any() || teacher.Courses.Any())
            {
                TempData["Error"] = "Cannot delete this teacher because they are assigned to classes or courses.";
                return RedirectToAction("Delete", new { id });
            }

            _teacherRepo.Delete(id);
            _teacherRepo.SaveChanges();
            return RedirectToAction("Index");
        }

    }

}
