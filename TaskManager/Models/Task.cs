using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    /// <summary>
    /// Describes single task
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Gets or sets the unique identifier of task
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of task
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets category, to which task belongs
        /// </summary>
        [ForeignKey("Category_Id")]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets Id of task's category
        /// </summary>
        public Guid Category_Id { get; set; }

        /// <summary>
        /// Gets or sets date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets descriptive text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the state of the task (done or not)
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// Gets or sets a set of subtasks, which belong to current task
        /// </summary>
        public IEnumerable<Subtask> Subtasks { get; set; }
    }
}