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
        private readonly ITeacherRepository _repo;
        private readonly IDepartmentRepository _deptRepo;

        public TeachersController(ITeacherRepository repo, IDepartmentRepository deptRepo)
        {
            _repo = repo;
            _deptRepo = deptRepo;
        }

        public IActionResult Index()
        {
            var techers = _repo.GetAll();
            var teacherVM = techers.Select(t => new TeacherViewModel
            {
                Id = t.TeacherID,
                Name = t.Name,
                HireDate = t.HireDate,
                DepartmentName = t.Department?.Name,
                Phone = t.Phone,
                Email = t.Email,
                Role = t.Role
            }).ToList();
            return View(teacherVM);
        }
        public IActionResult Details(int id)
        {
            var teacher = _repo.GetById(id);
            if (teacher == null) return NotFound();
            var teacherVM = new TeacherViewModel
            {
                Id = teacher.TeacherID,
                Name = teacher.Name,
                HireDate = teacher.HireDate,
                DepartmentName = teacher.Department?.Name,
                Phone = teacher.Phone,
                Email = teacher.Email,
                Role = teacher.Role
            };
            return View(teacherVM);
        }
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_deptRepo.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Save_Teacher(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Add(teacher);
                    _repo.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the teacher: " + ex.Message);
                    return View("New", teacher);
                }
            }
            else
            {
                return View("New", teacher);
            }
        }

        public IActionResult Edit(int id)
        {
            var teacher = _repo.GetById(id);
            if (teacher == null) return NotFound();

            ViewBag.Departments = new SelectList(_deptRepo.GetAll(), "Id", "Name", teacher.DepartmentID);
            return View(teacher);
        }

        [HttpPost]
        public IActionResult Update_Teacher(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_deptRepo.GetAll(), "Id", "Name", teacher.DepartmentID);
                return View(teacher);
            }

            _repo.Update(teacher);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }

    }

}
