using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TaskManager.Models.DAL;
using TaskManager.Models.DAL.SpecificRepositories;
using TaskManager.Models.Entities;
using TaskManager.Models.Services;
using System.Net.Http;
using System.Net;


namespace TaskManager.Controllers
{
    /// <summary>
    /// Provides handling of requests to categories
    /// </summary>
    public class CategoriesController : ApiController
    {
        private CategoryService _service = new CategoryService(new CategoryRepository());
        private bool _disposed = false;

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>IEnumerable of all categories</returns>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _service.GetAll();
        }

        /// <summary>
        /// Gets category by id
        /// </summary>
        /// <param name="id">id of category</param>
        /// <returns>Category with current id</returns>
        [HttpGet]
        public Category Get(Guid id)
        {
            return _service.Get(id);
        }

        /// <summary>
        /// Adds new category
        /// </summary>
        /// <param name="category">Category for adding</param>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Category category)
        {
            try
            {
                _service.AddCategory(category);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Updates category
        /// </summary>
        /// <param name="id">id of category for updating</param>
        /// <param name="category">New category value</param>
        [HttpPut]
        public HttpResponseMessage Put(Guid id, [FromBody]Category category)
        {
            try 
            {
                _service.UpdateCategory(id, category);
            }
            catch (NullReferenceException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes category, that matches id
        /// </summary>
        /// <param name="id">id of category for deleting</param>
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                _service.DeleteCategory(id);
            }
            catch (NullReferenceException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
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
             _service.Dispose();
             base.Dispose(disposing);
            _disposed = true;
        }

        #endregion Disposing
    }
}