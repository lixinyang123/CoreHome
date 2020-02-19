using Microsoft.AspNetCore.Mvc;

namespace coreHome.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}