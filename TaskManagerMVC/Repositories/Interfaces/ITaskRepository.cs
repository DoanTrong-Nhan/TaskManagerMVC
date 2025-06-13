namespace TaskManagerMVC.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<Models.Task>> GetAllWithRelationsAsync();

        Task AddAsync(Models.Task task);
        Task SaveChangesAsync();
    }

}
