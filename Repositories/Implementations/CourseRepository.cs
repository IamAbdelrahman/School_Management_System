using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private  ITIContext db { get; set; }
        public CourseRepository(ITIContext dbcontext)
        {
            db = dbcontext;
        }
        void IRepository<Course>.Add(Course entity)
        {
            db.Courses.Add(entity);
        }

        void IRepository<Course>.Delete(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid course ID.", nameof(id));
            var course = db.Courses.Find(id);
            if (course == null) throw new KeyNotFoundException($"Course with ID {id} not found.");
            db.Courses.Remove(course);
        }

        IEnumerable<Course> IRepository<Course>.GetAll()
        {
            return db.Courses.ToList();
        }

        Course IRepository<Course>.GetById(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid course ID.", nameof(id));
            var course = db.Courses.Find(id);
            if (course == null) throw new KeyNotFoundException($"Course with ID {id} not found.");
            return course;
        }

        IEnumerable<Course> ICourseRepository.GetCoursesByClassId(int classId)
        {
            //var courses = db.Courses
            //    .Include(c => c.Enrollments)
            //    .ThenInclude(e => e.Student)
            //    .Where(c => c.Enrollments.Any(e => e.ClassID == classId))
            //    .ToList();
            //var courses = db.Courses.Include(c => c.Teacher.)
            throw new NotImplementedException("Method GetCoursesByClassId is not implemented yet.");
        }

        IEnumerable<Course> ICourseRepository.GetCoursesByDepartmentId(int departmentId)
        {
            var courses = db.Courses
                .Include(c => c.Department)
                .Where(c => c.DepartmentID == departmentId)
                .ToList();
            if (courses == null || !courses.Any())
                return Enumerable.Empty<Course>();
            return courses;
        }

        IEnumerable<Course> ICourseRepository.GetCoursesByName(string name)
        {
            var courses = db.Courses.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (courses == null || !courses.Any())
                return Enumerable.Empty<Course>();
            return courses;
        }


        IEnumerable<Course> ICourseRepository.GetCoursesByTeacherId(int teacherId)
        {
            throw new NotImplementedException();
        }

        void IRepository<Course>.SaveChanges()
        {
            db.SaveChanges();
        }

        void IRepository<Course>.Update(Course entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity), "Course entity cannot be null.");
            if (entity.CourseID <= 0) throw new ArgumentException("Invalid course ID.", nameof(entity.CourseID));
            db.Update(entity);
            db.SaveChanges();
        }

        public IEnumerable<Course> GetCoursesByStudentId(int studentId)
        {
            throw new NotImplementedException();
        }
        public void ReseedTable(string tableName, int seedValue = 0)
        {
            var sql = $"DBCC CHECKIDENT ('{tableName}', RESEED, {seedValue})";
            db.Database.ExecuteSqlRaw(sql);
        }
    }
}
