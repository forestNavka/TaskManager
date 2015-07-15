using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Models.Entities;
using TaskManager.Models.DAL;

namespace TaskManager.Models.Services
{
    public class SubtaskService : Service<Subtask>
    {
        public SubtaskService(IRepository<Subtask> repository) : base(repository) { }

        public IEnumerable<Subtask> GetByQuery(string query)
        {
            return _repository.GetByQuery(string.Format("Select * from Subtasks where {0}", query));
        }

        public void AddSubtask(Subtask subtask, IRepository<Task> taskRepository)
        {
            subtask.Id = Guid.NewGuid();
            try
            {
                _repository.Add(subtask);
                _repository.Save();
                using (taskRepository)
                {
                    Task task = taskRepository.GetById(subtask.Task_Id);
                    task.IsDone = false;
                    taskRepository.Update(task);
                    taskRepository.Save();
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateSubtask(Guid id, bool isDone, IRepository<Task> taskRepository)
        {
            try
            {
                Subtask subtask = _repository.GetById(id);
                subtask.IsDone = isDone;
                _repository.Update(subtask);
                _repository.Save();
                using (taskRepository)
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
                throw;
            }
        }

        public void DeleteSubtask(Guid id)
        {
            try
            {
                Subtask subtask = _repository.GetById(id);
                if (subtask == null)
                    throw new NullReferenceException();
                _repository.Delete(subtask);
                _repository.Save();
            }
            catch
            {
                throw;
            }
        }
    }
}