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

        public HomeController(IWebHostEnvironment environment, SearchEngineService searchEngineService)
        {
            this.environment = environment;
            this.searchEngineService = searchEngineService;
        }

        public IActionResult Index()
        {
            searchEngineService.PushToBaidu(environment.WebRootPath);

            ViewBag.PageTitle = "Home";

            string lastTime = Request.Cookies["lastTime"];

            if (lastTime != null)
            {
                ViewBag.Title = "Welcome Back !";
            }

            Response.Cookies.Append("lastTime", string.Empty, new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(5)
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
