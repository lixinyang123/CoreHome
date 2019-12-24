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
                string str = session.GetString("VerificationCode");
                string code = Request.Form["VerificationCode"];

                if (code != null && code != string.Empty && str == code.ToLower())
                {
                    string contact = Request.Form["contact"];
                    string title = Request.Form["title"];
                    string detail = Request.Form["message"];

                    NotifyService.PushNotify(title + $"[{contact}]", detail);

                    return Ok();
                }
                return ValidationProblem("验证码错误");
            }
            return View();
        }
    }
}