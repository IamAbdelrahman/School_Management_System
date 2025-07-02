using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;
using System.Diagnostics.Eventing.Reader;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FuzzySharp;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection.Metadata.Ecma335;
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
            var departments = db.Departments;
            return departments.SingleOrDefault(d => d.DepartmentID == id);
        }

        public void Add(Department entity)
        {
            db.Departments.Add(entity);

        }

        public void Update(Department entity)
        {
            db.Departments.Update(entity);
        }

        public void Delete(int id)
        {
            db.Departments.Remove(GetById(id));
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
        public void ReseedTable(string tableName, int seedValue = 0)
        {
            var sql = $"DBCC CHECKIDENT ('{tableName}', RESEED, {seedValue})";
            db.Database.ExecuteSqlRaw(sql);
        }

        IEnumerable<Department> IDepartmentRepository.GetDepartmentsByName(string name)
        {
            var matchingDepartments = db.Departments
                .Where(d => Fuzz.Ratio( d.Name.ToLower(), name.ToLower())>= 80)
                .ToList();
            if (!matchingDepartments.Any())
                return Enumerable.Empty<Department>();
            return matchingDepartments;
        }

        IEnumerable<Department> IDepartmentRepository.GetDepartmentsByCourseId(int courseId)
        {
            var query = from d in db.Departments
                        join c in db.Courses on d.DepartmentID equals c.DepartmentID
                        where c.CourseID == courseId
                        select d;
            return query.ToList();
        }

        IEnumerable<Department> IDepartmentRepository.GetDepartmentsByTeacherId(int teacherId)
        {
            var query = from d in db.Departments
                        join t in db.Teachers on d.DepartmentID equals t.DepartmentID
                        where t.TeacherID == teacherId
                        select d;
            return query.ToList();
        }

        IEnumerable<Department> IDepartmentRepository.GetDepartmentsByStudentId(int studentId)
        {
            var query = from d in db.Departments
                        join c in db.Courses on d.DepartmentID equals c.DepartmentID
                        join e in db.Enrollments on c.CourseID equals e.CourseID
                        join s in db.Students on e.StudentID equals s.StudentID
                        where s.StudentID == studentId
                        select d;
            return query.ToList();
        }

        IEnumerable<Department> IDepartmentRepository.GetDepartmentsByClassId(int classId)
        {
            var query = from d in db.Departments
                        join c in db.Courses on d.DepartmentID equals c.DepartmentID
                        join e in db.Enrollments on c.CourseID equals e.CourseID
                        join s in db.Students on e.StudentID equals s.StudentID
                        where s.ClassID == classId
                        select d;
            return query.ToList();
        }

    }
}
