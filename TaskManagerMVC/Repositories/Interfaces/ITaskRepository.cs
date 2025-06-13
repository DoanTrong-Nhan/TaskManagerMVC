using TaskManagerMVC.Models;

namespace TaskManagerMVC.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<Models.Task>> GetAllWithRelationsAsync();

        Task<Models.Task?> GetByIdAsync(int id);
        System.Threading.Tasks.Task UpdateAsync(Models.Task task);

        System.Threading.Tasks.Task AddAsync(Models.Task task);
        System.Threading.Tasks.Task SaveChangesAsync();

        Task<IEnumerable<Models.TaskStatus>> GetAllStatusesAsync();
        Task<IEnumerable<TaskPriority>> GetAllPrioritiesAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
    }

}
