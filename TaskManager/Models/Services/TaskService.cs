using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Models.DAL;
using TaskManager.Models.Entities;

namespace TaskManager.Models.Services
{
    public class TaskService : Service<Task>
    {
        public TaskService(IRepository<Task> repository) : base(repository) { }

        public IEnumerable<Task> GetByQuery(string query)
        {
            return _repository.GetByQuery(string.Format("Select * from Tasks where {0}", query));
        }

        public void AddTask(Task task)
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
                throw;
            }
        }

        public void UpdateTask(Guid id, bool isDone)
        {
            try
            {
                Task task = _repository.GetById(id);
                task.IsDone = isDone;
                _repository.Update(task);
                _repository.Save();
            }

            catch
            {
                throw;
            }
        }

        public void DeleteFinishedTasks()
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
                throw;
            }
        }

    }
}