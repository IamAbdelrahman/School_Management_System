using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface IExamRepository:IRepository<Exam>
    {
        IEnumerable<Question> GetQuestionsByExamId(int examId);
    }
}
