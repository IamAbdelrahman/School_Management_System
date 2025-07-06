using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System.Repositories.Implementations
{
    public class ExamRepository:IExamRepository
    {
        private readonly ITIContext _context;

        public ExamRepository(ITIContext context)
        {
            _context = context;
        }

        public IEnumerable<Exam> GetAll()
        {
            return _context.Exams
                .Include(e => e.Course)
                .Include(e => e.Questions)
                .ToList();
        }

        public Exam GetById(int id)
        {
            return _context.Exams
                .Include(e => e.Course)
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamID == id) ?? throw new KeyNotFoundException($"Exam with ID {id} not found.");
        }

        public void Add(Exam entity)
        {
            _context.Exams.Add(entity);
        }

        public void Update(Exam entity)
        {
            _context.Exams.Update(entity);
        }

        public void Delete(int id)
        {
            var exam = _context.Exams.Find(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Question> GetQuestionsByExamId(int examId)
        {
            return _context.Questions
                .Where(q => q.ExamID == examId)
                .ToList();
        }
        public void ReseedTable(string tableName, int seedValue = 0)
        {
            var sql = $"DBCC CHECKIDENT ('[{tableName}]', RESEED, {seedValue})";
            _context.Database.ExecuteSqlRaw(sql);
        }


    }
}
