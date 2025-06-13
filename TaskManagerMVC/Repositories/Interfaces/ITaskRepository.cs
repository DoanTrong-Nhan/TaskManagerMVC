namespace TaskManagerMVC.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<Models.Task>> GetAllWithRelationsAsync();

        Task<Models.Task?> GetByIdAsync(int id);
        Task UpdateAsync(Models.Task task);

        Task AddAsync(Models.Task task);
        Task SaveChangesAsync();
    }

}
