using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace School_Management_System.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ITIContext _context;

        public QuestionRepository(ITIContext context)
        {
            _context = context;
        }

        public IEnumerable<Question> GetAll()
        {
            return _context.Questions.Include(q => q.Exam).ToList();
        }

        public Question GetById(int id)
        {
            return _context.Questions.Include(q => q.Exam).FirstOrDefault(q => q.QuestionID == id)
                ?? throw new KeyNotFoundException($"Question with ID {id} not found.");
        }

        public void Add(Question entity)
        {
            Console.WriteLine("➡️ Add() called with:");
            Console.WriteLine($" - Body: {entity.Body}");
            Console.WriteLine($" - Mark: {entity.Mark}");
            Console.WriteLine($" - ExamID: {entity.ExamID}");

            _context.Questions.Add(entity);
        }


        public void Update(Question entity)
        {
            _context.Questions.Update(entity);
        }

        public void Delete(int id)
        {
            var question = _context.Questions.Find(id);
            if (question != null)
                _context.Questions.Remove(question);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void ReseedTable(string tableName, int seedValue = 0)
        {
            var sql = $"DBCC CHECKIDENT ('[{tableName}]', RESEED, {seedValue})";
            _context.Database.ExecuteSqlRaw(sql);
        }

        public IEnumerable<Question> GetQuestionsByExamId(int examId)
        {
            return _context.Questions
                           .Where(q => q.ExamID == examId)
                           .Include(q => q.Exam)
                           .ToList();
        }
    }
}
