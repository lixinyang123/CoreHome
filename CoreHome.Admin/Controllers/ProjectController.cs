using CoreHome.Admin.Filter;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.PageTitle = "Project";

            return View();
        }
    }
}
