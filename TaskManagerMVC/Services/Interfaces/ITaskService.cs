namespace TaskManagerMVC.Services.Interfaces
{
    using TaskManagerAPI.Dtos;

    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<TaskUpdateDto> GetTaskForUpdateAsync(int id);
        Task CreateTaskAsync(TaskCreateDto dto);
        Task UpdateTaskAsync(int id, TaskUpdateDto dto);
    }

}
