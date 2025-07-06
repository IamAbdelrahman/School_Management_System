using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System.Repositories.Implementations
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        ITIContext context;
        public EnrollmentRepository(ITIContext _context)
        {
            context = _context;
        }

        //Add
        public void Add(Enrollment Enro)
        {
            context.Enrollments.Add(Enro);
        }

        //Get By ID
        public Enrollment? GetById(int id)
        {
            return context.Enrollments
                .Include(Enro=> Enro.Student)
                .Include(Enro=> Enro.Course)
                .FirstOrDefault(Enro => Enro.EnrollmentID == id);
        }

        //Get All Enrollments
        public IEnumerable<Enrollment> GetAll()
        {
            return context.Enrollments.ToList();
        }

        //Delete
        public void Delete(int id)
        {
            Enrollment enrollmentFromDB = GetById(id);
            context.Enrollments.Remove(enrollmentFromDB);
        }

        //Edit 
        public void Update(Enrollment entity)
        {
            //old Ref
            Enrollment enrollmentFromDB = GetById(entity.EnrollmentID);
            //new Ref
            enrollmentFromDB.EnrollmentDate = entity.EnrollmentDate;
            enrollmentFromDB.CourseID = entity.CourseID;    
            enrollmentFromDB.StudentID = entity.StudentID;
        }

        //Save
        public void SaveChanges()
        {
            context.SaveChanges();
        }


        public IEnumerable<Enrollment> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount)
        {
            var query = context.Enrollments
                 .Include(Enro => Enro.Course)
                 .Include (Enro=> Enro.Student)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(Enro => Enro.Student.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            totalCount = query.Count();

            return query
                .OrderBy(Enro => Enro.EnrollmentID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public void ReseedTable(string tableName, int seedValue = 0)
        {
            throw new NotImplementedException();
        }

        public List<Course> GetCoursesByStudentId(int studentId)
        {
            return context.Enrollments
                 .Where(e => e.StudentID == studentId)
                 .Include(e => e.Course)
                 .Select(e => e.Course)
                 .ToList();
        }
    }
}
