using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    [Authorize(Roles = "Admin,Teacher, Parent")]
    public class EnrollmentsController : Controller
    {
        IEnrollmentRepository enRepo;
        IStudentRepository stRepo;
        ICourseRepository crsRepol;


        public EnrollmentsController(IEnrollmentRepository enrollRepo, IStudentRepository studentRepo, ICourseRepository courseRepo)
        {
            enRepo = enrollRepo;
            stRepo = studentRepo;
            crsRepol = courseRepo;

        }


        //////////////////////////////Index
        public IActionResult Index(string? searchTerm, int pageNumber = 1, int pageSize = 5)
        {
            var enrollments = enRepo.GetPagedAndFiltered(searchTerm, pageNumber, pageSize, out int totalCount);
            var enrollmentViewModel = enrollments.Select(e => new EnrollmentViewModel
            {
                EnrollmentID = e.EnrollmentID,
                EnrollmentDate = e.EnrollmentDate,
                CourseID = e.CourseID,
                Course = e.Course,
                StudentID = e.StudentID,
                Student = e.Student
                
            }
            ).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(enrollmentViewModel);

        }

        //////////////////////////////Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Students"] = stRepo.GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.StudentID.ToString(),
                    Text = $"{s.Name} (ID: {s.StudentID}, Age: {s.Age})"
                }).ToList();
            ViewData["Courses"] = crsRepol.GetAll();
            return View("Create");
        }
        [HttpPost]
        public IActionResult Save(EnrollmentViewModel enFromReq)
        {
            //Hereeeeee For Validation as i check when add must choose student + course
            if (enFromReq.StudentID == null || enFromReq.StudentID == 0)
            {
                ModelState.AddModelError("StudentID", "Please select a student.");
            }

            if (enFromReq.CourseID == null || enFromReq.CourseID == 0)
            {
                ModelState.AddModelError("CourseID", "Please select a course.");
            }

            //Hereeeeee For Validation as i check if student enroll again to same course
            //Prevent Duplication
            var checkDuplication = enRepo.GetAll()
                                   .Any(en => en.StudentID == enFromReq.StudentID && en.CourseID == enFromReq.CourseID);
            if (checkDuplication)
            {
                ModelState.AddModelError("StudentID", "This student is already enrolled in the selected course.");
            }
            if (ModelState.IsValid == true)
            {
                try
                {
                    var maxId = enRepo.GetAll().Any()
                       ? enRepo.GetAll().Max(e => e.EnrollmentID)
                       : 0;

                    Enrollment enrollment = new Enrollment
                    {
                        EnrollmentID = maxId + 1,
                        CourseID = enFromReq.CourseID,
                        StudentID = enFromReq.StudentID,
                        EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),

                    };
                    enRepo.Add(enrollment);
                    enRepo.SaveChanges();
                    return RedirectToAction("Index");
                
                }
                catch
                {
                    ModelState.AddModelError("","error");

                }
            }
   

            ViewData["Students"] = stRepo.GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.StudentID.ToString(),
                    Text = $"{s.Name} (ID: {s.StudentID}, Age: {s.Age})"
                }).ToList();
            ViewData["Courses"] = crsRepol.GetAll();
            return View("Create", enFromReq);
        }



        ////////////////////////Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var enrollment = enRepo.GetById(id);
            if (enrollment == null) return NotFound();

            var enrollmentsViewModel = new EnrollmentViewModel
            {
                EnrollmentID = enrollment.EnrollmentID,
                EnrollmentDate= enrollment.EnrollmentDate,
                CourseID = enrollment.CourseID,
                StudentID = enrollment.StudentID,
                Student = enrollment.Student,
                Course = enrollment.Course,
               
            };

            return View(enrollmentsViewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            var enrollment = enRepo.GetById(id);
            if (enrollment == null) return NotFound();
          

            enRepo.Delete(id);
            enRepo.SaveChanges();
            TempData["Success"] = "Enrollment deleted successfully.";
            return RedirectToAction("Index");
        }

    }
}
