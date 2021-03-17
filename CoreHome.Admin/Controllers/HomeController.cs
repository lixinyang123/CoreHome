using CoreHome.Admin.Services;
using CoreHome.Infrastructure.Services;
using CoreHome.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace CoreHome.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache cache;
        private readonly NotifyService notifyService;
        private readonly SecurityService securityService;
        private readonly ProfileService profileService;

        public HomeController(IMemoryCache cache,
            NotifyService notifyService,
            SecurityService securityService,
            ProfileService profileService)
        {
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
            return View();
        }

        public IActionResult Login()
        {
            string cacheKey = Guid.NewGuid().ToString();
            Response.Cookies.Append("user", cacheKey, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(1)
            });

            //随机生成密码
            string password = Guid.NewGuid().ToString().Substring(0, 6);

            //记录密码并设置过期时间为一分钟
            cache.Set(cacheKey, password, DateTimeOffset.Now.AddMinutes(1));
            try
            {
                //发送密码到手机
                notifyService.PushNotify("CoreHomeLogin", "VerifyCode：" + password);
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
        public IActionResult VerifyPassword([FromForm] string pwd)
        {
            string cacheKey = null, password = null;
            try
            {
                cacheKey = Request.Cookies["user"];
                password = cache.Get(cacheKey).ToString();
            }
            catch (Exception) { }

            var verifyAdminPwd = securityService.SHA256Encrypt(pwd) == profileService.Config.AdminPassword && !string.IsNullOrEmpty(pwd);
            var verifyDynamicPwd = pwd == password && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(password);

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
