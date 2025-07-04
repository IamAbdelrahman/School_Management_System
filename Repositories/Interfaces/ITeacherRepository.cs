using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface ITeacherRepository:IRepository<Teacher>
    {
        public IEnumerable<Teacher> GetTeachersByDepartmentId(int departmentId);
        public IEnumerable<Teacher> GetTeachersByName(string name);
        public IEnumerable<Teacher> GetTeachersByRole(string role);

    }
}
