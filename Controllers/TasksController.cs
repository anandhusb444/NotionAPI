using Microsoft.AspNetCore.Mvc;

namespace NotionAPI.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
