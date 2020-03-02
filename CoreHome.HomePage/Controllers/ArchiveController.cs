using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Controllers
{
    public class ArchiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}