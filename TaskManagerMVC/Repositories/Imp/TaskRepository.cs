using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskManagerAPI.Dtos;
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
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Models.Task>> GetAllWithRelationsAsync()
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Priority)
                    .Include(t => t.Status)
                    .Include(t => t.User)
                    .ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to retrieve tasks with relations.", ex);
            }
        }

        public async Task<Models.Task?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Task ID must be greater than zero.", nameof(id));

            try
            {
                var task = await _context.Tasks
                    .Include(t => t.Status)
                    .Include(t => t.Priority)
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.TaskId == id);

                return task;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Failed to retrieve task with ID {id}.", ex);
            }
        }

        public async System.Threading.Tasks.Task AddAsync(Models.Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            try
            {
                await _context.Tasks.AddAsync(task);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add task.", ex);
            }
        }

        public async System.Threading.Tasks.Task UpdateAsync(Models.Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            try
            {
                _context.Tasks.Update(task);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to update task.", ex);
            }
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Task ID must be greater than zero.", nameof(id));

            try
            {
                var task = await _context.Tasks
                    .Include(t => t.TaskComments)
                    .FirstOrDefaultAsync(t => t.TaskId == id);

                if (task == null)
                    throw new KeyNotFoundException($"Task with ID {id} not found.");

                _context.TaskComments.RemoveRange(task.TaskComments);
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Failed to delete task with ID {id}.", ex);
            }
        }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Concurrency error occurred while saving changes.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to save changes to the database.", ex);
            }
        }

        public async Task<IEnumerable<Models.TaskStatus>> GetAllStatusesAsync()
        {
            try
            {
                return await _context.TaskStatuses.AsNoTracking().ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to retrieve task statuses.", ex);
            }
        }

        public async Task<IEnumerable<TaskPriority>> GetAllPrioritiesAsync()
        {
            try
            {
                return await _context.TaskPriorities.AsNoTracking().ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to retrieve task priorities.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.AsNoTracking().ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to retrieve users.", ex);
            }
        }

        public async Task<List<TaskDto>> GetFilteredTasksAsync(string? title, int? statusId, int? priorityId)
        {
            var taskDtos = new List<TaskDto>();
            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "sp_FilterTasks";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter(SqlConstants.ParamTitle, title ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter(SqlConstants.ParamStatusId, statusId ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter(SqlConstants.ParamPriorityId, priorityId ?? (object)DBNull.Value));

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    taskDtos.Add(new TaskDto
                    {
                        TaskId = reader.GetInt32(reader.GetOrdinal(SqlConstants.ColTaskId)),
                        Title = reader.GetString(reader.GetOrdinal(SqlConstants.ColTitle)),
                        Description = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColDescription))
                            ? null
                            : reader.GetString(reader.GetOrdinal(SqlConstants.ColDescription)),
                        StartDate = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColStartDate))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal(SqlConstants.ColStartDate)).ToString("yyyy-MM-dd HH:mm:ss"),
                        DueDate = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColDueDate))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal(SqlConstants.ColDueDate)).ToString("yyyy-MM-dd HH:mm:ss"),
                        StatusName = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColStatusName))
                            ? null
                            : reader.GetString(reader.GetOrdinal(SqlConstants.ColStatusName)),
                        PriorityName = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColPriorityName))
                            ? null
                            : reader.GetString(reader.GetOrdinal(SqlConstants.ColPriorityName)),
                        UserFullName = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColUserFullName))
                            ? null
                            : reader.GetString(reader.GetOrdinal(SqlConstants.ColUserFullName)),
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Failed to execute stored procedure 'sp_FilterTasks'.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }

            return taskDtos;
        }
    }
}