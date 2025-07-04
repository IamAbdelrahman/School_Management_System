namespace School_Management_System.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        public void ReseedTable(string tableName, int seedValue = 0);

        public void SaveChanges();
    }
}

