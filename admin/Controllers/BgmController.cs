using Infrastructure.common;
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
            return View(musics);
        }

        [HttpPost]
        public IActionResult UploadMusic(IFormFile music)
        {
            using Stream stream = music.OpenReadStream();
            byte[] buffer = new byte[stream.Length];
            BgmManager.SaveMusic(music.FileName, buffer);
            return RedirectToAction("index");
        }

        public IActionResult DeleteMusic(string musicName)
        {
            BgmManager.DelMusic(musicName);
            return RedirectToAction("index");
        }

    }
}