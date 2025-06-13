namespace TaskManagerMVC.Services.Interfaces
{
    using TaskManagerAPI.Dtos;

    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task CreateTaskAsync(TaskCreateDto dto);
    }

}
