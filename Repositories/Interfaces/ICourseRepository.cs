using School_Management_System.Models;
using School_Management_System.ViewModel;

namespace School_Management_System.Repositories.Interfaces
{
    public interface ICourseRepository:IRepository<Course>
    {
      
        IEnumerable<Course> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount);

    }
}
