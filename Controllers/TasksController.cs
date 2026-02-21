using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using NotionAPI.DTOs.Task;
using NotionAPI.Services;
using NotionAPI.Utilites;
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

        [HttpPost("{Id}")]
        public async Task<IActionResult> AddTaskDescription(int Id, DescriptionDto description)
        {
            var result = await _taskServeice.AddTaskDescription(Id, description);

            if (result.StatusCode == 404)
                return NotFound("Task not found");

            if (!result.Status)
                return BadRequest();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Get")]// need to remove the get
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

        [HttpGet("NotCompletedTasks")]
        public async Task<IActionResult> GetNotCompletedTasks()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            int userId = int.Parse(userClaim);

            var result = await _taskServeice.GetAllNotCompletedTasks(userId);

            if (result.StatusCode == 404) return NotFound(result);

            if (result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTaskDescriptio(int id,DescriptionDto description)
        {
            GenericRespones<bool> result = await _taskServeice.UpdateDescription(id, description);

            if(result.StatusCode == 404 ) return NotFound(result);

            if (!result.Status) return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id,TaskGetDto taskDto)
        {
            string userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            var result = await _taskServeice.UpdateTask(id,taskDto);

            if (result.StatusCode == 404) return NotFound(result);

            if (!result.Status) return BadRequest(result);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            var result = await _taskServeice.DeleteTask(id);

            if (result.StatusCode == 404) return NotFound(result);

            if (!result.Status) return BadRequest(result);

            return NoContent();
        }
    }
}
