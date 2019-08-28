using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using admin.Models;
using DataContext.Models;
using Infrastructure.Service;

namespace admin.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            string url = "http://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1";
            string jsonStr = await new HttpClient().GetStringAsync(url);
            BingWallpaper wallpaper = JsonConvert.DeserializeObject<BingWallpaper>(jsonStr);
            ViewBag.picUrl = "http://cn.bing.com" + wallpaper.images[0].url;
            return View();
        }

        public void Login()
        {
            //随机生成密码
            string password = Guid.NewGuid().ToString().Substring(0, 6);
            //发送密码到手机
            NotifyManager.PushNotify("coreHome admin password", password);
            TempData.Add("pwd", password);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
