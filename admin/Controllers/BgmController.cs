using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.common;

namespace admin.Controllers
{
    public class BgmController : Controller
    {
        public IActionResult Index()
        {
            List<string> musics = BgmManager.GetBgmList();
            return View(musics);
        }

        [HttpPost]
        public IActionResult UploadMusic(IFormFile music)
        {
            return RedirectToAction("index");
        }

    }
}