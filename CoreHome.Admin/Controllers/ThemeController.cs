using CoreHome.Admin.Filter;
using Infrastructure.common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ThemeController : Controller
    {
        public IActionResult Index()
        {
            Theme theme = ThemeManager.Theme;
            return View(theme);
        }

        [HttpPost]
        public IActionResult ChangeTheme(int themeType, int backgroundType)
        {
            Theme theme = new Theme()
            {
                ThemeType = (ThemeType)themeType,
                BackgroundType = (BackgroundType)backgroundType
            };

            if (theme.BackgroundType == BackgroundType.Image)
            {
                IFormFile file = Request.Form.Files["background"];
                using Stream stream = file.OpenReadStream();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                System.IO.File.WriteAllBytes(ThemeManager.backgroundUrl, buffer);
            }

            ThemeManager.Theme = theme;

            return RedirectToAction("Index");
        }

    }
}