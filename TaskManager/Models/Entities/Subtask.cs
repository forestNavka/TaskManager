using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models.Entities
{
    /// <summary>
    /// Describes a subtask
    /// </summary>
    public class Subtask
    {
        /// <summary>
        /// Gets or sets the unique identifier of subtask
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of subtask
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the task, to which  current subtask belongs
        /// </summary>
        [ForeignKey("Task_Id")]
        public Task Task { get; set; }

        /// <summary>
        /// Gets or sets the id of task to which current subtask belongs
        /// </summary>
        public Guid Task_Id { get; set; }

        /// <summary>
        /// Gets or sets the state of subtask (done or not)
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// Gets or sets the descriptive text
        /// </summary>
        public string Text { get; set; }
    }
}