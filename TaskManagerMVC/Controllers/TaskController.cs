using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagerMVC.Dtos;
using TaskManagerMVC.Dto.TaskDto;
using TaskManagerMVC.Helper;
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;

        public TaskController(ITaskService taskService, IAuthService authService)
        {
            _taskService = taskService;
            _authService = authService;
        }

        public async Task<IActionResult> ListTask()
        {
            var userId = _authService.GetCurrentUserId(User);
            var userRoleId = await _authService.GetUserRoleIdAsync(User);

            List<TaskDto> tasks;
            if (userRoleId != 1)
            {
                tasks = await _taskService.GetAllTasksAsync(userId);
                ViewBag.IsUserRole2 = true;
            }
            else
            {
                tasks = await _taskService.GetAllTasksAsync();
                ViewBag.IsUserRole2 = false;
            }

            var canCreate = await _authService.HasPermissionAsync(User,
                PermissionConstants.POST_METHOD,
                PermissionConstants.CREATE_TASK_ENDPOINT);

            ViewBag.CanCreate = canCreate;
            await LoadDropdowns();
            return View(tasks);
        }

        // Form tạo mới
        [HttpGet]
        public async Task<IActionResult> CreateTask()
        {
            await LoadDropdowns();
            return View();
        }

        // Xử lý tạo mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(dto);
            }

            await _taskService.CreateTaskAsync(dto);
            return RedirectToAction(nameof(ListTask));
        }

        // Form cập nhật task
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dto = await _taskService.GetTaskForUpdateAsync(id);

            var canUpdateGet = await _authService.HasPermissionAsync(User,
                PermissionConstants.GET_METHOD,
                PermissionConstants.UPDATE_TASK_ENDPOINT);
            var canUpdatePost = await _authService.HasPermissionAsync(User, "POST",
                PermissionConstants.UPDATE_TASK_ENDPOINT);
            var canUpdate = canUpdateGet && canUpdatePost;

            var canDelete = await _authService.HasPermissionAsync(User,
                PermissionConstants.POST_METHOD,
                PermissionConstants.DELETE_TASK_ENDPOINT);

            ViewBag.CanUpdate = canUpdate;
            ViewBag.CanDelete = canDelete;
            ViewBag.TaskId = id;

            await LoadDropdowns();
            return View(dto);
        }

        // Xử lý cập nhật task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
        {
            var hasPermission = await _authService.HasPermissionAsync(User, "POST",
                PermissionConstants.UPDATE_TASK_ENDPOINT);
            if (!hasPermission)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TaskId = id;
                ViewBag.CanUpdate = true;
                await LoadDropdowns();
                return View(dto);
            }

            await _taskService.UpdateTaskAsync(id, dto);
            return RedirectToAction(nameof(ListTask));
        }

        // Action để xóa Task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var canDelete = await _authService.HasPermissionAsync(User,
                PermissionConstants.POST_METHOD,
                PermissionConstants.DELETE_TASK_ENDPOINT);

            if (!canDelete)
            {
                return NotFound();
            }

            var dto = new DeleteTaskDto { TaskId = id };
            await _taskService.DeleteTaskAsync(dto);
            return RedirectToAction(nameof(ListTask));
        }

        // Action to Search
        [HttpGet]
        public async Task<IActionResult> Search(string? title, int? statusId, int? priorityId)
        {
            // Get the current user's ID and role
            var userId = _authService.GetCurrentUserId(User);
            var userRoleId = await _authService.GetUserRoleIdAsync(User);

            List<TaskDto> tasks;
            if (userRoleId != 1)
            {
                // For Role ID != 1, only show tasks assigned to the current user
                tasks = await _taskService.GetFilteredTasks(title, statusId, priorityId, userId);
                ViewBag.IsUserRole2 = true; // Có thể giữ để hiển thị thông báo cho Role != 1
            }
            else
            {
                // For Role ID = 1, show all matching tasks
                tasks = await _taskService.GetFilteredTasks(title, statusId, priorityId);
                ViewBag.IsUserRole2 = false;
            }

            ViewBag.SelectedTitle = title;
            ViewBag.SelectedStatusId = statusId;
            ViewBag.SelectedPriorityId = priorityId;

            await LoadDropdowns();
            return View("ListTask", tasks);
        }

        // Load dropdowns for status, priority, and user
        private async Task LoadDropdowns()
        {
            var statuses = await _taskService.GetStatusListAsync();
            var priorities = await _taskService.GetPriorityListAsync();
            var users = await _taskService.GetUserListAsync();

            ViewBag.StatusId = new SelectList(statuses, "Value", "Text");
            ViewBag.PriorityId = new SelectList(priorities, "Value", "Text");
            ViewBag.UserId = new SelectList(users, "Value", "Text");
        }
    }
}