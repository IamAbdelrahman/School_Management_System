using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        IEnumerable<Student> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount);

    }
}
