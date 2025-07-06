using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Data;
using School_Management_System.Models;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;
using System;
using System.IO;

namespace School_Management_System.Controllers
{

    //public class CoursesController : Controller
    //{
    //    private readonly ICourseRepository _repo;
    //    private readonly ITeacherRepository _teacherRepo;
    //    private readonly IDepartmentRepository _DepartmentRepo;

    //    public CoursesController(ICourseRepository repo, ITeacherRepository teacherRepo, 
    //        IDepartmentRepository DepartmentRepo)
    //    {
    //        _repo = repo;
    //        _teacherRepo = teacherRepo;
    //        _DepartmentRepo = DepartmentRepo;
    //    }

    //    public IActionResult Index()
    //    {
    //        var courses =  _repo.GetAll()
    //            .Select(c => new CourseViewModel
    //            {
    //                CourseID = c.CourseID,
    //                Name = c.Name,
    //                Description = c.Description,
    //                TeacherName = c.Teacher != null ? c.Teacher.Name : "-",
    //                DepartmentName = c.Department != null ? c.Department.Name : "-"
    //            });

    //        return View(courses);
    //    }

    //    public IActionResult Create()
    //    {
    //        ViewBag.Teachers = new SelectList(_teacherRepo.GetAll(), "TeacherID", "Name");
    //        ViewBag.Departments = new SelectList(_DepartmentRepo.GetAll(), "DepartmentID", "Name");
    //        return View();
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public IActionResult Create(Course course)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _repo.Add(course);
    //            _repo.SaveChanges();
    //            return RedirectToAction(nameof(Index));
    //        }

    //        ViewBag.Teachers = new SelectList(_teacherRepo.GetAll(), "TeacherID", "Name", course.TeacherID);
    //        ViewBag.Departments = new SelectList(_DepartmentRepo.GetAll(), "DepartmentID", "Name", course.DepartmentID);
    //        return View(course);
    //    }

    //    public IActionResult Edit(int id)
    //    {
    //        var course = _repo.GetById(id);
    //        if (course == null) return NotFound();

    //        ViewBag.Teachers = new SelectList(_teacherRepo.GetAll(), "TeacherID", "Name", course.TeacherID);
    //        ViewBag.Departments = new SelectList(_DepartmentRepo.GetAll(), "DepartmentID", "Name", course.DepartmentID);
    //        return View(course);
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public IActionResult Edit(int id, Course course)
    //    {
    //        if (id != course.CourseID) return NotFound();

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _repo.Update(course);
    //                _repo.SaveChanges();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!_repo.GetAll().Any(c => c.CourseID == id))
    //                    return NotFound();
    //                throw;
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(course);
    //    }

    //    public IActionResult Delete(int id)
    //    {
    //        var course = _repo.GetAll()
    //            .FirstOrDefault(m => m.CourseID == id);
    //        if (course == null) return NotFound();

    //        return View(course);
    //    }

    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public IActionResult DeleteConfirmed(int id)
    //    {
    //        var course = _repo.GetById(id);
    //        if (course != null)
    //        {
    //            _repo.Delete(id);
    //            _repo.SaveChanges();
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //}
    [Authorize(Roles = "Admin,Teacher")]
    public class CoursesController : Controller
    {
       ICourseRepository crsRepo;
       ITeacherRepository teacRepo;
        IDepartmentRepository deptRepo;
        public CoursesController(ICourseRepository courseRepository, ITeacherRepository teacherRepository, IDepartmentRepository departmentRepository)
        {
            crsRepo = courseRepository;
            teacRepo = teacherRepository;
            deptRepo = departmentRepository;
        }


        //////////////////////////////Index
        public IActionResult Index(string? searchTerm, int pageNumber = 1, int pageSize = 5)
        {
            var courses = crsRepo.GetPagedAndFiltered(searchTerm, pageNumber, pageSize, out int totalCount);
            var courseViewModel = courses.Select(c => new CourseViewModel
            {
               
               Id = c.CourseID,
               Title = c.Name,
               DepartmentId = c.DepartmentID,
               DepartmentName = c.Department?.Name, 
               TeacherId = c.TeacherID,


            }
            ).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(courseViewModel);

        }

        //////////////////////////////Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Teachers"] = teacRepo.GetAll();
            ViewData["Departments"] = deptRepo.GetAll();
            return View("Create");
        }
        [HttpPost]
        public IActionResult Save(CourseViewModel crsFromReq)
        {
            ////Hereeeeee For Validation as i check when add must choose teacher + 
            if (crsFromReq.TeacherId == null || crsFromReq.TeacherId == 0)
            {
                ModelState.AddModelError("TeacherId", "Please select a teacher.");
            }

            if (crsFromReq.DepartmentId == null || crsFromReq.DepartmentId == 0)
            {
                ModelState.AddModelError("DepartmentId", "Please select a department.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var maxId = crsRepo.GetAll().Any()
                       ? crsRepo.GetAll().Max(c => c.CourseID)
                       : 0;

                    Course course = new Course
                    {
                        CourseID = maxId + 1,
                        Name = crsFromReq.Title,
                        Description = crsFromReq.Description,
                        TeacherID = crsFromReq.TeacherId,
                        DepartmentID = crsFromReq.DepartmentId,



                    };
                    crsRepo.Add(course);
                    crsRepo.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch
                {
                    ModelState.AddModelError("", "error");

                }
            }
            

            ViewData["Teachers"] = teacRepo.GetAll();
            ViewData["Departments"] = deptRepo.GetAll();
            return View("Create", crsFromReq);
        }

        ////////////////////////Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = crsRepo.GetById(id);
            if (course == null) return NotFound();

            var courseViewModel = new CourseViewModel
            {
                Id = course.CourseID,
                Title = course.Name,
                Description = course.Description,
                TeacherId = course.TeacherID,
                DepartmentId = course.DepartmentID,
                TeacherName = course.Teacher?.Name,
                DepartmentName = course.Department?.Name

            };

            return View(courseViewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            var course = crsRepo.GetById(id);
            if (course == null) return NotFound();
            if (course.Enrollments?.Any() == true)
            {
                TempData["Error"] = "Cannot delete course with existing enrollments.";
                return RedirectToAction("Delete", new { id });
            }

            crsRepo.Delete(id);
            crsRepo.SaveChanges();

            TempData["Success"] = "Course deleted successfully.";
            return RedirectToAction("Index");
        }


        //////////////////////////////Edit
        public IActionResult Edit(int id)
        {
            var course = crsRepo.GetById(id);
            if (course == null)
                return NotFound();

            var courseViewModel = new CourseViewModel
            {
                Id = course.CourseID,
                Title = course.Name,
                Description = course.Description,
                TeacherId = course.TeacherID,
                DepartmentId = course.DepartmentID,
                
                
            };

            ViewData["Teachers"] = teacRepo.GetAll();
            ViewData["Departments"] = deptRepo.GetAll();
            return View("Edit", courseViewModel);
        }
        [HttpPost]
        public IActionResult Edit(int id, CourseViewModel crsFromReq)
        {
            if (ModelState.IsValid)
            {
                var course = crsRepo.GetById(id);
                if (course == null)
                    return NotFound();
                course.Name = crsFromReq.Title;
                course.Description = crsFromReq.Description;
                course.TeacherID = crsFromReq.TeacherId;
                course.DepartmentID = crsFromReq.DepartmentId;
                

                crsRepo.Update(course);
                crsRepo.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewData["Teachers"] = teacRepo.GetAll();
            ViewData["Departments"] = deptRepo.GetAll();
            return View("Edit", crsFromReq);
        }




    }
}

