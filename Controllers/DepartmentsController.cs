using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;
using System.Runtime.Intrinsics.Arm;

namespace School_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _depRepo;
        private readonly ITeacherRepository _teacRepo;
        private readonly ICourseRepository _crsRepo;
        private readonly ITIContext _context;
        public DepartmentsController(IDepartmentRepository depRepo, ITeacherRepository teacRepo,
            ICourseRepository crsRepo)
        {
            this._depRepo = depRepo;
            this._teacRepo=teacRepo;
            this._crsRepo= crsRepo;
        }

        public IActionResult Index(string? searchTerm, int pageNumber = 1, int pageSize = 5)
        {
            var deps = _depRepo.GetPagedAndFiltered(searchTerm, pageNumber, pageSize, out int totalCount);
            var depViewModel = deps.Select(d => new DepartmentViewModel
            {
                Id = d.DepartmentID,
                Name = d.Name,
                TeacherCount = d.Teachers.Count,
                CourseCount = d.Courses.Count,
                TeacherId = d.Teachers.Any() ? d.Teachers.FirstOrDefault()?.TeacherID : 0,
                CourseId = d.Courses.Any() ? d.Courses.FirstOrDefault()?.CourseID : 0
            }
            ).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(depViewModel);

        }
        public IActionResult Details(int id)
        {
            var dep = _depRepo.GetById(id);
            if (dep == null) return NotFound();
            var deptViewModel = new DepartmentViewModel
            {
                Id = dep.DepartmentID,
                Name = dep.Name,
                TeacherCount = dep.Teachers.Count,
                CourseCount = dep.Courses.Count,
                TeacherId = dep.Teachers.Any() ? dep.Teachers.FirstOrDefault()?.TeacherID : 0,
                CourseId = dep.Courses.Any() ? dep.Courses.FirstOrDefault()?.CourseID : 0
            };

            return View(deptViewModel);
        }
        public IActionResult Create()
        {
            ViewData["Teachers"] = _teacRepo.GetAll();
            ViewData["Courses"] = _crsRepo.GetAll();
            return View("Create");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Save(DepartmentViewModel depFromReq)
        {
            if (depFromReq.TeacherId == null)
            {
                ModelState.AddModelError("TeacherId", "Please select a teacher.");
            }

            if (depFromReq.CourseId == null)
            {
                ModelState.AddModelError("DepartmentId", "Please select a department.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var maxId = _depRepo.GetAll().Any()
                       ? _depRepo.GetAll().Max(d => d.DepartmentID)
                       : 0;

                    Department dep = new Department
                    {
                        DepartmentID = maxId + 1,
                        Name = depFromReq.Name,
                        Courses = _crsRepo.GetAll().Where(c => c.CourseID == depFromReq.CourseId).ToList(),
                        Teachers = _teacRepo.GetAll().Where(t => t.TeacherID == depFromReq.TeacherId).ToList()
                    };
                    _depRepo.Add(dep);
                    _depRepo.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch
                {
                    ModelState.AddModelError("", "error");
                }
            }

            ViewData["Teachers"] = _teacRepo.GetAll();
            ViewData["Courses"] = _crsRepo.GetAll();
            return View("Create", depFromReq);
        }

        public IActionResult Edit(int id)
        {
            var dep = _depRepo.GetById(id);
            if (dep == null)
                return NotFound();

            var deptViewModel = new DepartmentViewModel
            {
                Id = dep.DepartmentID,
                Name = dep.Name,
                TeacherCount = dep.Teachers.Count,
                CourseCount = dep.Courses.Count,
                TeacherId = dep.Teachers.Any() ? dep.Teachers.FirstOrDefault()?.TeacherID : 0,
                CourseId = dep.Courses.Any() ? dep.Courses.FirstOrDefault()?.CourseID : 0

            };

            ViewData["Teachers"] = _teacRepo.GetAll();
            ViewData["Courses"] = _crsRepo.GetAll();
            return View("Edit", deptViewModel);
        }
        
        [HttpPost]
        public IActionResult Edit(int id, DepartmentViewModel depFromReq)
        {
            if (ModelState.IsValid)
            {
                var dep = _depRepo.GetById(id);
                if (dep == null)
                    return NotFound();
                dep.Name = depFromReq.Name;
                dep.Courses = _crsRepo.GetAll().Where(c => c.CourseID == depFromReq.CourseId).ToList();
                dep.Teachers = _teacRepo.GetAll().Where(t => t.TeacherID == depFromReq.TeacherId).ToList();

                _depRepo.Update(dep);
                _depRepo.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewData["Teachers"] = _teacRepo.GetAll();
            ViewData["Courses"] = _crsRepo.GetAll();
            return View("Edit", depFromReq);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var dep = _depRepo.GetById(id);
            if (dep == null) return NotFound();

            var deptViewModel = new DepartmentViewModel
            {
                Id = dep.DepartmentID,
                Name = dep.Name,
                TeacherCount = dep.Teachers.Count,
                CourseCount = dep.Courses.Count,
                TeacherId = dep.Teachers.Any() ? dep.Teachers.FirstOrDefault()?.TeacherID : 0,
                CourseId = dep.Courses.Any() ? dep.Courses.FirstOrDefault()?.CourseID : 0

            };
            if (deptViewModel.CourseCount > 0)
            {
                TempData["Error"] = "Cannot delete department.";
                return RedirectToAction("Delete", new { id });
            }
            return View(deptViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            var dep = _depRepo.GetById(id);
            if (dep == null) return NotFound();

            _depRepo.Delete(id);
            _depRepo.SaveChanges();

            TempData["Success"] = "Department deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
