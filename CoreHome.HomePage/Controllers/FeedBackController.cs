using CoreHome.HomePage.ViewModels;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly NotifyService notifyService;

        public FeedbackController(NotifyService notifyService)
        {
            this.notifyService = notifyService;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Feedback";

            ViewBag.Warning = "反馈中心";
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm] FeedbackViewModel feedback)
        {
            ViewBag.PageTitle = "Feedback";

            if (!ModelState.IsValid)
            {
                ViewBag.Warning = "请完善反馈信息";
                return View(feedback);
            }

            string str = HttpContext.Session.GetString("VerificationCode");
            if (str == feedback.VerificationCode.ToLower())
            {
                notifyService.PushNotify(feedback.Title + $"[{feedback.Contact}]", feedback.Content);
                ViewBag.Warning = "感谢您的反馈，开发者会尽快答复";
                return View();
            }

            ViewBag.Warning = "验证码错误";
            return View(feedback);
        }
    }
}