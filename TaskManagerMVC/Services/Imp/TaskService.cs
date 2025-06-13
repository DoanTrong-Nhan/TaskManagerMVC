using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Validate;
using TaskManagerMVC.Models;
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
                StartDate = DateHelper.ToDisplayDate(t.StartDate),
                DueDate = DateHelper.ToDisplayDate(t.DueDate),
                PriorityName = t.Priority?.PriorityName,
                StatusName = t.Status?.StatusName,
                UserFullName = t.User?.FullName
            }).ToList();
        }

        public async System.Threading.Tasks.Task CreateTaskAsync(TaskCreateDto dto)
        {
            var startDate = DateHelper.ParseExactOrNull(dto.StartDate);
            var dueDate = DateHelper.ParseExactOrNull(dto.DueDate);

            var task = new Models.Task(dto.Title)
            {
                Description = dto.Description,
                StartDate = startDate,
                DueDate = dueDate
            };

            task.SetForeignKeys(dto.StatusId, dto.PriorityId, dto.UserId);

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();
        }

            public async Task<TaskUpdateDto> GetTaskForUpdateAsync(int id)
            {
                var task = await _taskRepository.GetByIdAsync(id);
                if (task == null) throw new KeyNotFoundException();

                return new TaskUpdateDto
                {
                    Title = task.Title,
                    Description = task.Description,
                    StartDate = DateHelper.ToDisplayDate(task.StartDate),
                    DueDate = DateHelper.ToDisplayDate(task.DueDate),
                    StatusId = task.StatusId ?? 0,
                    PriorityId = task.PriorityId ?? 0,
                    UserId = task.UserId ?? 0
                };
            }

        public async System.Threading.Tasks.Task UpdateTaskAsync(int id, TaskUpdateDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            var startDate = DateHelper.ParseExactOrNull(dto.StartDate);
            var dueDate = DateHelper.ParseExactOrNull(dto.DueDate);

            task.Update(dto.Title, dto.Description, startDate, dueDate,
                        dto.StatusId, dto.PriorityId, dto.UserId);

            await _taskRepository.UpdateAsync(task);
            await _taskRepository.SaveChangesAsync();
        }


        public async Task<IEnumerable<SelectListItem>> GetStatusListAsync()
        {
            var statuses = await _taskRepository.GetAllStatusesAsync();
            return statuses.Select(s => new SelectListItem
            {
                Value = s.StatusId.ToString(),
                Text = s.StatusName
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetPriorityListAsync()
        {
            var priorities = await _taskRepository.GetAllPrioritiesAsync();
            return priorities.Select(p => new SelectListItem
            {
                Value = p.PriorityId.ToString(),
                Text = p.PriorityName
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetUserListAsync()
        {
            var users = await _taskRepository.GetAllUsersAsync();
            return users.Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.FullName ?? u.Username
            });
        }

    }
}
