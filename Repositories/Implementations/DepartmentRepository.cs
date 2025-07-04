using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
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
            return db.Departments.Include(d => d.Teachers).ToList();
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
            throw new NotImplementedException();
        }

        public void ReseedTable(string tableName, int seedValue = 0)
        {
            throw new NotImplementedException();
        }
    }
}







