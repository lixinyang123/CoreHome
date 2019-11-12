using DataContext.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache cache;

        public HomeController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        public async Task<IActionResult> Index()
        {
            //有管理员权限的话直接跳转的Overview验证访问令牌
            if (HttpContext.Session.TryGetValue("accessToken", out byte[] value))
            {
                return Redirect("/Admin/Overview");
            }

            ViewBag.picUrl = await BingWallpaperService.GetUrl();
            return View();
        }

        public IActionResult Login()
        {
            string cacheKey = Guid.NewGuid().ToString();
            Response.Cookies.Append("user", cacheKey);

            //随机生成密码
            string password = Guid.NewGuid().ToString().Substring(0, 6);

            //记录密码并设置过期时间为一分钟
            cache.Set(cacheKey, password, DateTimeOffset.Now.AddMinutes(1));
            try
            {
                //发送密码到手机
                NotifyService.PushNotify("CoreHome", "VerifyCode：" + password);
                return Content("验证码已经发送");
            }
            catch (Exception)
            {
                return Content("网络错误");
            }
        }

        public IActionResult VerfyPassword([FromForm]string pwd)
        {
            string cacheKey = null, password = null;
            try
            {
                cacheKey = Request.Cookies["user"];
                password = cache.Get(cacheKey).ToString();
            }
            catch (Exception) { }

            if (pwd == password && pwd != null && password != null)
            {
                //生成访问令牌
                string accessToken = Guid.NewGuid().ToString();
                //颁发访问令牌
                ISession session = HttpContext.Session;
                session.SetString("accessToken", accessToken);

                //服务端维持两小时的状态保持
                cache.Set(cacheKey, accessToken, DateTimeOffset.Now.AddHours(2));

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
