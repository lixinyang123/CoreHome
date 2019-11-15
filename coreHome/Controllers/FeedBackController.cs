using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
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
                ISession session = HttpContext.Session;
                string str = session.GetString("Verification");
                string code = Request.Form["code"];
                if (str == null)
                {
                    return Redirect("/Home/Message?msg=请先同意隐私策略&url=/Feedback");
                }

                if (code != null && code != string.Empty && str == code.ToLower())
                {
                    string contact = Request.Form["contact"];
                    string title = Request.Form["title"];
                    string detail = Request.Form["detail"];

                    NotifyService.PushNotify(title + $"[{contact}]", detail);

                    return Redirect("/Home/Message?msg=感谢您的反馈，开发者会尽快答复&url=/Feedback");
                }
                return Redirect("/Home/Message?msg=验证码错误&url=/Feedback");
            }
            return View();
        }
    }
}