using System.Collections.Generic;

namespace App.SQLite.Core
{
    public interface IReadable<T> where T : class, new()
    {
        IEnumerable<T> GetAll();
    }
}
