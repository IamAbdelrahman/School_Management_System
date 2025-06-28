using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System.Repositories.Implementations
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ITIContext _context;
        public TeacherRepository(ITIContext context)
        {
            _context = context;
        }
        public IEnumerable<Teacher> GetAll()
        {
            return _context.Teachers.ToList();
        }
        public Teacher? GetById(int id)
        {
            return _context.Teachers.FirstOrDefault(t => t.TeacherID == id);
        }
    }
}
