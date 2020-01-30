using CoreHome.Admin.Filter;
using Infrastructure.common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class BgmController : Controller
    {
        public IActionResult Index()
        {
            List<string> musics = BgmManager.GetBgmList();
            ViewBag.Bgm = BgmManager.Bgm;
            return View(musics);
        }

        public IActionResult SetBgmType(int bgmType)
        {
            Bgm bgm = new Bgm()
            {
                BgmType = (BgmType)bgmType
            };

            //特殊处理的类别
            switch (bgm.BgmType)
            {
                case BgmType.Single:
                    List<string> musicList = BgmManager.GetBgmList();
                    if (musicList.Count > 0)
                    {
                        bgm.DefaultMusic = musicList[0];
                    }
                    break;

                case BgmType.Web:
                    bgm.Url = Request.Form["url"];
                    break;
            }

            BgmManager.Bgm = bgm;
            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult UploadMusic(IFormFile music)
        {
            using Stream stream = music.OpenReadStream();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            string fullPath = BgmManager.bgmPath + music.FileName;
            System.IO.File.WriteAllBytes(fullPath, buffer);
            return RedirectToAction("index");
        }

        public IActionResult DeleteMusic(string musicName)
        {
            System.IO.File.Delete(BgmManager.bgmPath + musicName);
            return RedirectToAction("index");
        }

        public IActionResult SetDefaultMusic(string musicName)
        {
            BgmManager.Bgm = new Bgm() { BgmType = BgmType.Single, DefaultMusic = musicName };
            return RedirectToAction("index");
        }

        public IActionResult Play(string musicName)
        {
            string fullPath = BgmManager.bgmPath + musicName;
            byte[] buffer = System.IO.File.ReadAllBytes(fullPath);
            return File(buffer, "audio/mpeg");
        }

    }
}