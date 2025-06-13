using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.DBContext;
using TaskManagerMVC.Models;
using TaskManagerMVC.Repositories.Interfaces;

namespace TaskManagerMVC.Repositories.Imp
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _context;

        public TaskRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Task>> GetAllWithRelationsAsync()
        {
            return await _context.Tasks
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.User)
                .ToListAsync();
        }
        public async Task<Models.Task?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TaskId == id);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Models.Task task)
        {
            _context.Tasks.Update(task);

            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task AddAsync(Models.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.TaskStatus>> GetAllStatusesAsync()
        {
            return await _context.TaskStatuses.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TaskPriority>> GetAllPrioritiesAsync()
        {
            return await _context.TaskPriorities.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
    }

}
