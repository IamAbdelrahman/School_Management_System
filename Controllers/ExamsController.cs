using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    [Authorize(Roles = "Admin,Teacher, Parent")]
    public class ExamsController : Controller
    {
        private readonly IExamRepository _examRepo;
        private readonly ITIContext _context;

        public ExamsController(IExamRepository examRepo, ITIContext context)
        {
            _examRepo = examRepo;
            _context = context;
        }

        public IActionResult Index(string searchTerm, int pageNumber = 1)
        {
            const int pageSize = 5;
            var exams = _examRepo.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                exams = exams.Where(e =>
                    e.Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.Grade.ToString().Contains(searchTerm));
            }

            var totalExams = exams.Count();
            var totalPages = (int)Math.Ceiling(totalExams / (double)pageSize);

            var paginatedExams = exams
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm;

            return View(paginatedExams);
        }


        public IActionResult Details(int id)
        {
            var exam = _examRepo.GetById(id);
            if (exam == null)
            {
                return NotFound();
            }

            ViewBag.Questions = _examRepo.GetQuestionsByExamId(id);
            return View(exam);
        }

        public IActionResult Create()
        {
            var examViewModel = new ExamViewModel
            {
                Courses = _context.Courses
                    .Select(c => new SelectListItem
                    {
                        Value = c.CourseID.ToString(),
                        Text = c.Name
                    }).ToList()
            };

            return View(examViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExamViewModel examViewModel)
        {
            if (!ModelState.IsValid)
            {
                examViewModel.Courses = _context.Courses.Select(c => new SelectListItem
                {
                    Value = c.CourseID.ToString(),
                    Text = c.Name
                }).ToList();

                return View(examViewModel);
            }

            int nextId = _context.Exams.Any() ? _context.Exams.Max(e => e.ExamID) + 1 : 1;

            var exam = new Exam
            {
                ExamID = nextId, 
                Type = examViewModel.Type,
                Grade = examViewModel.Grade,
                CourseID = examViewModel.CourseID
            };

            _examRepo.Add(exam);
            _examRepo.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var exam = _examRepo.GetById(id);
            if (exam == null) return NotFound();

            var examViewModel = new ExamViewModel
            {
                ExamID = exam.ExamID,
                Type = exam.Type,
                Grade = exam.Grade.Value,
                CourseID = exam.CourseID,
                Courses = _context.Courses.Select(c => new SelectListItem
                {
                    Value = c.CourseID.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(examViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ExamViewModel examViewModel)
        {
            if (!ModelState.IsValid)
            {
                examViewModel.Courses = _context.Courses.Select(c => new SelectListItem
                {
                    Value = c.CourseID.ToString(),
                    Text = c.Name
                }).ToList();
                return View(examViewModel);
            }

            var exam = _examRepo.GetById(examViewModel.ExamID);
            if (exam == null) return NotFound();

            exam.Type = examViewModel.Type;
            exam.Grade = examViewModel.Grade;
            exam.CourseID = examViewModel.CourseID;

            _examRepo.Update(exam);
            _examRepo.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var exam = _examRepo.GetById(id);
            if (exam == null) return NotFound();

            bool hasQuestions = _context.Questions.Any(q => q.ExamID == id);
            ViewBag.HasQuestions = hasQuestions;

            return View(exam);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _examRepo.Delete(id);
            _examRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
