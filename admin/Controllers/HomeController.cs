using admin.Models;
using DataContext.Models;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
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
            if(Request.Cookies.TryGetValue("admin",out string admin))
            {
                return Redirect("/Admin/Overview");
            }
            string url = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1";
            string jsonStr = await new HttpClient().GetStringAsync(url);
            BingWallpaper wallpaper = JsonConvert.DeserializeObject<BingWallpaper>(jsonStr);
            ViewBag.picUrl = "https://cn.bing.com" + wallpaper.images[0].url;
            Response.Cookies.Append("user", Guid.NewGuid().ToString());
            return View();
        }

        public IActionResult Login()
        {
            //随机生成密码
            string password = Guid.NewGuid().ToString().Substring(0, 6);
            //记录密码并设置过期时间为一分钟
            string cacheKey = Request.Cookies["user"];
            cache.Set(cacheKey, password, DateTimeOffset.Now.AddMinutes(1));
            try
            {
                //发送密码到手机
                NotifyManager.PushNotify("coreHomeVerfy", password);
                return Content("验证码已经发送");
            }
            catch (Exception)
            {
                return Content("网络错误");
            }
        }

        public IActionResult VerfyPassword([FromForm]string pwd)
        {
            string cacheKey = Request.Cookies["user"];
            string password = cache.Get(cacheKey).ToString();
            if (pwd == password)
            {
                //移除缓存
                cache.Remove(cacheKey);
                //生成访问令牌
                string accessToken = Guid.NewGuid().ToString();
                //赋予管理员权限
                string admin = Guid.NewGuid().ToString();
                CookieOptions cookieOptions = new CookieOptions() { Expires = DateTimeOffset.Now.AddHours(2) };
                Response.Cookies.Append("admin", admin, cookieOptions);
                //颁发访问令牌
                ISession session = HttpContext.Session;
                session.SetString(admin, accessToken);
                //服务端维持两小时的状态保持
                cache.Set(admin, accessToken, DateTimeOffset.Now.AddHours(2));

                //验证方式
                //通过cookie.Get("admin")值获取session和cache中的accessToken进行对比

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
