namespace TaskManagerMVC.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task AddAsync(Models.Task task);
        Task SaveChangesAsync();
    }

}
