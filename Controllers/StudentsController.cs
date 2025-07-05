using Microsoft.AspNetCore.Mvc;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    public class StudentsController : Controller
    {
        IStudentRepository stRepo;
        IClassRepository clsRepo;

        public StudentsController(IStudentRepository studentRepo, IClassRepository classRepository)
        {
            stRepo = studentRepo;
            clsRepo = classRepository;
        }


        //////////////////////////////Index
        public IActionResult Index(string? searchTerm, int pageNumber = 1, int pageSize = 5)
        {
            var students = stRepo.GetPagedAndFiltered(searchTerm, pageNumber, pageSize, out int totalCount);
            var studentsViewModel = students.Select(s => new StudentViewModel
            {
                StudentID = s.StudentID,
                Name = s.Name,
                Age = s.Age,
                Gender = s.Gender,
                ClassID = s.ClassID,


            }
            ).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(studentsViewModel);

        }

        //////////////////////////////Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Classes"] = clsRepo.GetAll();
            return View("Create");
        }
        [HttpPost]
        public IActionResult Save(StudentViewModel stFromReq)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    var maxId = stRepo.GetAll().Any()
                       ? stRepo.GetAll().Max(s => s.StudentID)
                       : 0;

                    Student student = new Student
                    {
                        StudentID = maxId + 1,
                        Name = stFromReq.Name,
                        Age = stFromReq.Age,
                        ClassID = stFromReq.ClassID,

                    };
                    stRepo.Add(student);
                    stRepo.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("ClassID", "Please Select Class");
                }
            }
            ViewData["Classes"] = clsRepo.GetAll();
            return View("Create", stFromReq);
        }

        //////////////////////////////Details
        public IActionResult Details(int id)
        {
            var st = stRepo.GetById(id);
            var studentsViewModel = new StudentViewModel
            {
                Name = st.Name,
                Age = st.Age,
                Phone = st.Phone,
                Address = st.Address,
                Class = st.Class,
                ClassID = st.ClassID,
            };

            return View(studentsViewModel);
        }

        //////////////////////////////Edit
        public IActionResult Edit(int id)
        {
            var student = stRepo.GetById(id);
            if (student == null)
                return NotFound();

            var studentsViewModel = new StudentViewModel
            {
                StudentID = student.StudentID,
                Name = student.Name,
                Age = student.Age,
                Address = student.Address,
                Phone = student.Phone,
                Gender = student.Gender,
                ClassID = student.ClassID
            };

            ViewData["Classes"] = clsRepo.GetAll();
            return View("Edit", studentsViewModel);
        }
        [HttpPost]
        public IActionResult Edit(int id, StudentViewModel stFromReq)
        {
            if (ModelState.IsValid)
            {
                var student = stRepo.GetById(id);
                if (student == null)
                    return NotFound();

                student.Name = stFromReq.Name;
                student.Age = stFromReq.Age;
                student.Address = stFromReq.Address;
                student.Phone = stFromReq.Phone;
                student.Gender = stFromReq.Gender;
                student.ClassID = stFromReq.ClassID;

                stRepo.Update(student);
                stRepo.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewData["Classes"] = clsRepo.GetAll();
            return View("Edit", stFromReq);
        }



        ////////////////////////Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = stRepo.GetById(id);
            if (student == null) return NotFound();

            var studentsViewModel = new StudentViewModel
            {
                StudentID = student.StudentID,
                Name = student.Name,
                Age = student.Age,
                Address = student.Address,
                Phone = student.Phone,
                Gender = student.Gender,
                ClassID = student.ClassID
            };

            return View(studentsViewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            var student = stRepo.GetById(id);
            if (student == null) return NotFound();
            if (student.StudentExams.Any())
            {
                TempData["Error"] = "Cannot delete student with existing Exam.";
                return RedirectToAction("Delete", new { id });
            }
            if (student.Enrollments.Any())
            {
                TempData["Error"] = "Cannot delete student with existing Enrollments.";
                return RedirectToAction("Delete", new { id });
            }

            stRepo.Delete(id);
            stRepo.SaveChanges();
            TempData["Success"] = "Student deleted successfully.";
            return RedirectToAction("Index");
        }




    }

}
