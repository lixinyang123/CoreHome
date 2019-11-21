using DataContext.Models;
using Infrastructure.common;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace coreHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly VerificationCodeHelper verificationHelper;

        public HomeController(IWebHostEnvironment env)
        {
            verificationHelper = new VerificationCodeHelper();
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
            if (System.IO.File.Exists(ThemeManager.backgroundUrl))
            {
                byte[] buffer = System.IO.File.ReadAllBytes(ThemeManager.backgroundUrl);
                return File(buffer, "image/png");
            }
            else
            {
                Theme theme = ThemeManager.GetTheme();
                theme.BackgroundType = BackgroundType.Color;
                ThemeManager.ChangeTheme(theme);
                return NotFound();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Message(string msg, string url)
        {
            ViewBag.Msg = msg;
            ViewBag.Url = url;
            return View();
        }

        public IActionResult VerificationCode()
        {
            ISession session = HttpContext.Session;
            session.SetString("Verification", verificationHelper.VerificationCode);
            return File(verificationHelper.VerificationImage, "image/png");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
