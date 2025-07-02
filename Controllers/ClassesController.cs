using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ClassesController : Controller
    {
        private readonly IClassRepository _classRepo;
        private readonly ITeacherRepository _teacherRepo;
        public ClassesController(IClassRepository classRepo, ITeacherRepository teacherRepo)
        {
            _classRepo = classRepo;
            _teacherRepo = teacherRepo;
        }

        //-------------------------Index--------------------//
        public IActionResult Index(string? searchTerm, int pageNumber = 1, int pageSize = 5)
        {
            var classes = _classRepo.GetPagedAndFiltered(searchTerm, pageNumber, pageSize, out int totalCount);

            var viewModels = classes.Select(c => new ClassViewModel
            {
                ClassID = c.ClassID,
                Name = c.Name,
                GradeLevel = c.GradeLevel,
                TeacherID = c.TeacherID,
                TeacherName = c.Teacher?.Name,
                Students = c.Students.ToList()
            }).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(viewModels);
        }

        //-------------------------Details--------------------//
        public IActionResult Details(int id)
        {
            var cls = _classRepo.GetById(id);
            if (cls == null)
                return NotFound();
            var viewModel = new ClassViewModel
            {
                ClassID = cls.ClassID,
                Name = cls.Name,
                GradeLevel = cls.GradeLevel,
                TeacherID = cls.TeacherID,
                TeacherName = cls.Teacher?.Name,
                TeacherRole = cls.Teacher?.Role,
                Students = cls.Students.ToList()
            };
            return View(viewModel);
        }

        //-------------------------Create--------------------//
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new ClassViewModel
            {
                Teachers = _teacherRepo.GetAll()
                    .Select(t => new SelectListItem { Value = t.TeacherID.ToString(), Text = t.Name })
                    .ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Teachers = _teacherRepo.GetAll()
                    .Select(t => new SelectListItem { Value = t.TeacherID.ToString(), Text = t.Name })
                    .ToList();
                return View(model);
            }

            var maxId = _classRepo.GetAll().Any()
                ? _classRepo.GetAll().Max(c => c.ClassID)
                : 0;

            var newClass = new Class
            {
                ClassID = maxId + 1,
                Name = model.Name!,
                GradeLevel = model.GradeLevel,
                TeacherID = model.TeacherID
            };

            _classRepo.Add(newClass);
            _classRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        //-------------------------Edit--------------------//
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cls = _classRepo.GetById(id);
            if (cls == null)
                return NotFound();
            var viewModel = new ClassViewModel
            {
                ClassID = cls.ClassID,
                Name = cls.Name,
                GradeLevel = cls.GradeLevel,
                TeacherID = cls.TeacherID,
                Teachers = _teacherRepo.GetAll()
                    .Select(t => new SelectListItem { Value = t.TeacherID.ToString(), Text = t.Name })
                    .ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Teachers = _teacherRepo.GetAll()
                    .Select(t => new SelectListItem { Value = t.TeacherID.ToString(), Text = t.Name })
                    .ToList();
                return View(model);
            }
            var existingClass = _classRepo.GetById(model.ClassID);
            if (existingClass == null)
                return NotFound();
            existingClass.Name = model.Name!;
            existingClass.GradeLevel = model.GradeLevel;
            existingClass.TeacherID = model.TeacherID;
            _classRepo.Update(existingClass);
            _classRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        //-------------------------Delete--------------------//
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cls = _classRepo.GetById(id);
            if (cls == null)
                return NotFound();
            var viewModel = new ClassViewModel
            {
                ClassID = cls.ClassID,
                Name = cls.Name,
                GradeLevel = cls.GradeLevel,
                TeacherID = cls.TeacherID,
                Students = cls.Students.ToList()
            };
            return View(viewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cls = _classRepo.GetById(id);
            if (cls == null)
                return NotFound();
            if (cls.Students.Any())
            {
                TempData["Error"] = "Cannot delete class because it has assigned students.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _classRepo.Delete(id);
            _classRepo.Save();
            TempData["Success"] = "Class deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
