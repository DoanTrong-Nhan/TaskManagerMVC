using Microsoft.AspNetCore.Mvc;
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
        public IActionResult CreateTask()
        {
            return View();
        }

        // Xử lý tạo mới
        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto); // Trả lại form nếu có lỗi validate

            await _taskService.CreateTaskAsync(dto);

            return RedirectToAction("ListTask");
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
    }
}
