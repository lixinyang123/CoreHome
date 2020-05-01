using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CoreHome.HomePage.Controllers
{
    public class ServiceController : Controller
    {
        private readonly VerificationCodeService verificationHelper;
        private readonly OssService ossService;

        public ServiceController(VerificationCodeService verificationHelper, OssService ossService)
        {
            this.verificationHelper = verificationHelper;
            this.ossService = ossService;
        }

        public IActionResult VerificationCode()
        {
            ISession session = HttpContext.Session;
            session.SetString("VerificationCode", verificationHelper.VerificationCode.ToLower());
            return File(verificationHelper.VerificationImage, "image/png");
        }

        public IActionResult BackgroundMusic()
        {
            List<string> musics = ossService.GetMusics();
            string music = musics[new Random().Next(musics.Count)];
            return Content(music);
        }
    }
}
