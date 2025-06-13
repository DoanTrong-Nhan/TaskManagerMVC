using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagerAPI.Dtos;
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // Hiển thị danh sách Task
        public async Task<IActionResult> ListTask()
        {
            var tasks = await _taskService.GetAllTasksAsync();
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
        public async Task<IActionResult> CreateTask(TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(dto);
            }

            try
            {
                await _taskService.CreateTaskAsync(dto);
                TempData["Success"] = "Task created successfully!";
                return RedirectToAction("ListTask");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                await LoadDropdowns();
                return View(dto);
            }
        }

        // Form cập nhật task
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var dto = await _taskService.GetTaskForUpdateAsync(id);
                return View(dto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Xử lý cập nhật task
        [HttpPost]
        public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _taskService.UpdateTaskAsync(id, dto);
                return RedirectToAction("ListTask");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (FormatException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }


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
