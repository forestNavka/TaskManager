using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TaskManager.Models;
using TaskManager.Models.DAL;

namespace TaskManager.Controllers
{
    /// <summary>
    /// Provides handling of requests to categories
    /// </summary>
    public class CategoriesController : ApiController
    {
        private Repository<Category> _repository = new Repository<Category>();
        private bool _disposed = false;

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>IEnumerable of all categories</returns>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _repository.GetAll().ToList();
        }

        /// <summary>
        /// Gets category by id
        /// </summary>
        /// <param name="id">id of category</param>
        /// <returns>Category with current id</returns>
        [HttpGet]
        public Category Get(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Adds new category
        /// </summary>
        /// <param name="category">Category for adding</param>
        [HttpPost]
        public void Post([FromBody]Category category)
        {
            category.Id = Guid.NewGuid();
            _repository.Add(category);
            _repository.Save();
        }

        /// <summary>
        /// Updates category
        /// </summary>
        /// <param name="id">id of category for updating</param>
        /// <param name="category">New category value</param>
        [HttpPut]
        public void Put(Guid id, [FromBody]Category category)
        {
            Category native = _repository.GetById(id);
            native = category;
            _repository.Update(native);
            _repository.Save();
        }

        /// <summary>
        /// Deletes category, that matches id
        /// </summary>
        /// <param name="id">id of category for deleting</param>
        [HttpDelete]
        public void Delete(Guid id)
        {
            _repository.Delete(_repository.GetById(id));
            _repository.Save();
        }

        #region Disposing

        /// <summary>
        /// Overriding of dispose method of ApiController for adding context disposing
        /// </summary>
        /// <param name="disposing">Disposing state</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }
             _repository.Dispose();
             base.Dispose(disposing);
            _disposed = true;
        }

        #endregion Disposing
    }
}