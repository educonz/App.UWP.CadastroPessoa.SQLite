using App.SQLite.Core;
using App.SQLite.Helpers;
using App.SQLite.Model;
using SQLite.Net;
using System.Collections.Generic;
using System.Linq;

namespace App.SQLite.Services
{
    public abstract class ServiceBase<T> : IOperable<T>, IReadable<T> where T : class, new()
    {
        private readonly DatabaseHelper _database;

        public ServiceBase()
        {
            _database = new DatabaseHelper();
        }

        public void Delete(T entity)
        {
            using (var connection = _database.OpenConnection())
            {
                var existingRegister = ExistingRegisterById(connection, (IEntity)entity);
                if (existingRegister != null)
                {
                    connection.RunInTransaction(() =>
                    {
                        connection.Delete(existingRegister);
                    });
                }
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var connection = _database.OpenConnection())
            {
                return connection.Table<T>().ToArray();
            }
        }

        public void Insert(T entity)
        {
            using (var connection = _database.OpenConnection())
            {
                connection.RunInTransaction(() =>
                {
                    connection.Insert(entity);
                });
            }
        }

        public void Update(T entity)
        {
            using (var connection = _database.OpenConnection())
            {
                var existingRegister = ExistingRegisterById(connection, ((IEntity)entity));
                if (existingRegister != null)
                {
                    connection.RunInTransaction(() =>
                    {
                        connection.Update(entity);
                    });
                }
            }
        }

        private T ExistingRegisterById(SQLiteConnection connection, IEntity entity)
        {
            return connection.Query<T>($"select * from {entity.GetType().Name} where Id =" + entity.Id).FirstOrDefault();
        }
    }
}
