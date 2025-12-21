using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using NotionAPI.DTOs.Task;
using NotionAPI.Services;
using System.Security.Claims;

namespace NotionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private ITasksServices _taskServeice;

        public TasksController(ITasksServices taskServices)
        {
            _taskServeice = taskServices;
        }


        [Authorize]
        [HttpPost("task")]
        public async Task<IActionResult> AddTask(TaskDto task)
        {
            string userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userClaim == null) return Unauthorized();

            int userId = int.Parse(userClaim);

            var result = await _taskServeice.AddTasks(userId, task);

            return Ok(result);
        }


    }
}
