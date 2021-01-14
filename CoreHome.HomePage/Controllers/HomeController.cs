using CoreHome.Infrastructure.Services;
using CoreHome.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CoreHome.HomePage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<string> logger;
        private readonly IWebHostEnvironment environment;
        private readonly SearchEngineService searchEngineService;

        public HomeController(IWebHostEnvironment environment, SearchEngineService searchEngineService, ILogger<string> logger)
        {
            this.logger = logger;
            this.environment = environment;
            this.searchEngineService = searchEngineService;
        }

        public IActionResult Index()
        {
            Task.Run(() =>
            {
                var log = searchEngineService.PushToBaidu(environment.WebRootPath).Result;
                logger.LogInformation($"Push to Baidu：{log}");
            });

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
