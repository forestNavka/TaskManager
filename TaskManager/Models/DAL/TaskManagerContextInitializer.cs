using System;
using System.Data.Entity;
using TaskManager.Models.Entities;

namespace TaskManager.Models.DAL
{
    public class TaskManagerContextInitializer : DropCreateDatabaseAlways<TaskManagerContext>
    {
        protected override void Seed(TaskManagerContext context)
        {
            Guid categoryId = Guid.NewGuid();
            Guid taskId = Guid.NewGuid();
            context.Categories.Add(new Category() {Id = categoryId, Name = "Новий проект", Text = "Завдання до нового проекту"});
            context.Tasks.Add(new Task() {Id = taskId, Category_Id = categoryId, Name = "Рівень доступу до бази даних",
               Text = "Створити рівень доступу до бази даних", Date = DateTime.Now });
            context.Subtasks.Add(new Subtask() {Id = Guid.NewGuid(), Name = "Описати моделі сутностей",
                Task_Id = taskId, Text = "Вивчити предметну область та розробити моделі відповідних сутностей" });
            context.SaveChanges();
            base.Seed(context);
        }
    }
}