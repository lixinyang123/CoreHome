using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Controllers
{
    public class ServiceController : Controller
    {
        private readonly VerificationCodeService verificationHelper;

        public ServiceController(VerificationCodeService verificationHelper)
        {
            this.verificationHelper = verificationHelper;
        }

        public IActionResult VerificationCode()
        {
            ISession session = HttpContext.Session;
            session.SetString("VerificationCode", verificationHelper.VerificationCode.ToLower());
            return File(verificationHelper.VerificationImage, "image/png");
        }
    }
}
