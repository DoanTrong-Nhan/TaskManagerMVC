namespace TaskManagerMVC.Services.Interfaces
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TaskManagerAPI.Dtos;

    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<TaskUpdateDto> GetTaskForUpdateAsync(int id);
        Task CreateTaskAsync(TaskCreateDto dto);
        Task UpdateTaskAsync(int id, TaskUpdateDto dto);

        Task<IEnumerable<SelectListItem>> GetStatusListAsync();
        Task<IEnumerable<SelectListItem>> GetPriorityListAsync();
        Task<IEnumerable<SelectListItem>> GetUserListAsync();
    }

}
