using Microsoft.AspNetCore.Mvc;
using NotionAPI.DTOs.User;
using NotionAPI.Models;
using NotionAPI.Services;
using NotionAPI.Utilites;

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
            GenericRespones<Users> isAdded = await _userService.AddUser(user);

            if(!isAdded.Status)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> LoginUser(int id)
        {
            bool isLoggedIn = await _userService.LoginUser(id);
            if (!isLoggedIn)
            {
                return NotFound();
            }

            return Ok();
        }


    }
}
