using System;
using coreHome.Models;
using coreHome.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace coreHome.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment environment;

        public HomeController(IHostingEnvironment env)
        {
            environment = env;

            SearchEngineService.PushToBaidu(environment.WebRootPath);
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

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Append("lastTime", now.ToString("yyyy/MM/dd hh:mm:ss"), options);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
