using System;
using System.Collections.Generic;

namespace TaskManagerMVC.Models
{
    public class TaskStatus
    {
        private string? _statusName;
        private ICollection<Task> _tasks = new List<Task>();

        public int StatusId { get; private set; }

        public string? StatusName
        {
            get => _statusName;
            set => _statusName = value;
        }

        public virtual ICollection<Task> Tasks
        {
            get => _tasks;
            private set => _tasks = value;
        }

        private TaskStatus() { }

        public TaskStatus(string? statusName)
        {
            StatusName = statusName;
        }
    }
}
