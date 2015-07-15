using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManager.Models.DAL;
using TaskManager.Models.DAL.SpecificRepositories;
using TaskManager.Models.Entities;
using TaskManager.Models.Services;

namespace TaskManager.Controllers
{
    /// <summary>
    /// Provides handling of requests to subtasks
    /// </summary>
    public class SubtasksController : ApiController
    {
        private SubtaskService _service = new SubtaskService(new SubtaskRepository());
        private bool _disposed = false;

        /// <summary>
        /// Gets all subtasks that match the query
        /// </summary>
        /// <param name="query">The part of sql query after where keyword</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Subtask> Get(string query = null)
        {
            if (query == null)   
               return _service.GetAll();
            return _service.GetByQuery(query);
        }

        /// <summary>
        /// Gets subtask by id
        /// </summary>
        /// <param name="id">Id of subtask</param>
        /// <returns>Subtask, which has current id</returns>
        [HttpGet]
        public Subtask Get(Guid id)
        {
                return _service.Get(id);
        }

       /// <summary>
       /// Adds new subtask
       /// </summary>
       /// <param name="subtask">Subtask for adding</param>
       /// <returns>Http responce message</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Subtask subtask)
        {
            if (subtask == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            try
            {
                _service.AddSubtask(subtask, new TaskRepository());
                
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Update the state of subtask
        /// </summary>
        /// <param name="id">id of subtask for updating</param>
        /// <param name="isDone">New state of subtask</param>
        /// <returns>Http response message</returns>
        [HttpPut]
        public HttpResponseMessage Put([FromUri]Guid id, [FromBody]bool isDone)
        {
            try
            {
                _service.UpdateSubtask(id, isDone, new TaskRepository());
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
        /// Deletes subtask which has current id
        /// </summary>
        /// <param name="id">Id of subtask for deleting</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                _service.DeleteSubtask(id);
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