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

        [Authorize]
        [HttpGet("Get")]
        public async Task<IActionResult> GetTasks()
        {
            string userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userClaim == null) return Unauthorized();

            int userId = int.Parse(userClaim);

            var result = await _taskServeice.GetAllTask(userId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            string userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userClaim == null) return Unauthorized();

            var result = await _taskServeice.GetTaskById(taskId);

            if(!result.Status)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPut("Update/id")]
        public async Task<IActionResult> UpdateTask(int taskId, TaskDto taskDto)
        {
            string userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            var result = await _taskServeice.UpdateTask(taskId,taskDto);

            if (result.StatusCode == 404) return NotFound(result);

            if (!result.Status) return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Delete/id")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            var result = await _taskServeice.DeleteTask(taskId);

            if (result.StatusCode == 404) return NotFound(result);

            if (!result.Status) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("NotCompletedTasks")]
        public async Task<IActionResult> GetNotCompletedTasks()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            int userId = int.Parse(userClaim);

            var result = await _taskServeice.GetAllNotCompletedTasks(userId);

            if (result.StatusCode == 404) return NotFound(result);

            if(result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result);

        }

    }
}
