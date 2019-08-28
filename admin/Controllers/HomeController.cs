using admin.Models;
using DataContext.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

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

        public IActionResult Login()
        {
            return Redirect("");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
