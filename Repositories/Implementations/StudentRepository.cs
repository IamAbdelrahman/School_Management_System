using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        ITIContext context;
        public StudentRepository(ITIContext _context)
        {
            context = _context;
        }

        //ADD
        public void Add(Student st)
        {
            context.Students.Add(st);
        }

        //GetByID
        public Student GetById(int id)
        {
            return context.Students.FirstOrDefault(st => st.StudentID == id);
        }

        //GetAll
        public IEnumerable<Student> GetAll()
        {
            return context.Students.ToList();
        }

        //Delete
        public void Delete(int id)
        {
            Student studentFromDB = GetById(id);
            context.Students.Remove(studentFromDB);
        }

     
        public void ReseedTable(string tableName, int seedValue = 0)
        {
            throw new NotImplementedException();
        }

        //Update Student
        public void Update(Student entity)
        {
            //get old Ref
            Student studentFromDb = GetById(entity.StudentID);
            //new 
            studentFromDb.Name = entity.Name;
            studentFromDb.Address = entity.Address;
            studentFromDb.Phone = entity.Phone;
            studentFromDb.Age = entity.Age;
        }

        //Save
        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IEnumerable<Student> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount)
        {
            var query = context.Students
                 .Include(s => s.Class)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Name.Contains(searchTerm));
            }

            totalCount = query.Count();

            return query
                .OrderBy(s => s.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
