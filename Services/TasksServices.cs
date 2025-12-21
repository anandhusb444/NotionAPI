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
    }
}
