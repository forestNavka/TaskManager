using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Objects;

namespace TaskManager.Models.DAL
{
    /// <summary>
    /// Provides CRUD functionality for each entity type
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private TaskManagerContext _context = new TaskManagerContext();
        private bool _disposed = false;

        #region CRUD

        /// <summary>
        /// Gets all items of current type
        /// </summary>
        /// <returns>IEnumerable of all items</returns>
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable<T>();
        }

        /// <summary>
        /// Gets items, that matches the query
        /// </summary>
        /// <param name="query">SQL query in full format</param>
        /// <returns>IEnumerable of items, which matches the query</returns>
        public IEnumerable<T> GetByQuery(string query)
        {
            return _context.Set<T>().SqlQuery(query);
        }

        /// <summary>
        /// Gets item by id
        /// </summary>
        /// <param name="id">id of item</param>
        /// <returns>Item with current id</returns>
        public T GetById(Guid id)
        {

            return _context.Set<T>().Find(id);
        }

        /// <summary>
        /// Adds item to context
        /// </summary>
        /// <param name="item">Item for adding</param>
        public void Add(T item)
        {
            _context.Set<T>().Add(item);
        }

        /// <summary>
        /// Changes item status to deleted
        /// </summary>
        /// <param name="item">Item for deleting</param>
        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        /// <summary>
        /// Change item's status to modified
        /// </summary>
        /// <param name="item">Item for updating</param>
        public void Update(T item)
        {
            _context.Entry(item).State = System.Data.EntityState.Modified;
        }

        /// <summary>
        /// Saves changes in contaxt
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }
        #endregion CRUD

        #region Disposing
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }
             _context.Dispose();
            _disposed = true;
        }

        ~Repository()
        {
            Dispose(false);
        }
        #endregion Disposing
    }
}