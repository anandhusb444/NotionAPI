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

        [HttpPost("Register")]
        public async Task<IActionResult> AddUser(UserRegister user)
        {
            GenericRespones<Users> isAdded = await _userService.AddUser(user);

            if(!isAdded.Status)
            {
                return BadRequest(isAdded);
            }

            return Ok(isAdded);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(UserLogin user)
        {
            GenericRespones<string> isLoggedIn = await _userService.LoginUser(user);
            if (!isLoggedIn.Status)
            {
                return NotFound(isLoggedIn);
            }

            return Ok(isLoggedIn);
        }


    }
}
