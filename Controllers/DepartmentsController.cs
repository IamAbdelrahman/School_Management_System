using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;

namespace School_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }


        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAllDepartments();
            return View(departments);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var department = _departmentRepository.GetById(id.Value);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DepartmentID,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                var maxId = _departmentRepository.GetAll().Any()
                    ? _departmentRepository.GetAll().Max(d => d.DepartmentID)
                    : 0;
                department.DepartmentID = maxId + 1; 
                _departmentRepository.Add(department);
                _departmentRepository.SaveChanges(); 
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }


        public  IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department =  _departmentRepository.GetById(id.Value);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("DepartmentID,Name")] Department department)
        {
            if (id != department.DepartmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     _departmentRepository.Update(department);
                    _departmentRepository.SaveChanges(); 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! _departmentRepository.GetAll().Any(d => d.DepartmentID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department =  _departmentRepository.GetById(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
             _departmentRepository.Delete(id);
            _departmentRepository.SaveChanges(); 
            return RedirectToAction(nameof(Index));
        }
    }
}
