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
        private HttpClient httpClient;

        public HomeController()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "lixinyang123");
        }

        public IActionResult Index(int index)
        {
            //检测不到session（检测到return view）
            return Redirect("https://github.com/login/oauth/authorize?client_id=ff20fbc177c00d6a7214");
        }

        public async Task<IActionResult> Login(string code)
        {
            //获取accessToken
            string codeUrl = @"https://github.com/login/oauth/access_token?client_id=ff20fbc177c00d6a7214&client_secret=d01a7148670c731fb98f13479d52b6c837c09523&code=" + code;
            HttpResponseMessage accessToken = await httpClient.GetAsync(codeUrl);
            string accessTokenStr = await accessToken.Content.ReadAsStringAsync();
            accessTokenStr = accessTokenStr.Replace("access_token=", "");

            //获取用户数据
            string accessTokenUrl = @"https://api.github.com/user?access_token=" + accessTokenStr;
            HttpResponseMessage userInfo = await httpClient.GetAsync(accessTokenUrl);
            string jsonStr = await userInfo.Content.ReadAsStringAsync();
            //解析JSON字符串
            Admin admin = JsonConvert.DeserializeObject<Admin>(jsonStr);
            
            //判断管理员

            //添加Session

            //跳转回Index
            return Content(jsonStr);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
