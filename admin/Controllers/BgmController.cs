using Infrastructure.common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.IO;

namespace admin.Controllers
{
    public class BgmController : AuthorizationController
    {
        public BgmController(IMemoryCache _cache, IWebHostEnvironment env) : base(_cache, env) { }

        public IActionResult Index()
        {
            List<string> musics = BgmManager.GetBgmList();
            ViewBag.Bgm = BgmManager.Bgm;
            return View(musics);
        }

        public IActionResult SetBgmType(int bgmType)
        {
            Bgm bgm = new Bgm();
            bgm.BgmType = (BgmType)bgmType;
            if(bgm.BgmType == BgmType.Single)
            {
                List<string> musicList = BgmManager.GetBgmList();
                if (musicList.Count > 0)
                {
                    bgm.DefaultMusic = musicList[0];
                }
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
            var fullPath = BgmManager.bgmPath + musicName;
            byte[] buffer = System.IO.File.ReadAllBytes(fullPath);
            return File(buffer, "audio/mpeg");
        }

    }
}