using System;
using System.Collections.Generic;

namespace TaskManagerMVC.Models
{
    public class Task
    {
        private string _title = null!;
        private string? _description;
        private DateTime? _startDate;
        private DateTime? _dueDate;
        private int? _statusId;
        private int? _priorityId;
        private int? _userId;

        private TaskPriority? _priority;
        private TaskStatus? _status;
        private User? _user;
        private ICollection<TaskComment> _taskComments = new List<TaskComment>();

        public int TaskId { get; private set; }

        public string Title
        {
            get => _title;
            set => _title = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Title cannot be null or empty.");
        }

        public string? Description
        {
            get => _description;
            set => _description = value;
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }

        public DateTime? DueDate
        {
            get => _dueDate;
            set => _dueDate = value;
        }

        public int? StatusId
        {
            get => _statusId;
            private set => _statusId = value;
        }

        public int? PriorityId
        {
            get => _priorityId;
            private set => _priorityId = value;
        }

        public int? UserId
        {
            get => _userId;
            private set => _userId = value;
        }

        public TaskPriority? Priority
        {
            get => _priority;
            set => _priority = value;
        }

        public TaskStatus? Status
        {
            get => _status;
            set => _status = value;
        }

        public User? User
        {
            get => _user;
            set => _user = value;
        }

        public virtual ICollection<TaskComment> TaskComments
        {
            get => _taskComments;
            private set => _taskComments = value;
        }

       
        public Task(string title)
        {
            Title = title;
        }

        private Task() { }
    }
}
