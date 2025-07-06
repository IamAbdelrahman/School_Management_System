using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace School_Management_System.Repositories.Implementations
{
    public class StudentExamRepository : IStudentExamRepository
    {
        private readonly ITIContext _context;

        public StudentExamRepository(ITIContext context)
        {
            _context = context;
        }

        public void Add(StudentExam entity)
        {
            int maxId = _context.StudentExams.Any()
                ? _context.StudentExams.Max(se => se.StudentExamID)
                : 0;

            entity.StudentExamID = maxId + 1;

            _context.StudentExams.Add(entity);
        }


        public void Delete(int id)
        {
            var studentExam = _context.StudentExams.Find(id);
            if (studentExam != null)
            {
                _context.StudentExams.Remove(studentExam);
            }
        }

        public IEnumerable<StudentExam> GetAll()
        {
            return _context.StudentExams
                .Include(se => se.Student)
                .Include(se => se.Exam)
                .ToList();
        }

        public StudentExam GetById(int id)
        {
            return _context.StudentExams
                .Include(se => se.Student)
                .Include(se => se.Exam)
                .FirstOrDefault(se => se.StudentExamID == id);
        }

        public StudentExam GetStudentExamById(int id)
        {
            return GetById(id);
        }

        public void Update(StudentExam entity)
        {
            _context.StudentExams.Update(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void ReseedTable(string tableName, int seedValue = 0)
        {
            string sql = $"DBCC CHECKIDENT ('{tableName}', RESEED, {seedValue})";
            _context.Database.ExecuteSqlRaw(sql);
        }
    }
}
