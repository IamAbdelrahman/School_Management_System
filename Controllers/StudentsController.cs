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

        public StudentsController(IStudentRepository studentRepo, IClassRepository classRepo)
        {
            stRepo = studentRepo;
            clsRepo = classRepo;
        }

        public IActionResult Index(string? searchTerm, int pageNumber = 1, int pageSize = 5)
        {
            var Headrer = stRepo.GetPagedAndFiltered(searchTerm, pageNumber, pageSize, out int totalCount);
            var students = stRepo.GetAll();
            var studentsViewModel = students.Select(s => new StudentViewModel
            {
                StudentID = s.StudentID,
                Name = s.Name,
                Age = s.Age,
                Address = s.Address,
                Gender = s.Gender,
                Phone = s.Phone,
                ClassID = s.ClassID,
                ClassName = s.Class.Name,

            }
            ).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(studentsViewModel);

        }
    }
}
