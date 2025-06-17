using TaskManagerMVC.Dtos;
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
        System.Threading.Tasks.Task DeleteAsync(int id);

        /// Lọc danh sách task theo tiêu đề, trạng thái, và độ ưu tiên.
        Task<List<TaskDto>> GetFilteredTasksAsync(string? title, int? statusId, int? priorityId);
    }

}
