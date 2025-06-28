using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        IEnumerable<Teacher> GetAll();
        Teacher? GetById(int id);
    }
}
