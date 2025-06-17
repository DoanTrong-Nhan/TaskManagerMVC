namespace TaskManagerMVC.Services.Interfaces
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TaskManagerMVC.Dtos;
    using TaskManagerMVC.Dto.TaskDto;

    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<TaskUpdateDto> GetTaskForUpdateAsync(int id);
        Task CreateTaskAsync(TaskCreateDto dto);
        Task UpdateTaskAsync(int id, TaskUpdateDto dto);

        Task<IEnumerable<SelectListItem>> GetStatusListAsync();
        Task<IEnumerable<SelectListItem>> GetPriorityListAsync();
        Task<IEnumerable<SelectListItem>> GetUserListAsync();

        Task DeleteTaskAsync(DeleteTaskDto dto);

        /// <summary>
        /// Lọc task theo tiêu đề, trạng thái hoặc độ ưu tiên.
        /// </summary>
        /// <param name="title">Tiêu đề (tuỳ chọn)</param>
        /// <param name="statusId">ID trạng thái (tuỳ chọn)</param>
        /// <param name="priorityId">ID độ ưu tiên (tuỳ chọn)</param>
        /// <returns>Danh sách TaskDto đã lọc</returns>
        Task<List<TaskDto>> GetFilteredTasks(string? title, int? statusId, int? priorityId);
    }

}
