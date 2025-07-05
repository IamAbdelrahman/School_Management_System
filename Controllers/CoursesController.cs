using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Data;
using School_Management_System.Models;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;
using System;

namespace School_Management_System.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _repo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IDepartmentRepository _DepartmentRepo;

        public CoursesController(ICourseRepository repo, ITeacherRepository teacherRepo, 
            IDepartmentRepository DepartmentRepo)
        {
            _repo = repo;
            _teacherRepo = teacherRepo;
            _DepartmentRepo = DepartmentRepo;
        }

        public IActionResult Index()
        {
            var courses =  _repo.GetAll()
                .Select(c => new CourseViewModel
                {
                    CourseID = c.CourseID,
                    Name = c.Name,
                    Description = c.Description,
                    TeacherName = c.Teacher != null ? c.Teacher.Name : "-",
                    DepartmentName = c.Department != null ? c.Department.Name : "-"
                });

            return View(courses);
        }

        public IActionResult Create()
        {
            ViewBag.Teachers = new SelectList(_teacherRepo.GetAll(), "TeacherID", "Name");
            ViewBag.Departments = new SelectList(_DepartmentRepo.GetAll(), "DepartmentID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(course);
                _repo.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teachers = new SelectList(_teacherRepo.GetAll(), "TeacherID", "Name", course.TeacherID);
            ViewBag.Departments = new SelectList(_DepartmentRepo.GetAll(), "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        public IActionResult Edit(int id)
        {
            var course = _repo.GetById(id);
            if (course == null) return NotFound();

            ViewBag.Teachers = new SelectList(_teacherRepo.GetAll(), "TeacherID", "Name", course.TeacherID);
            ViewBag.Departments = new SelectList(_DepartmentRepo.GetAll(), "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Course course)
        {
            if (id != course.CourseID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(course);
                    _repo.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.GetAll().Any(c => c.CourseID == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public IActionResult Delete(int id)
        {
            var course = _repo.GetAll()
                .FirstOrDefault(m => m.CourseID == id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _repo.GetById(id);
            if (course != null)
            {
                _repo.Delete(id);
                _repo.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

