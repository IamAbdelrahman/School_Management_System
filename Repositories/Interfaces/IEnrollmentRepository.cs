using School_Management_System.Models;

namespace School_Management_System.Repositories.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {

        IEnumerable<Enrollment> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount);
        List<Course> GetCoursesByStudentId(int studentId);
    

    }
}
