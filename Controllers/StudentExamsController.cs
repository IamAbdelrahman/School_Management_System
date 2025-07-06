using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    public class StudentExamController : Controller
    {
        private readonly IStudentExamRepository _studentExamRepo;
        private readonly IExamRepository _examRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ITIContext _context;

        public StudentExamController(IStudentExamRepository studentExamRepo, IExamRepository examRepo,
            IStudentRepository studentRepo, ITIContext context)
        {
            _studentExamRepo = studentExamRepo;
            _examRepo = examRepo;
            _studentRepo = studentRepo;
            _context = context;
        }

        public IActionResult Index(string searchTerm, int pageNumber = 1)
        {
            const int pageSize = 5;
            var studentExams = _studentExamRepo.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                studentExams = studentExams.Where(se =>
                    (se.Student != null && se.Student.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (se.Exam != null && se.Exam.Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (se.StudentGrade.HasValue && se.StudentGrade.Value.ToString().Contains(searchTerm))
                );
            }

            int totalStudentExams = studentExams.Count();
            int totalPages = (int)Math.Ceiling(totalStudentExams / (double)pageSize);

            var paginatedStudentExams = studentExams
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm;

            return View(paginatedStudentExams);
        }

        public IActionResult Details(int id)
        {
            var studentExam = _studentExamRepo.GetStudentExamById(id);
            if (studentExam == null)
                return NotFound();

            return View(studentExam);
        }
        public IActionResult Create()
        {
            var studentExamViewModel = new StudentExamViewModel
            {
                Students = _context.Students.Select(s => new SelectListItem
                {
                    Value = s.StudentID.ToString(),
                    Text = s.Name
                }).ToList(),

                Exams = _context.Exams.Select(e => new SelectListItem
                {
                    Value = e.ExamID.ToString(),
                    Text = $"{e.Type} (Grade: {e.Grade})"
                }).ToList()
            };

            return View(studentExamViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentExamViewModel studentExamViewModel)
        {
            var exam = _examRepo.GetById(studentExamViewModel.ExamID ?? 0);
            studentExamViewModel.MaxExamGrade = exam?.Grade;

            studentExamViewModel.Students = _studentRepo.GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.StudentID.ToString(),
                    Text = s.Name
                }).ToList();

            studentExamViewModel.Exams = _examRepo.GetAll()
                .Select(e => new SelectListItem
                {
                    Value = e.ExamID.ToString(),
                    Text = $"{e.Type} - Grade: {e.Grade}"
                }).ToList();

            if (!ModelState.IsValid)
            {
                return View(studentExamViewModel);
            }

            var studentExam = new StudentExam
            {
                StudentID = studentExamViewModel.StudentID,
                ExamID = studentExamViewModel.ExamID,
                StudentGrade = studentExamViewModel.StudentGrade
            };

            _studentExamRepo.Add(studentExam);
            _studentExamRepo.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var studentExam = _studentExamRepo.GetById(id);
            if (studentExam == null)
                return NotFound();

            var viewModel = new StudentExamViewModel
            {
                StudentExamID = studentExam.StudentExamID,
                StudentID = studentExam.StudentID,
                ExamID = studentExam.ExamID,
                StudentGrade = studentExam.StudentGrade,
                MaxExamGrade = studentExam.Exam?.Grade,

                Students = _context.Students.Select(s => new SelectListItem
                {
                    Value = s.StudentID.ToString(),
                    Text = s.Name
                }).ToList(),

                Exams = _context.Exams.Select(e => new SelectListItem
                {
                    Value = e.ExamID.ToString(),
                    Text = $"{e.Type} (Grade: {e.Grade})"
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentExamViewModel viewModel)
        {
            var exam = _context.Exams.FirstOrDefault(e => e.ExamID == viewModel.ExamID);
            viewModel.MaxExamGrade = exam?.Grade;

            viewModel.Students = _context.Students.Select(s => new SelectListItem
            {
                Value = s.StudentID.ToString(),
                Text = s.Name
            }).ToList();

            viewModel.Exams = _context.Exams.Select(e => new SelectListItem
            {
                Value = e.ExamID.ToString(),
                Text = $"{e.Type} (Grade: {e.Grade})"
            }).ToList();

            if (!ModelState.IsValid)
                return View(viewModel);

            var studentExam = _studentExamRepo.GetById(viewModel.StudentExamID);
            if (studentExam == null)
                return NotFound();

            studentExam.StudentID = viewModel.StudentID;
            studentExam.ExamID = viewModel.ExamID;
            studentExam.StudentGrade = viewModel.StudentGrade;

            _studentExamRepo.Update(studentExam);
            _studentExamRepo.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var studentExam = _studentExamRepo.GetStudentExamById(id);
            if (studentExam == null)
                return NotFound();

            bool hasReferences = false; 

            ViewBag.CanDelete = !hasReferences;

            return View(studentExam);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _studentExamRepo.Delete(id);
            _studentExamRepo.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
