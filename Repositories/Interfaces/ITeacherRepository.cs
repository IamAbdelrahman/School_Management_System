using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface ITeacherRepository:IRepository<Teacher>
    {
        IEnumerable<Teacher> GetTeachersByDepartmentId(int departmentId);
        IEnumerable<Teacher> GetTeachersByName(string name);
        IEnumerable<Teacher> GetTeachersByRole(string role);
        IEnumerable<Teacher> GetPaginatedTeachers(int pageNumber, int pageSize, string? searchTerm);
        int CountTeachers(string? searchTerm);

    }
}
