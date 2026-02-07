using Microsoft.EntityFrameworkCore;
using NotionAPI.Context;
using NotionAPI.DTOs.Task;
using NotionAPI.Models;
using NotionAPI.Utilites;

namespace NotionAPI.Services
{
    public interface ITasksServices
    {
        Task<GenericRespones<TodoTasks>> AddTasks(int userId,TaskDto task);
        Task<GenericRespones<bool>> UpdateTask(int taskId, TaskGetDto task);
        Task<GenericRespones<bool>> DeleteTask(int taskId);
        Task<GenericRespones<List<TaskGetDto>>> GetAllTask(int userId);
        Task<GenericRespones<TaskDto>> GetTaskById(int taskId);
        Task<GenericRespones<List<TaskDto>>> GetAllNotCompletedTasks(int userId);
        Task<GenericRespones<bool>> AddTaskDescription(int taskId, DescriptionDto description);
    }

    public class TasksServices : ITasksServices
    {
        private NotionData _context;

        public TasksServices(NotionData context)
        {
            _context = context;
        }

        public async Task<GenericRespones<TodoTasks>> AddTasks(int userId,TaskDto taks)
        {
            try
            {
                var user = await _context.users.FindAsync(userId);

                if (user == null)
                {
                    return new GenericRespones<TodoTasks>("User not found", "NOT FOUND", 404, null, false);
                }


                TodoTasks userTask = new TodoTasks()
                {
                    Title = taks.Title,
                    IsCompleted = taks.IsCompleted,
                    CreateAt = DateTime.UtcNow,
                    UserId = userId
                };

                await _context.Tasks.AddAsync(userTask);
                await _context.SaveChangesAsync();

                return new GenericRespones<TodoTasks>("Task Added successfuly", "SUCCESS", 200, null, true);
            }
            catch (Exception ex)
            {
                return new GenericRespones<TodoTasks>("Internal server error", ex.Message, 500, null, false);
            }
        }

        public async Task<GenericRespones<List<TaskGetDto>>> GetAllTask(int userId)
        {
            try
            {
                Users user = await _context.users.FindAsync(userId);

                if (user == null)
                {
                    return new GenericRespones<List<TaskGetDto>>("User not found", "NOT FOUND", 404, null, false);
                }

                List<TaskGetDto> tasks = await _context.Tasks
                    .AsNoTracking()
                    .Where(task => task.UserId == userId && task.IsCompleted == false)
                    .Select(taskD => new TaskGetDto(
                        taskD.Id,
                        taskD.Title,
                        taskD.TaskDescription.Select( d=> new DescriptionDto(d.Id,d.Description)).ToList(),
                        taskD.IsCompleted))
                    .ToListAsync();

                return new GenericRespones<List<TaskGetDto>>("Tasks retrieved successfully", "OK", 200, tasks, true);
            }
            catch (Exception ex)
            {
                return new GenericRespones<List<TaskGetDto>>("Internal server error", ex.Message, 500, null, false);
            }
        }

        public async Task<GenericRespones<TaskDto>> GetTaskById(int taskId)
        {
            try
            {
                var tasks = await _context.Tasks.FindAsync(taskId);

                if (tasks == null) return new GenericRespones<TaskDto>("Task not found", "NOT FOUND", 404, null, false);

                TaskDto currentTask = new TaskDto(
                    tasks.Id,
                    tasks.Title,
                    tasks.IsCompleted);

                return new GenericRespones<TaskDto>("Task retrieved successfully", "OK", 200, currentTask, true);

            }
            catch (Exception ex)
            {
                return new GenericRespones<TaskDto>("Internal server error", ex.Message, 500, null, false);
            }
        }

        public async Task<GenericRespones<bool>> UpdateTask(int taskId,TaskGetDto task)
        {
            try
            {
                var curTask = await _context.Tasks.FindAsync(taskId);

                if (curTask == null) return new GenericRespones<bool>("Task not found", "NOT EXIST", 404, false, false);

                curTask.Title = task.Title;
                curTask.IsCompleted = task.IsCompleted;
                curTask.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();


                return new GenericRespones<bool>("Updated successfuly", "SUCCESS", 200, true, true);
            }
            catch (Exception ex)
            {
                return new GenericRespones<bool>("Internal server error", ex.Message, 500, false,false);
            }
        }

        public async Task<GenericRespones<bool>> DeleteTask(int taskId)
        {
            try
            {
                var currentTask = await _context.TaskDescription.FindAsync(taskId);

                if (currentTask == null) return new GenericRespones<bool>("Task id not found", "NOT FOUND", 404, false, false);

                _context.TaskDescription.Remove(currentTask);

                await _context.SaveChangesAsync();

                return new GenericRespones<bool>($"Successfuly deleted task = {taskId}", null, 200, true, true);
            }
            catch (Exception ex)
            {
                return new GenericRespones<bool>("Ineternal server error", ex.Message, 500, false, false);
            }
        }

        public async Task<GenericRespones<List<TaskDto>>> GetAllNotCompletedTasks(int userId)
        {
            try
            {
                var notCompletedTasks = await _context.Tasks.AsNoTracking()
                    .Where(task => task.UserId == userId && task.IsCompleted == false && task.CreateAt < DateTime.Now)
                    .ToListAsync();

                if(notCompletedTasks == null)
                {
                    return new GenericRespones<List<TaskDto>>("No Task found", "NOT FOUND", 404, null, false);
                }

                List<TaskDto> taskDto = notCompletedTasks.Select(task => new TaskDto(
                    task.Id,
                    task.Title,
                    task.IsCompleted)).ToList();


                return new GenericRespones<List<TaskDto>>("Tasks retrieved successfully", "OK", 200, taskDto, true);
            }
            catch (Exception ex)
            {
                return new GenericRespones<List<TaskDto>>("Internal server error", "", 500, null, false);
            }
        }

        public async Task<GenericRespones<bool>> AddTaskDescription(int taskId, DescriptionDto description)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);

                if (task == null)
                {
                    return new GenericRespones<bool>("Task not found", "NOT FOUND", 404, false, false);
                }

                await _context.TaskDescription.AddAsync(new TaskDescription
                {
                    TaskTd = taskId,
                    Description = description.Description
                });

                await _context.SaveChangesAsync();

                return new GenericRespones<bool>("Description added successfully", "SUCCESS", 200, true, true);
            }
            catch (Exception ex)
            {
                return new GenericRespones<bool>("Internal server error", null, 500, false,false);
            }
        }
    }
}
