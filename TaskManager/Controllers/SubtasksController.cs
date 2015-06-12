using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TaskManager.Models;
using TaskManager.Models.DAL;
using System.Net.Http;
using System.Net;

namespace TaskManager.Controllers
{
    /// <summary>
    /// Provides handling of requests to subtasks
    /// </summary>
    public class SubtasksController : ApiController
    {
        private Repository<Subtask> _repository = new Repository<Subtask>();
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
               return _repository.GetAll().ToList();
            return _repository.GetByQuery(string.Format("Select * from Subtasks where {0}", query));
        }

        /// <summary>
        /// Gets subtask by id
        /// </summary>
        /// <param name="id">Id of subtask</param>
        /// <returns>Subtask, which has current id</returns>
        [HttpGet]
        public Subtask Get(Guid id)
        {
                return _repository.GetById(id);
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
            subtask.Id = Guid.NewGuid();
            try
            {
                _repository.Add(subtask);
                _repository.Save();
                using (Repository<Task> taskRepository = new Repository<Task>())
                {
                    Task task = taskRepository.GetById(subtask.Task_Id);
                    task.IsDone = false;
                    taskRepository.Update(task);
                    taskRepository.Save();
                }
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
            Subtask subtask = _repository.GetById(id);
            subtask.IsDone = isDone;
              try
                { 
                    _repository.Update(subtask);
                    _repository.Save();
                    using (Repository<Task> taskRepository = new Repository<Task>())
                    {
                        Task task = taskRepository.GetById(subtask.Task_Id);
                        if (task.IsDone == true && subtask.IsDone == false)
                            task.IsDone = false;
                        if (subtask.IsDone == true && task.IsDone == false)
                        {
                            bool isChanged = true;
                            task.Subtasks = _repository.GetByQuery(string.Format("Select * from Subtasks where Task_Id='{0}'", task.Id));
                            foreach (Subtask subt in task.Subtasks)
                            {
                                if (subt.IsDone == false)
                                {
                                    isChanged = false;
                                    break;
                                }
                            }

                            if (isChanged)
                                task.IsDone = true;    
                        }

                        taskRepository.Update(task);
                        taskRepository.Save();
                    }
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
                _repository.Delete(_repository.GetById(id));
                _repository.Save();
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
            _repository.Dispose();
            base.Dispose(disposing);
            _disposed = true;
        }

        #endregion Disposing
    }
}