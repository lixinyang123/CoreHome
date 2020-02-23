using Infrastructure.common;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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

        public IActionResult Background()
        {
            if (System.IO.File.Exists(ThemeManager.backgroundUrl))
            {
                byte[] buffer = System.IO.File.ReadAllBytes(ThemeManager.backgroundUrl);
                return File(buffer, "image/png");
            }
            else
            {
                ThemeManager.Theme = new Theme();
                return NotFound();
            }
        }

        public IActionResult Bgm(int bgmType)
        {
            BgmType type = (BgmType)bgmType;

            if (type == BgmType.None)
            {
                return Accepted();
            }
            else if (type == BgmType.Single)
            {
                string fullPath = BgmManager.bgmPath + BgmManager.Bgm.DefaultMusic;
                if (System.IO.File.Exists(fullPath))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(fullPath);
                    return File(buffer, "audio/mpeg");
                }
                else
                {
                    BgmManager.Bgm = new Bgm();
                    return Accepted();
                }
            }
            else if (type == BgmType.Random)
            {
                List<string> musicList = BgmManager.GetBgmList();
                int random = new Random().Next(0, musicList.Count);
                string fullPath = BgmManager.bgmPath + musicList[random];
                byte[] buffer = System.IO.File.ReadAllBytes(fullPath);
                return File(buffer, "audio/mpeg");
            }
            else
            {
                return NotFound();
            }
        }

    }
}
