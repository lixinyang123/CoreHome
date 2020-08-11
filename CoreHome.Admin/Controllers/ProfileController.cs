using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CoreHome.Admin.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProfileService profileService;
        private readonly OssService ossService;

        public ProfileController(ProfileService profileService, OssService ossService)
        {
            this.profileService = profileService;
            this.ossService = ossService;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Profile";
            return View(profileService.Config);
        }

        [HttpPost]
        public IActionResult Index(Profile profile)
        {
            profileService.Config = profile;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UploadAvatar(IFormFile avatar)
        {
            using Stream stream = avatar.OpenReadStream();
            try
            {
                return Content(ossService.UploadAvatar(stream));
            }
            catch (System.Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }
    }
}
