using System.IO;
using Infrastructure.common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace admin.Controllers
{
    public class ThemeController : Controller
    {
        public IActionResult Index()
        {
            Theme theme = ThemeManager.GetTheme();
            return View(theme);
        }

        [HttpPost]
        public IActionResult ChangeTheme(int themeType,int backgroundType)
        {
            Theme theme = new Theme()
            {
                themeType = (ThemeType)themeType,
                backgroundType = (BackgroundType)backgroundType
            };

            if (theme.backgroundType == BackgroundType.Image)
            {
                IFormFile file = Request.Form.Files["background"];
                using Stream stream = file.OpenReadStream();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                System.IO.File.WriteAllBytes(ThemeManager.backgroundUrl, buffer);
            }

            ThemeManager.ChangeTheme(theme);

            return Redirect("/Home/Message?msg=更换主题成功&url=/Admin/Theme");
        }

    }
}