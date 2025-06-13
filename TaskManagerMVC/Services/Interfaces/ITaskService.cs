namespace TaskManagerMVC.Services.Interfaces
{
    using TaskManagerAPI.Dtos;

    public interface ITaskService
    {
        Task CreateTaskAsync(TaskCreateDto dto);
    }

}
