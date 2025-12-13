using Microsoft.AspNetCore.Mvc;
using NotionAPI.DTOs.User;
using NotionAPI.Models;
using NotionAPI.Services;

namespace NotionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public IuserService _userService { get; set; }

        public UsersController(IuserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDto user)
        {
            bool isAdded = await _userService.AddUser(user);

            if(!isAdded)
            {
                return BadRequest();
            }

            return Ok();

        }


    }
}
