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
            Theme theme = themeService.Theme;
            return View(theme);
        }

        [HttpPost]
        public IActionResult Index(Theme theme)
        {
            themeService.Theme = theme;
            return View(theme);
        }

    }
}