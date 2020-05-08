using CoreHome.Admin.Filter;
using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ThemeController : Controller
    {
        private readonly ThemeService themeService;

        public ThemeController(ThemeService themeService)
        {
            this.themeService = themeService;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Theme";

            return View(themeService.Theme);
        }

        [HttpPost]
        public IActionResult Index(Theme theme)
        {
            ViewBag.PageTitle = "Theme";

            if (ModelState.IsValid)
            {
                themeService.Theme = theme;
            }
            return View(themeService.Theme);
        }

    }
}