using System;
using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Validate;

namespace TaskManagerAPI.Dtos
{
    public class TaskDto
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự.")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? Description { get; set; }

        [ValidDateFormat("dd/MM/yyyy", ErrorMessage = "StartDate must be in format dd/MM/yyyy.")]
        public string? StartDate { get; set; }

        [ValidDateFormat("dd/MM/yyyy", ErrorMessage = "DueDate must be in format dd/MM/yyyy.")]
        public string? DueDate { get; set; }

        public string? PriorityName { get; set; }
        public string? StatusName { get; set; }
        public string? UserFullName { get; set; }
    }
}
