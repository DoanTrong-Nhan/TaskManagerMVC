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
            var tasks = await _taskService.GetAllTasksAsync();

            var canCreate = await _authService.HasPermissionAsync(User,
                            PermissionConstants.CreateTaskMethod,
                            PermissionConstants.CreateTaskEndpoint);

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

            var canUpdate = await _authService.HasPermissionAsync(User,
                      PermissionConstants.UpdateTaskMethod,
                      PermissionConstants.UpdateTaskEndpoint);

            var canDelete = await _authService.HasPermissionAsync(User,
                                PermissionConstants.DeleteTaskMethod,
                                PermissionConstants.DeleteTaskEndpoint);

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
            if (!ModelState.IsValid)
            {
                ViewBag.TaskId = id;


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
            var dto = new DeleteTaskDto { TaskId = id };
            var canDelete = await _authService.HasPermissionAsync(User,
                      PermissionConstants.DeleteTaskMethod,
                      PermissionConstants.DeleteTaskEndpoint);
            ViewBag.CanDelete = canDelete;

            await _taskService.DeleteTaskAsync(dto);
            return RedirectToAction(nameof(ListTask));
        }

        //Action to Search
        [HttpGet]
        public async Task<IActionResult> Search(string? title, int? statusId, int? priorityId)
        {
            var tasks = await _taskService.GetFilteredTasks(title, statusId, priorityId);

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