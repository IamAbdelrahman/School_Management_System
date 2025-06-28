using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System.Repositories.Implementations
{
    public class ClassRepository : IClassRepository
    {
        private readonly ITIContext _context;
        public ClassRepository(ITIContext context)
        {
            _context = context;
        }
        public IEnumerable<Class> GetAll()
        {
            return _context.Classes
                .Include(c => c.Teacher)
                .Include(c => c.Students).ToList();
        }
        public Class? GetById(int id)
        {
            return _context.Classes
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .FirstOrDefault(c => c.ClassID == id);
        }
        public void Add(Class cls)
        {
            _context.Classes.Add(cls);
        }
        public void Update(Class cls)
        {
            _context.Classes.Update(cls);
        }
        public void Delete(int id)
        {
            var cls = _context.Classes.Find(id);
            if (cls != null)
                _context.Classes.Remove(cls);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
