using System;
using System.Collections.Generic;
using TaskStatus = TaskManagerMVC.Models.TaskStatus;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Models
{
    public class Task
    {
        private string _title;
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

        public int? StatusId => _statusId;
        public int? PriorityId => _priorityId;
        public int? UserId => _userId;

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

        public virtual ICollection<TaskComment> TaskComments => _taskComments;

        public Task(string title)
        {
            Title = title;
        }

        private Task() { }

        public void SetForeignKeys(int statusId, int priorityId, int userId)
        {
            _statusId = statusId;
            _priorityId = priorityId;
            _userId = userId;
        }

        public void Update(string title, string? description, DateTime? startDate, DateTime? dueDate,
                           int statusId, int priorityId, int userId)
        {
            Title = title;
            Description = description;
            StartDate = startDate;
            DueDate = dueDate;
            SetForeignKeys(statusId, priorityId, userId);
        }

        public bool IsOverdue =>
            DueDate.HasValue && DueDate.Value < DateTime.Today && Status?.StatusName != "Completed";
    }

}
