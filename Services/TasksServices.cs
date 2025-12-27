using Microsoft.EntityFrameworkCore;
using NotionAPI.Context;
using NotionAPI.DTOs.Task;
using NotionAPI.Models;
using NotionAPI.Utilites;

namespace NotionAPI.Services
{
    public interface ITasksServices
    {
        //Add
        Task<GenericRespones<TodoTasks>> AddTasks(int userId,TaskDto task);
        //update
        //delete
        //get
        Task<GenericRespones<List<TaskDto>>> GetAllTask(int userId);
        Task<GenericRespones<TaskDto>> GetTaskById(int taskId);
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
                    Description = taks.Description,
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

        public async Task<GenericRespones<List<TaskDto>>> GetAllTask(int userId)
        {

            try
            {
                Users user = await _context.users.FindAsync(userId);

                if (user == null)
                {
                    return new GenericRespones<List<TaskDto>>("User not found", "NOT FOUND", 404, null, false);
                }

                List<TaskDto> tasks = await _context.Tasks
                    .Where(task => task.UserId == userId && task.IsCompleted == false)
                    .Select(task => new TaskDto(
                        task.Id,
                        task.Title,
                        task.Description,
                        task.IsCompleted))
                    .ToListAsync();

                return new GenericRespones<List<TaskDto>>("Tasks retrieved successfully", "OK", 200, tasks, true);
            }
            catch (Exception ex)
            {
                return new GenericRespones<List<TaskDto>>("Internal server error", ex.Message, 500, null, false);
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
                    tasks.Description,
                    tasks.IsCompleted);

                return new GenericRespones<TaskDto>("Task retrieved successfully", "OK", 200, currentTask, true);

            }
            catch (Exception ex)
            {
                return new GenericRespones<TaskDto>("Internal server error", ex.Message, 500, null, false);
            }
        }
    }
}
