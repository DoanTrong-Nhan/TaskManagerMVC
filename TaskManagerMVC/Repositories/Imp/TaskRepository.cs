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

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.TaskComments)
                .FirstOrDefaultAsync(t => t.TaskId == id);
            if (task != null)
            {
                _context.TaskComments.RemoveRange(task.TaskComments);
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TaskDto>> GetFilteredTasksAsync(string? title, int? statusId, int? priorityId)
        {
            var taskDtos = new List<TaskDto>();

            // Lấy kết nối từ DbContext
            var connection = _context.Database.GetDbConnection();

            try
            {
                // Mở kết nối nếu chưa mở
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_FilterTasks";
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số
                    command.Parameters.Add(new SqlParameter(SqlConstants.ParamTitle, title ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter(SqlConstants.ParamStatusId, statusId ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter(SqlConstants.ParamPriorityId, priorityId ?? (object)DBNull.Value));

                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dto = new TaskDto
                            {
                                TaskId = reader.GetInt32(reader.GetOrdinal(SqlConstants.ColTaskId)),
                                Title = reader.GetString(reader.GetOrdinal(SqlConstants.ColTitle)),
                                Description = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColDescription)) ? null : reader.GetString(reader.GetOrdinal(SqlConstants.ColDescription)),

                                StartDate = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColStartDate)) ? null : reader.GetDateTime(reader.GetOrdinal(SqlConstants.ColStartDate)).ToString("yyyy-MM-dd HH:mm:ss"),
                                DueDate = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColDueDate)) ? null : reader.GetDateTime(reader.GetOrdinal(SqlConstants.ColDueDate)).ToString("yyyy-MM-dd HH:mm:ss"),

                                StatusName = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColStatusName)) ? null : reader.GetString(reader.GetOrdinal(SqlConstants.ColStatusName)),
                                PriorityName = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColPriorityName)) ? null : reader.GetString(reader.GetOrdinal(SqlConstants.ColPriorityName)),
                                UserFullName = reader.IsDBNull(reader.GetOrdinal(SqlConstants.ColUserFullName)) ? null : reader.GetString(reader.GetOrdinal(SqlConstants.ColUserFullName)),
                            };

                            taskDtos.Add(dto);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Lỗi khi thực thi stored procedure 'sp_FilterTasks'.", ex);
            }
            finally
            {
                // Đóng kết nối nếu cần
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }

            return taskDtos;
        }

    }

}
