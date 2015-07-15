using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Models.DAL;
using TaskManager.Models.Entities;

namespace TaskManager.Models.Services
{
    public class Service<T> : IDisposable
        where T : class
    {
        private bool _disposed = false;
        protected IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T Get(Guid id)
        {
            return _repository.GetById(id);
        }

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
            _repository.Dispose();
            _disposed = true;
        }

        ~Service()
        {
            Dispose(false);
        }
        #endregion
    }
}