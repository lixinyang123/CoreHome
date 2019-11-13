using Infrastructure.common;
using Infrastructure.Models;
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
    }
}