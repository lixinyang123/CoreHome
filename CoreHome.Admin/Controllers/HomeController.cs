using CoreHome.Admin.Services;
using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using CoreHome.Infrastructure.Services;
using CoreHome.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace CoreHome.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly IMemoryCache cache;
        private readonly NotifyService notifyService;
        private readonly SecurityService securityService;
        private readonly ProfileService profileService;

        public HomeController(ArticleDbContext articleDbContext,
            IMemoryCache cache,
            NotifyService notifyService,
            SecurityService securityService,
            ProfileService profileService)
        {
            this.articleDbContext = articleDbContext;
            this.cache = cache;
            this.notifyService = notifyService;
            this.securityService = securityService;
            this.profileService = profileService;
        }

        public IActionResult Index()
        {
            //有管理员权限，或未设定管理员密码，直接跳转到 Overview 验证访问令牌
            if (Request.Cookies.TryGetValue("accessToken", out _) || string.IsNullOrEmpty(profileService.Config.AdminPassword))
            {
                return Redirect("/Admin/Overview");
            }

            if (!Request.Cookies.TryGetValue("user", out string _))
            {
                string cacheKey = Guid.NewGuid().ToString();
                Response.Cookies.Append("user", cacheKey, new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });
            }

            return View();
        }

        public async Task<IActionResult> Login()
        {
            //随机生成密码
            string password = Guid.NewGuid().ToString()[..6];
            string cacheKey = Request.Cookies["user"];

            //记录密码并设置过期时间为一分钟
            _ = cache.Set(cacheKey, password, DateTimeOffset.Now.AddMinutes(1));
            try
            {
                string title = "[ Login Notify ]";
                string content = $"### VerifyCode\n > {password}";

                _ = await articleDbContext.Notifications.AddAsync(new Notification(title, content));
                _ = await articleDbContext.SaveChangesAsync();

                notifyService.PushNotify(title, content);
                return Content("验证码已经发送");
            }
            catch (Exception)
            {
                return Content("网络错误");
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult VerifyPassword([FromForm][Required] string pwd)
        {
            string cacheKey = null, password = null;
            try
            {
                cacheKey = Request.Cookies["user"];
                password = cache.Get(cacheKey)?.ToString();
            }
            catch (Exception) { }

            if (!ModelState.IsValid || string.IsNullOrEmpty(cacheKey))
            {
                return Redirect("/Admin/Home");
            }

            bool verifyAdminPwd = securityService.SHA256Encrypt(pwd) == profileService.Config.AdminPassword && !string.IsNullOrEmpty(pwd);
            bool verifyDynamicPwd = pwd == password && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(password);

            if (verifyAdminPwd || verifyDynamicPwd)
            {
                //颁发访问令牌
                Response.Cookies.Append("accessToken", securityService.AESEncrypt(cacheKey), new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });

                //重定向到仪表盘
                return Redirect("/Admin/Overview");
            }
            return Redirect("/Admin/Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
