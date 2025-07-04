using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface IClassRepository
    {
        IEnumerable<Class> GetAll();
        Class? GetById(int id);
        void Add(Class cls);
        void Update(Class cls);
        void Delete(int id);
        void Save();
        IEnumerable<Class> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount);
    }
}
