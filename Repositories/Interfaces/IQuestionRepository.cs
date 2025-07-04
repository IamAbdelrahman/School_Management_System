using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface IQuestionRepository:IRepository<Question>
    {
            IEnumerable<Question> GetQuestionsByExamId(int examId);
        
    }
}
