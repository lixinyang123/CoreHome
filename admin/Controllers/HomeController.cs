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
            Response.Cookies.Append("admin", Guid.NewGuid().ToString());
            return View();
        }

        public void Login()
        {
            //随机生成密码
            string password = Guid.NewGuid().ToString().Substring(0, 6);
            //发送密码到手机
            NotifyManager.PushNotify("coreHome admin password", password);
            //记录密码并设置过期时间为一分钟
            cache.Set(Request.Cookies["admin"], password, DateTimeOffset.Now.AddMinutes(1));
        }

        public IActionResult VerfyPassword([FromForm]string pwd)
        {
            string cacheKey = Request.Cookies["admin"];
            if(pwd== cache.Get(cacheKey).ToString())
            {
                //移除缓存
                cache.Remove(cacheKey);
                //添加Session

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
