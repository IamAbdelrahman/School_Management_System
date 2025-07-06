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
        //ADD
        public void Add(Course crs)
        {
            db.Courses.Add(crs);
        }

        //GetByID
        public Course? GetById(int id)
        {
            return db.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Department)
                .Include(c => c.Enrollments)
                .FirstOrDefault(c => c.CourseID == id);
        }

        //GetAll
        public IEnumerable<Course> GetAll()
        {
            return db.Courses.ToList();
        }

        //Delete
        public void Delete(int id)
        {
            Course courseFromDB = GetById(id);
            db.Courses.Remove(courseFromDB);

        }


        public void ReseedTable(string tableName, int seedValue = 0)
        {
            throw new NotImplementedException();
        }

        //Update Student
        public void Update(Course entity)
        {
            //get old Ref
            Course courseFromDB = GetById(entity.CourseID);
            //new
            courseFromDB.Name = entity.Name;
            courseFromDB.Description = entity.Description;
            courseFromDB.TeacherID= entity.TeacherID;
            courseFromDB.DepartmentID= entity.DepartmentID;
        }

        //Save
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public IEnumerable<Course> GetPagedAndFiltered(string? searchTerm, int pageNumber, int pageSize, out int totalCount)
        {
            var query = db.Courses
                 .Include(c => c.Department)
                 .Include(c => c.Teacher)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Name.ToLower().Contains(searchTerm.ToLower()));
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