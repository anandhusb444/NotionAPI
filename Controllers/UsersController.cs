using Microsoft.AspNetCore.Mvc;

namespace NotionAPI.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
