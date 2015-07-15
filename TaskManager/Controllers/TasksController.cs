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
    /// Provides handling of requests to tasks
    /// </summary>
    public class TasksController : ApiController
    {
        private TaskService _service = new TaskService(new TaskRepository());
        private bool _disposed = false;

        /// <summary>
        /// Returns set of tasks, wich match the query
        /// </summary>
        /// <param name="query">The part of sql query after where keyword</param>
        /// <returns>IEnumerable of tasks, wich match the query</returns>
        [HttpGet]
        public IEnumerable<Task> Get(string query=null)
        {
            if (query == null)
                return _service.GetAll();
            return _service.GetByQuery(query);
        }

        /// <summary>
        /// Gets task by id
        /// </summary>
        /// <param name="id">Id of task</param>
        /// <returns>Task, that has current id</returns>
        [HttpGet]
        public Task Get(Guid id)
        {
            return _service.Get(id);
        }

        /// <summary>
        /// Adds new task
        /// </summary>
        /// <param name="task">Task for adding</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Task task)
        {
            try
            {
                _service.AddTask(task);
            }

            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Updates status of task with current id
        /// </summary>
        /// <param name="id">id of task for updating</param>
        /// <param name="isDone">New task status</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        public HttpResponseMessage Put(Guid id, [FromBody]bool isDone)
        {
            try
            {
                _service.UpdateTask(id, isDone);
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
        /// Deletes all finished tasks
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        public HttpResponseMessage Delete()
        {
            try
            {
                _service.DeleteFinishedTasks();
            }

            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /*Useful method, can be added with action routing
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                _repository.Delete(_repository.GetById(id));
                _repository.Save();
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);

        } */

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