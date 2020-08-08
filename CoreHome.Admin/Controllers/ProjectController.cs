using CoreHome.Admin.Filter;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ProjectController : Controller
    {
        private readonly HomePageService homePageService;

        public ProjectController(HomePageService homePageService)
        {
            this.homePageService = homePageService;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Project";

            return View(homePageService.Config);
        }
    }
}
