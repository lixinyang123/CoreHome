using CoreHome.Infrastructure.Services;
using CoreHome.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;

namespace CoreHome.HomePage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly SearchEngineService searchEngineService;
        private readonly ProfileService profileService;


        public HomeController(IWebHostEnvironment environment, SearchEngineService searchEngineService, ProfileService profileService)
        {
            this.environment = environment;
            this.searchEngineService = searchEngineService;
            this.profileService = profileService;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Home";
            searchEngineService.PushToBaidu(environment.WebRootPath);

            ViewBag.Title = profileService.Config.Name;
            string lastTime = Request.Cookies["lastTime"];

            DateTime now = DateTime.Now;
            if (lastTime != null)
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(lastTime, "yyyy/MM/dd hh:mm:ss", CultureInfo.CurrentCulture);
                    int ts = now.Subtract(dt).Minutes;
                    if (ts < 5)
                    {
                        ViewBag.Title = "Welcome Back !";
                    }
                }
                catch (Exception) { }
            }

            Response.Cookies.Append("lastTime", now.ToString("yyyy/MM/dd hh:mm:ss"), new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(30)
            });
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.PageTitle = "Privacy";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.PageTitle = "Error";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
