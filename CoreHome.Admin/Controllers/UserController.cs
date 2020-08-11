using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
