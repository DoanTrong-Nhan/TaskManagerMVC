﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TaskManagerMVC.Dtos;
using TaskManagerMVC.Dto.TaskDto;
using TaskManagerMVC.Helper;
using TaskManagerMVC.Models;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Services.Imp
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<List<TaskDto>> GetAllTasksAsync(int? userId = null)
        {
            var tasks = await _taskRepository.GetAllWithRelationsAsync();

            // Filter tasks by userId if provided (for Role ID = 2)
            if (userId.HasValue)
            {
                tasks = tasks.Where(t => t.UserId == userId.Value).ToList();
            }

            return tasks.Select(t => MapToTaskDto(t)).ToList();
        }

        public async System.Threading.Tasks.Task CreateTaskAsync(TaskCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var task = new Models.Task(dto.Title)
            {
                Description = dto.Description,
                StartDate = DateHelper.ParseExactOrNull(dto.StartDate),
                DueDate = DateHelper.ParseExactOrNull(dto.DueDate)
            };

            task.SetForeignKeys(dto.StatusId, dto.PriorityId, dto.UserId);

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();
        }

        public async Task<TaskUpdateDto> GetTaskForUpdateAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Task ID must be greater than zero.", nameof(id));

            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            return new TaskUpdateDto
            {
                Title = task.Title,
                Description = task.Description,
                StartDate = DateHelper.ToDisplayDate(task.StartDate),
                DueDate = DateHelper.ToDisplayDate(task.DueDate),
                StatusId = task.StatusId ?? 0,
                PriorityId = task.PriorityId ?? 0,
                UserId = task.UserId ?? 0
            };
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(int id, TaskUpdateDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("Task ID must be greater than zero.", nameof(id));
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            task.Update(
                dto.Title,
                dto.Description,
                DateHelper.ParseExactOrNull(dto.StartDate),
                DateHelper.ParseExactOrNull(dto.DueDate),
                dto.StatusId,
                dto.PriorityId,
                dto.UserId
            );

            await _taskRepository.UpdateAsync(task);
            await _taskRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(DeleteTaskDto dto)
        {
            if (dto == null || dto.TaskId <= 0)
                throw new ArgumentException("Invalid task ID.", nameof(dto));

            await _taskRepository.DeleteAsync(dto.TaskId);
        }

        public async Task<IEnumerable<SelectListItem>> GetStatusListAsync()
        {
            var statuses = await _taskRepository.GetAllStatusesAsync();
            return statuses.Select(s => new SelectListItem
            {
                Value = s.StatusId.ToString(),
                Text = s.StatusName
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetPriorityListAsync()
        {
            var priorities = await _taskRepository.GetAllPrioritiesAsync();
            return priorities.Select(p => new SelectListItem
            {
                Value = p.PriorityId.ToString(),
                Text = p.PriorityName
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetUserListAsync()
        {
            var users = await _taskRepository.GetAllUsersAsync();
            return users.Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.FullName ?? u.Username
            });
        }
        public async Task<List<TaskDto>> GetFilteredTasks(string? title, int? statusId, int? priorityId, int? userId = null)
        {
            return await _taskRepository.GetFilteredTasksAsync(title, statusId, priorityId, userId);
        }



        private static TaskDto MapToTaskDto(Models.Task task)
        {
            return new TaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                StartDate = DateHelper.ToDisplayDate(task.StartDate),
                DueDate = DateHelper.ToDisplayDate(task.DueDate),
                PriorityName = task.Priority?.PriorityName,
                StatusName = task.Status?.StatusName,
                UserFullName = task.User?.FullName,
                IsOverdue = task.IsOverdue
            };
        }
    }
}