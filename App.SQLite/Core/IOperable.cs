using App.SQLite.Model;

namespace App.SQLite.Core
{
    public interface IOperable<T> where T : class, new()
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
