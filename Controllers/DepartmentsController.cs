using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create() => View();
        [HttpPost]
        public IActionResult Save_Dept(Department dept)
        { 
            if (ModelState.IsValid)
            {
                try
                {
                    departmentRepo.Add(dept);
                    departmentRepo.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the department: " + ex.Message);
                    return View("New", dept);
                }
            }
            else
            {
                return View("New", dept);
            }
        }
        public IActionResult Edit(int id)
        {
            var dept = departmentRepo.GetById(id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost]
        public IActionResult Update_Dept(Department dept)
        {
            if (!ModelState.IsValid) return View(dept);
            departmentRepo.Update(dept);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            departmentRepo.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
