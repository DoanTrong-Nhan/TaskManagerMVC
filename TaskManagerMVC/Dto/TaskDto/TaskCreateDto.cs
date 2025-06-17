using System.ComponentModel.DataAnnotations;
using TaskManagerMVC.Helper;

namespace TaskManagerMVC.Dtos
{
    [DateRangeValidation("StartDate", "DueDate", ErrorMessage = "StartDate must be earlier than or equal to DueDate.")]
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [ValidDateFormat("dd/MM/yyyy", ErrorMessage = "StartDate must be in format dd/MM/yyyy.")]
        public string? StartDate { get; set; }

        [ValidDateFormat("dd/MM/yyyy", ErrorMessage = "DueDate must be in format dd/MM/yyyy.")]
        public string? DueDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PriorityId must be a positive number.")]
        public int PriorityId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StatusId must be a positive number.")]
        public int StatusId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive number.")]
        public int UserId { get; set; }
    }

}
