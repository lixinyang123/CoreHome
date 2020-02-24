using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}