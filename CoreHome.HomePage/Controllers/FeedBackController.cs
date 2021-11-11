using CoreHome.HomePage.ViewModels;
using CoreHome.Infrastructure.Services;
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

        /// <summary>
        /// 访问反馈
        /// </summary>
        /// <returns>反馈页面</returns>
        public IActionResult Index()
        {
            ViewBag.PageTitle = "Feedback";

            ViewBag.Warning = "Feedback Center";
            return View();
        }

        /// <summary>
        /// 处理反馈
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index([FromForm] FeedbackViewModel feedback)
        {
            ViewBag.PageTitle = "Feedback";

            if (!ModelState.IsValid)
            {
                ViewBag.Warning = "Please refine your feedback";
                return View(feedback);
            }

            string str = HttpContext.Session.GetString("VerificationCode");
            if (str == feedback.VerificationCode.ToLower())
            {
                notifyService.PushNotify("New feedback" + $"[{feedback.Title}/{feedback.Contact}]", feedback.Content);
                ViewBag.Warning = "Thank you for your feedback";
                return View();
            }

            ViewBag.Warning = "The verification code is wrong";
            return View(feedback);
        }
    }
}