using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
