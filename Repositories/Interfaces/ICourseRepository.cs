using School_Management_System.Models;
using School_Management_System.ViewModel;

namespace School_Management_System.Repositories.Interfaces
{
    public interface ICourseRepository:IRepository<Course>
    {
        public IEnumerable<Course> GetCoursesByName(string name);
        public IEnumerable<Course> GetCoursesByTeacherId(int teacherId);
        public IEnumerable<Course> GetCoursesByStudentId(int studentId);
        public IEnumerable<Course> GetCoursesByClassId(int classId);
        public IEnumerable<Course> GetCoursesByDepartmentId(int departmentId);
        public IEnumerable<CourseViewModel> GetAllCoursesViewModel();
        public CourseViewModel GetCourseByIdViewModel(int id);
        public void CreateCourse(CourseViewModel courseViewModel);
        public void UpdateCourseViewModel(CourseViewModel courseViewModel);
        public bool CourseExists(int id);
        public IEnumerable<Department> GetDepartments();
        public IEnumerable<Teacher> GetTeachers();
        public IEnumerable<Course> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount);

    }
}
