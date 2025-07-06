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
        public IActionResult Details(int id)
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
                TeacherName = course.Teacher?.Name,
                DepartmentName = course.Department?.Name
            };
            return View("Details", courseViewModel);
        }

    }
}