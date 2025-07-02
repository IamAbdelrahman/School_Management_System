using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private ITIContext db { get; set; }
        public DepartmentRepository(ITIContext dbcontext)
        {
            db = dbcontext;
        }

        public IEnumerable<Department> GetDepartmentsByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Department> GetAll()
        {
            return db.Departments.ToList();
        }

        public Department GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(Department entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Department entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
