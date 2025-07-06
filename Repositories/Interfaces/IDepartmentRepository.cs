using School_Management_System.Models;
using School_Management_System.ViewModel;
namespace School_Management_System.Repositories.Interfaces
{
    public interface IDepartmentRepository:IRepository<Department>
    {
        // Read Operations
        public IEnumerable<Department> GetDepartmentsByName(string name);
        public IEnumerable<DepartmentViewModel> GetAllDepartments();

    }
}
