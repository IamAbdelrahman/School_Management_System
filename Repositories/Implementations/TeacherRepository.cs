using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using System;

namespace School_Management_System.Repositories.Implementations
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ITIContext _context;

        public TeacherRepository(ITIContext context)
        {
            _context = context;
        }

        public IEnumerable<Teacher> GetAll()
        {
            return _context.Teachers.Include(t => t.Department).ToList();
        }

        public Teacher GetById(int id)
        {
            return _context.Teachers
                .Include(t => t.Department)
                .Include(t => t.Classes)
                .Include(t => t.Courses)
                .FirstOrDefault(t => t.TeacherID == id)
                ?? throw new KeyNotFoundException($"Teacher with ID {id} not found.");
        }

        public IEnumerable<Teacher> GetTeachersByDepartmentId(int departmentId)
        {
            return _context.Teachers
                .Include(t => t.Department)
                .Where(t => t.DepartmentID == departmentId)
                .ToList();
        }

        public IEnumerable<Teacher> GetTeachersByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<Teacher>();

            return _context.Teachers
                .Include(t => t.Department)
                .Where(t => EF.Functions.Like(t.Name, $"%{name}%"))
                .ToList();
        }

        public IEnumerable<Teacher> GetTeachersByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return Enumerable.Empty<Teacher>();

            return _context.Teachers
                .Include(t => t.Department)
                .Where(t => EF.Functions.Like(t.Role, $"%{role}%"))
                .ToList();
        }

        public IEnumerable<Teacher> GetTeachersByHireDate(DateOnly hireDate)
        {
            return _context.Teachers
                .Include(t => t.Department)
                .Where(t => t.HireDate == hireDate)
                .ToList();
        }

        public IEnumerable<Teacher> GetPaginatedTeachers(int pageNumber, int pageSize, string? searchTerm)
        {
            var query = _context.Teachers
                .Include(t => t.Department)
                .Include(t => t.Classes)
                .Include(t => t.Courses)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm));
            }

            return query
                .OrderBy(t => t.TeacherID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int CountTeachers(string? searchTerm)
        {
            var query = _context.Teachers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm));
            }

            return query.Count();
        }

        public void Add(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
        }

        public void Update(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
        }

        public void Delete(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void ReseedTable(string tableName, int seedValue = 0)
        {
            // Not implemented
        }
    }
}

