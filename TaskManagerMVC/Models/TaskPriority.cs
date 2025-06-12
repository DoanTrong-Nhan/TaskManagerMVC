using System;
using System.Collections.Generic;

namespace TaskManagerMVC.Models
{
    public class TaskPriority
    {
        private string? _priorityName;
        private ICollection<Task> _tasks = new List<Task>();

        public int PriorityId { get; private set; }

        public string? PriorityName
        {
            get => _priorityName;
            set => _priorityName = value;
        }

        public virtual ICollection<Task> Tasks
        {
            get => _tasks;
            private set => _tasks = value;
        }

        private TaskPriority() { }

        public TaskPriority(string? priorityName)
        {
            PriorityName = priorityName;
        }
    }
}
