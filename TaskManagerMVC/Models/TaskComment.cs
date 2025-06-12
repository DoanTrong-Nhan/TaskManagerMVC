using System;

namespace TaskManagerMVC.Models
{
    public  class TaskComment
    {
        private int? _taskId;
        private string? _commentText;
        private DateTime? _commentDate;
        private Task? _task;

        public int CommentId { get; private set; }

        public int? TaskId
        {
            get => _taskId;
            private set => _taskId = value;
        }

        public string? CommentText
        {
            get => _commentText;
            set => _commentText = value;
        }

        public DateTime? CommentDate
        {
            get => _commentDate;
            set => _commentDate = value;
        }

        public Task? Task
        {
            get => _task;
            set => _task = value;
        }

        public TaskComment(string? commentText, DateTime? commentDate = null)
        {
            CommentText = commentText;
            CommentDate = commentDate ?? DateTime.Now;
        }

        private TaskComment() { }
    }
}
