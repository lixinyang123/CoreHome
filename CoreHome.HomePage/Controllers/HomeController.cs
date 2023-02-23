using CoreHome.Infrastructure.Services;
using CoreHome.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // 推送到百度资源搜索平台
            _ = searchEngineService.PushToBaidu(environment.WebRootPath);

            ViewBag.PageTitle = "Home";

            string lastTime = Request.Cookies["IS_BACK"];

            if (lastTime != null)
            {
                ViewBag.Title = "Welcome Back !";
            }

            Response.Cookies.Append("IS_BACK", "true", new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(5)
            });
            return View();
        }

        /// <summary>
        /// 隐私页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            ViewBag.PageTitle = "Privacy";
            return View();
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.PageTitle = "Error";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
