using Infrastructure.Services;
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

                NotifyService.PushNotify(title + $"[{contact}]", detail);

                return Redirect("/Home/Message?msg=感谢您的反馈，开发者会尽快答复&url=/Feedback");
            }
            return View();
        }
    }
}