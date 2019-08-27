using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace coreHome.Controllers
{
    public class FeedBackController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "反馈中心";

            if (Request.Method == "POST")
            {
                string contact = Request.Form["contact"];
                string title = Request.Form["title"];
                string detail = Request.Form["detail"];

                NotifyManager.PushNotify(title + $"[{contact}]", detail);

                ViewBag.Title = "感谢您的反馈";
            }
            return View();
        }
    }
}