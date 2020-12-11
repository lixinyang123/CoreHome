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

            ViewBag.Warning = "Feedback Center";
            return View();
        }

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