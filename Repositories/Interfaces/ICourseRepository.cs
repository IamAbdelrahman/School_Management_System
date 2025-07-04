using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface ICourseRepository:IRepository<Course>
    {
        public IEnumerable<Course> GetCourseWithDepartments();
        public IEnumerable<Course> GetCourseWithTeachers();
        public IEnumerable<Course> GetCoursesByTeachersAndDepartments();
        public IEnumerable<Course> GetCoursesByName(string name);
        public IEnumerable<Course> GetCoursesByTeacherId(int teacherId);
        public IEnumerable<Course> GetCoursesByStudentId(int studentId);
        public IEnumerable<Course> GetCoursesByClassId(int classId);
        public IEnumerable<Course> GetCoursesByDepartmentId(int departmentId);
    }
}
