using System;
using System.ComponentModel.DataAnnotations;
using TaskManagerMVC.Helper;

namespace TaskManagerMVC.Dtos
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? StartDate { get; set; }
        public string? DueDate { get; set; }

        public string? PriorityName { get; set; }
        public string? StatusName { get; set; }
        public string? UserFullName { get; set; }
        public bool IsOverdue { get; set; }
    }
}
