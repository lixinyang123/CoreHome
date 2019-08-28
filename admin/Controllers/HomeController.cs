using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using admin.Models;
using DataContext.Models;
using Infrastructure.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace admin.Controllers
{
    public class HomeController : Controller
    {
        private IMemoryCache cache;

        public HomeController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        public async Task<IActionResult> Index()
        {
            string url = "http://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1";
            string jsonStr = await new HttpClient().GetStringAsync(url);
            BingWallpaper wallpaper = JsonConvert.DeserializeObject<BingWallpaper>(jsonStr);
            ViewBag.picUrl = "http://cn.bing.com" + wallpaper.images[0].url;
            Response.Cookies.Append("user", Guid.NewGuid().ToString());
            return View();
        }

        public void Login()
        {
            //随机生成密码
            string password = Guid.NewGuid().ToString().Substring(0, 6);
            //记录密码并设置过期时间为一分钟
            string cacheKey = Request.Cookies["user"];
            cache.Set(cacheKey, password, DateTimeOffset.Now.AddMinutes(1));
            //发送密码到手机
            NotifyManager.PushNotify("coreHomeVerfy", password);
        }

        public IActionResult VerfyPassword([FromForm]string pwd)
        {
            string cacheKey = Request.Cookies["user"];
            string password = cache.Get(cacheKey).ToString();
            if (pwd==password)
            {
                //移除缓存
                cache.Remove(cacheKey);
                //生成访问令牌
                string accessToken = Guid.NewGuid().ToString();
                //颁发访问令牌
                ISession session = HttpContext.Session;
                cacheKey = "admin" + cacheKey;
                session.SetString(cacheKey, accessToken);
                //服务端维持一小时的状态保持
                cache.Set(cacheKey, accessToken, DateTimeOffset.Now.AddHours(2));

                //验证方式
                //通过"admin"+cookie值获取session和cache中的accessToken进行对比

                //重定向到仪表盘
                return Redirect("/Overview");
            }
            return Redirect("/Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
