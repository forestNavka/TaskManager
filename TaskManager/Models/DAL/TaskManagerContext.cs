using System.Data.Entity;

namespace TaskManager.Models.DAL
{
    /// <summary>
    /// Describes the TaskManager DbContext
    /// </summary>
    public class TaskManagerContext : DbContext
    {
        /// <summary>
        /// Configures task manager context
        /// </summary>
        public TaskManagerContext()
        {
            Database.SetInitializer<TaskManagerContext>(new TaskManagerContextInitializer());
        }

        /// <summary>
        /// Gets or sets the dbset of categories
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the dbset of tasks
        /// </summary>
        public DbSet<Task> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the dbset of subtasks
        /// </summary>
        public DbSet<Subtask> Subtasks { get; set; }
    }
}