using CoreHome.Infrastructure.Services;
using CoreHome.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

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
            searchEngineService.PushToBaidu(environment.WebRootPath);

            ViewBag.PageTitle = "Home";
            ViewBag.Title = profileService.Config.Name;

            string lastTime = Request.Cookies["lastTime"];

            if (lastTime != null)
            {
                ViewBag.Title = "Welcome Back !";
            }

            Response.Cookies.Append("lastTime", string.Empty, new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(5),
                SameSite = SameSiteMode.Strict
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
