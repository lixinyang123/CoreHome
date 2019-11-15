using System;
using System.IO;
using System.Diagnostics;
using DataContext.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.common;
using Infrastructure.Models;

namespace coreHome.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IWebHostEnvironment env)
        {
            SearchEngineService.PushToBaidu(env.WebRootPath);
        }

        public IActionResult Index()
        {
            ViewBag.Title = "LLLXY";
            string lastTime = Request.Cookies["lastTime"];

            DateTime now = DateTime.Now;
            if (lastTime != null)
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(lastTime, "yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                    int ts = now.Subtract(dt).Days;
                    if (ts < 1)
                    {
                        ViewBag.Title = "Welcome Back !";
                    }
                }
                catch (Exception) { }
            }

            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30)
            };
            Response.Cookies.Append("lastTime", now.ToString("yyyy/MM/dd hh:mm:ss"), options);
            return View();
        }

        public IActionResult Background()
        {
            if(System.IO.File.Exists(ThemeManager.backgroundUrl))
            {
                byte[] buffer = System.IO.File.ReadAllBytes(ThemeManager.backgroundUrl);
                return File(buffer, "image/png");
            }
            else
            {
                Theme theme = ThemeManager.GetTheme();
                theme.backgroundType = BackgroundType.Color;
                ThemeManager.ChangeTheme(theme);
                return NotFound();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Message(string msg,string url)
        {
            ViewBag.Msg = msg;
            ViewBag.Url = url;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
