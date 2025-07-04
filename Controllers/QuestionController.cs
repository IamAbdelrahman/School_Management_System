using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepo;
        private readonly ITIContext _context;

        public QuestionController(IQuestionRepository questionRepo, ITIContext context)
        {
            _questionRepo = questionRepo;
            _context = context;
        }

        public IActionResult Index(string searchTerm, int pageNumber = 1)
        {
            const int pageSize = 6;

            List<Question> allQuestions = _questionRepo.GetAll().ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allQuestions = allQuestions
                    .Where(q => q.Body.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                (!string.IsNullOrEmpty(q.Header) && q.Header.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            int totalQuestions = allQuestions.Count;
            int totalPages = (int)Math.Ceiling(totalQuestions / (double)pageSize);

            List<Question> paginatedQuestions = allQuestions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm;

            return View(paginatedQuestions);
        }

        public IActionResult Details(int id)
        {
            var question = _questionRepo.GetById(id);
            if (question == null) return NotFound();
            return View(question);
        }

        public IActionResult Create()
        {
            var questionViewModel = new QuestionViewModel
            {
                Exams = _context.Exams.Select(e => new SelectListItem
                {
                    Value = e.ExamID.ToString(),
                    Text = $"{e.Type} - Grade: {e.Grade}"
                }).ToList()
            };

            return View(questionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuestionViewModel questionViewModel)
        {
            if (!ModelState.IsValid)
            {
                questionViewModel.Exams = _context.Exams.Select(e => new SelectListItem
                {
                    Value = e.ExamID.ToString(),
                    Text = $"{e.Type} - Grade: {e.Grade}"
                }).ToList();

                return View(questionViewModel);
            }

            var question = new Question
            {
                Body = questionViewModel.Body,
                Header = questionViewModel.Header,
                Mark = questionViewModel.Mark,
                CorrectAnswer = questionViewModel.CorrectAnswer,
                Type = questionViewModel.Type,
                ExamID = questionViewModel.ExamID.Value 
            };

            _questionRepo.Add(question);
            _questionRepo.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var question = _questionRepo.GetById(id);
            if (question == null) return NotFound();

            var vm = new QuestionViewModel
            {
                QuestionID = question.QuestionID,
                Header = question.Header,
                Body = question.Body,
                Mark = question.Mark,
                CorrectAnswer = question.CorrectAnswer,
                Type = question.Type,
                ExamID = question.ExamID,
                Exams = _context.Exams.Select(e => new SelectListItem
                {
                    Value = e.ExamID.ToString(),
                    Text = $"{e.Type} - Grade: {e.Grade}"
                }).ToList()
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuestionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Exams = _context.Exams.Select(e => new SelectListItem
                {
                    Value = e.ExamID.ToString(),
                    Text = $"{e.Type} - Grade: {e.Grade}"
                }).ToList();

                return View(vm);
            }

            var existingQuestion = _questionRepo.GetById(vm.QuestionID);
            if (existingQuestion == null) return NotFound();

            existingQuestion.Body = vm.Body;
            existingQuestion.Header = vm.Header;
            existingQuestion.Mark = vm.Mark;
            existingQuestion.CorrectAnswer = vm.CorrectAnswer;
            existingQuestion.Type = vm.Type;
            existingQuestion.ExamID = vm.ExamID.Value;

            _questionRepo.Update(existingQuestion);
            _questionRepo.SaveChanges();

            TempData["Success"] = "Question updated successfully!";
            return RedirectToAction(nameof(Index));
        }


        // GET: Question/Delete/5
        public IActionResult Delete(int id)
        {
            var question = _questionRepo.GetById(id);
            if (question == null) return NotFound();
            return View(question);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _questionRepo.Delete(id);
            _questionRepo.SaveChanges();
            TempData["Success"] = "Question deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

    }
}
