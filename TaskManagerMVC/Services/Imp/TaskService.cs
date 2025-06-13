using System.Globalization;
using TaskManagerAPI.Dtos;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Services.Imp
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllWithRelationsAsync();

            return tasks.Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate?.ToString("dd/MM/yyyy"),
                DueDate = t.DueDate?.ToString("dd/MM/yyyy"),
                PriorityName = t.Priority?.PriorityName,
                StatusName = t.Status?.StatusName,
                UserFullName = t.User?.FullName
            }).ToList();
        }


        public async Task CreateTaskAsync(TaskCreateDto dto)
        {
            // Chuyển đổi string date sang DateTime?
            DateTime? startDate = ParseDate(dto.StartDate);
            DateTime? dueDate = ParseDate(dto.DueDate);

            var task = new Models.Task(dto.Title)
            {
                Description = dto.Description,
                StartDate = startDate,
                DueDate = dueDate
            };

            // Gán các khóa ngoại
            typeof(Models.Task).GetProperty("StatusId")?.SetValue(task, dto.StatusId);
            typeof(Models.Task).GetProperty("PriorityId")?.SetValue(task, dto.PriorityId);
            typeof(Models.Task).GetProperty("UserId")?.SetValue(task, dto.UserId);

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();
        }

        private DateTime? ParseDate(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return null;
            return DateTime.ParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
