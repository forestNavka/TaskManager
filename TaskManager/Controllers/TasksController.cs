using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TaskManager.Models.DAL;
using TaskManager.Models;
using System.Net.Http;
using System.Net;

namespace TaskManager.Controllers
{
    /// <summary>
    /// Provides handling of requests to tasks
    /// </summary>
    public class TasksController : ApiController
    {
        private Repository<Task> _repository = new Repository<Task>();
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
                return _repository.GetAll().ToList();
            return _repository.GetByQuery(string.Format("Select * from Tasks where {0}", query)).ToList();
        }

        /// <summary>
        /// Gets task by id
        /// </summary>
        /// <param name="id">Id of task</param>
        /// <returns>Task, that has current id</returns>
        [HttpGet]
        public Task Get(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Adds new task
        /// </summary>
        /// <param name="task">Task for adding</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Task task)
        {
            task.Id = Guid.NewGuid();
            task.Date = DateTime.Now;
            try
            {
                _repository.Add(task);
                _repository.Save();
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
            Task task = _repository.GetById(id);
            task.IsDone = isDone;
                try
                {
                    _repository.Update(task);
                    _repository.Save();
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
                IEnumerable<Task> tasks = _repository.GetAll();
                foreach (Task task in tasks)
                {
                    if (task.IsDone)
                        _repository.Delete(task);
                }

                _repository.Save();
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
            _repository.Dispose();
            base.Dispose(disposing);
            _disposed = true;
        }

        #endregion Disposing
    }
}