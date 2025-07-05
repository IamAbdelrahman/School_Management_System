using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System.Repositories.Implementations
{
    public class StudentExamRepository:IStudentExamRepository
    {
        private readonly ITIContext db;
        public StudentExamRepository(ITIContext dbcontext)
        {
            db = dbcontext;
        }
        public IEnumerable<StudentExam> GetAll()
        {
            return db.StudentExams.Include(s => s.Student)
                                  .Include(s => s.Exam)
                                  .ToList();
        }

        void IRepository<StudentExam>.Add(StudentExam entity)
        {
            db.Add(entity);
        }

        void IRepository<StudentExam>.Delete(int id)
        {
            db.Remove(db.StudentExams.Find(id));
        }

        StudentExam IRepository<StudentExam>.GetById(int id)
        {
            return db.StudentExams.Include(s => s.Student)
                                  .Include(s => s.Exam)
                                  .FirstOrDefault(s => s.StudentExamID == id)
                ?? throw new KeyNotFoundException($"StudentExam with ID {id} not found.");
        }

        void IRepository<StudentExam>.ReseedTable(string tableName, int seedValue)
        {
            throw new NotImplementedException();
        }

        void IRepository<StudentExam>.SaveChanges()
        {
           db.SaveChanges();
        }

        void IRepository<StudentExam>.Update(StudentExam entity)
        {
            db.Update(entity);
        }
    }
}
