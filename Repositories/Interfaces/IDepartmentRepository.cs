using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface IDepartmentRepository:IRepository<Department>
    {
        // Add Custome Read operations here
        public IEnumerable<Department> GetDepartmentsByName(string name);
        public IEnumerable<Department> GetDepartmentsByCourseId(int courseId);
        public IEnumerable<Department> GetDepartmentsByTeacherId(int teacherId);
        public IEnumerable<Department> GetDepartmentsByStudentId(int studentId);
        public IEnumerable<Department> GetDepartmentsByClassId(int classId);


    }
}
