using CoreHome.Data.Model;
using Infrastructure.common;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace coreHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly VerificationCodeHelper verificationHelper;

        public HomeController(IWebHostEnvironment env)
        {
            environment = env;
            verificationHelper = new VerificationCodeHelper();
        }

        public IActionResult Index()
        {
            SearchEngineService.PushToBaidu(environment.WebRootPath);

            ViewBag.Title = "LLLXY";
            string lastTime = Request.Cookies["lastTime"];

            DateTime now = DateTime.Now;
            if (lastTime != null)
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(lastTime, "yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                    int ts = now.Subtract(dt).Days;
                    if (ts < 1)
                    {
                        ViewBag.Title = "Welcome Back !";
                    }
                }
                catch (Exception) { }
            }

            Response.Cookies.Append("lastTime", now.ToString("yyyy/MM/dd hh:mm:ss"), new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(30)
            });
            return View();
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Message(string msg, string url)
        {
            ViewBag.Msg = msg;
            ViewBag.Url = url;
            return View();
        }

        public IActionResult VerificationCode()
        {
            ISession session = HttpContext.Session;
            session.SetString("VerificationCode", verificationHelper.VerificationCode.ToLower());
            return File(verificationHelper.VerificationImage, "image/png");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
