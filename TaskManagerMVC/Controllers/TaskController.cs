﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagerAPI.Dtos;
using TaskManagerMVC.Dto.TaskDto;
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
            return RedirectToAction(nameof(Index));
        }

        // Form cập nhật task
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dto = await _taskService.GetTaskForUpdateAsync(id);
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
            return RedirectToAction(nameof(Index));
        }
        // Action để xóa Task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = new DeleteTaskDto { TaskId = id };
            await _taskService.DeleteTaskAsync(dto);
            return RedirectToAction(nameof(Index));
        }

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