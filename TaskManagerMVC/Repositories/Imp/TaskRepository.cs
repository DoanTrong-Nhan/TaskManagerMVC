using TaskManagerMVC.DBContext;
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

        public async Task AddAsync(Models.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
