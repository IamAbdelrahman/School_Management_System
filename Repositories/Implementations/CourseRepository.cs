using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using System.Diagnostics.Eventing.Reader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FuzzySharp;
using School_Management_System.ViewModel;
namespace School_Management_System.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private ITIContext db { get; set; }
        public CourseRepository(ITIContext dbcontext)
        {
            db = dbcontext;
        }
        void IRepository<Course>.Add(Course entity)
        {
            db.Courses.Add(entity);
        }
        public void CreateCourse(CourseViewModel courseViewModel)
        {
            if (courseViewModel == null) throw new ArgumentNullException(nameof(courseViewModel), "CourseViewModel cannot be null.");
            var course = new Course
            {
                Name = courseViewModel.Title,
                Description = courseViewModel.Description,
                DepartmentID = courseViewModel.DepartmentId,
                TeacherID = courseViewModel.TeacherId
            };
            db.Courses.Add(course);
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
            var courses = from course in db.Courses
                          join enrollment in db.Enrollments on course.CourseID equals enrollment.CourseID
                          join student in db.Students on enrollment.StudentID equals student.StudentID
                          where student.ClassID == classId
                          select course;

            if (courses == null || !courses.Any())
                return Enumerable.Empty<Course>();
            return courses.ToList();
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

        IEnumerable<Course> ICourseRepository.GetCoursesByName(string keyword)
        {
            var courses = db.Courses.ToList();

            var matchedCourses = courses
                .Where(c => Fuzz.Ratio(c.Name.ToLower(), keyword.ToLower()) >= 80)
                .ToList();

            if (!matchedCourses.Any())
                return Enumerable.Empty<Course>();

            return matchedCourses;
        }

        IEnumerable<Course> ICourseRepository.GetCoursesByTeacherId(int teacherId)
        {
            var courses = db.Courses
                .Include(c => c.Teacher)
                .Where(c => c.TeacherID == teacherId)
                .ToList();

            return courses.Any() ? courses : Enumerable.Empty<Course>();
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

        IEnumerable<CourseViewModel> ICourseRepository.GetAllCoursesViewModel()
        {
            var courses = db.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .Select(c => new CourseViewModel
                {
                    Id = c.CourseID,
                    Title = c.Name,
                    Description = c.Description,
                    DepartmentId = c.Department.DepartmentID,
                    DepartmentName = c.Department.Name,
                    TeacherId = c.Teacher.TeacherID,
                    TeacherName = c.Teacher.Name
                })
                .ToList();
            return courses.Any() ? courses : Enumerable.Empty<CourseViewModel>();
        }


        CourseViewModel ICourseRepository.GetCourseByIdViewModel(int id)
        {
            var course = db.Courses.Include(c => c.Department)
                .Include(c => c.Teacher)
                .SingleOrDefault(c => c.CourseID == id) ?? throw new KeyNotFoundException($"Course with ID {id} not found.");
            var courseVM = new CourseViewModel
            {
                Id = course.CourseID,
                Title = course.Name,
                Description = course.Description,
                DepartmentId = course.DepartmentID ?? 0,
                DepartmentName = course.Department?.Name ?? "N/A",
                TeacherId = course.TeacherID ?? 0,
                TeacherName = course.Teacher?.Name ?? "N/A"
            };
            return courseVM;
        }

        void ICourseRepository.UpdateCourseViewModel(CourseViewModel courseViewModel)
        {
            if (courseViewModel == null) throw new ArgumentNullException(nameof(courseViewModel), "CourseViewModel cannot be null.");
            var course = db.Courses.Find(courseViewModel.Id) ?? throw new KeyNotFoundException($"Course with ID {courseViewModel.Id} not found.");
            course.Name = courseViewModel.Title;
            course.Description = courseViewModel.Description;
            course.DepartmentID = courseViewModel.DepartmentId;
            course.TeacherID = courseViewModel.TeacherId;
            db.Update(course);
            db.SaveChanges();
        }

        bool ICourseRepository.CourseExists(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid course ID.", nameof(id));
            return db.Courses.Any(c => c.CourseID == id);
        }

        IEnumerable<Department> ICourseRepository.GetDepartments()
        {
            var departments = db.Departments.ToList();
            if (departments == null || !departments.Any())
                return Enumerable.Empty<Department>();
            return departments;
        }

        IEnumerable<Teacher> ICourseRepository.GetTeachers()
        {
            var teachers = db.Teachers.ToList();
            if (teachers == null || !teachers.Any())
                return Enumerable.Empty<Teacher>();
            return teachers;
        }

        public IEnumerable<Course> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount)
        {
            var query = db.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .Include(c => c.Enrollments)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            totalCount = query.Count();
            return query
                .OrderBy(c => c.CourseID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
