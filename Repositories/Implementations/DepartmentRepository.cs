using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using School_Management_System.ViewModel;
using System;

namespace School_Management_System.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private ITIContext db { get; set; }
        public DepartmentRepository(ITIContext dbcontext)
        {
            db = dbcontext;
        }

        public IEnumerable<Department> GetDepartmentsByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Department> GetAll()
        {
            return db.Departments.Include(d => d.Teachers).Include(d => d.Courses).ToList();
        }
        public IEnumerable<DepartmentViewModel> GetAllDepartments()
        {
            return db.Departments.Include(d => d.Teachers).Include(d => d.Courses)
                .Select(d => new DepartmentViewModel
                {
                    Id = d.DepartmentID,
                    Name = d.Name,
                    TeacherCount = d.Teachers.Count,
                    CourseCount = d.Courses.Count
                })
                .ToList();
        }
        public Department GetById(int id)
        {
            return db.Departments.Include(d => d.Teachers)
                .Include(d => d.Courses).SingleOrDefault(d => d.DepartmentID == id) 
                ?? throw new KeyNotFoundException($"Department with ID {id} not found.");
        }

        public void Add(Department entity)
        {
            db.Add(entity);
        }

        public void Update(Department entity)
        {
            db.Departments.Update(entity);
        }

        public void Delete(int id)
        {
            var dept = db.Departments.Find(id);
            if (dept == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found.");
            }
            if (dept != null) db.Departments.Remove(dept);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void ReseedTable(string tableName, int seedValue = 0)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Department> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount)
        {
            var query = db.Departments.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            totalCount = query.Count();
            return query
                .Include(d => d.Teachers)
                .Include(d => d.Courses)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}







