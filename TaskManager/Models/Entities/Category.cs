using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Models.Entities
{
    /// <summary>
    /// Represents a category of tasks in manager
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets unique identifier of category
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description of category
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the set of tasks of category
        /// </summary>
        public IEnumerable<Task> Tasks { get; set; }
    }
}