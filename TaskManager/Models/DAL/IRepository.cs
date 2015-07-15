using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManager.Models.DAL
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByQuery(string query);
        T GetById(Guid id);
        void Add(T item);
        void Delete(T item);
        void Update(T item);
        void Save();
    }
}
