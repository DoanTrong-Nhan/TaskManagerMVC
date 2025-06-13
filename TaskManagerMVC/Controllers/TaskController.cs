using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Dtos;
using TaskManagerMVC.Services;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Trả về form tạo Task
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto); // Trả lại form nếu có lỗi validate

            await _taskService.CreateTaskAsync(dto);

            return RedirectToAction("Index");
        }
    }
}
