using CoreHome.Admin.Filter;
using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ThemeController(ThemeService themeService, OssService ossService) : Controller
    {
        private readonly ThemeService themeService = themeService;
        private readonly OssService ossService = ossService;

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Theme";
            return View(themeService.Config);
        }

        [HttpPost]
        public IActionResult Index(Theme theme)
        {
            ViewBag.PageTitle = "Theme";

            if (ModelState.IsValid)
            {
                themeService.Config = theme;
            }

            return View(themeService.Config);
        }

        public IActionResult Reset()
        {
            themeService.ResetConfig();
            return Content("Reset Successful");
        }

        [HttpPost]
        public IActionResult UploadBackground(IFormFile file)
        {
            using Stream stream = file.OpenReadStream();
            try
            {
                ossService.UploadBackground(stream);
                return Ok();
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }

    }
}