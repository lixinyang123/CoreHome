using CoreHome.Admin.Filter;
using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
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
            var config = profileService.Config;
            profile.WhatsNew = config.WhatsNew;
            profile.FriendLinks = config.FriendLinks;
            profile.About = config.About;

            profileService.Config = profile;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UploadAvatar(IFormFile file)
        {
            using Stream stream = file.OpenReadStream();
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

        [HttpPost]
        public IActionResult AddFooterLink(FooterLink footerLink)
        {
            if (ModelState.IsValid)
            {
                footerLink.Id = Guid.NewGuid().ToString();
                var config = profileService.Config;

                switch (Request.Query["type"])
                {
                    case "WhatsNew":
                        config.WhatsNew.Add(footerLink);
                        break;

                    case "FriendLinks":
                        config.FriendLinks.Add(footerLink);
                        break;

                    case "About":
                        config.About.Add(footerLink);
                        break;

                    case "Others":
                        config.Others.Add(footerLink);
                        break;
                }

                profileService.Config = config;
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteFooterLink(string id)
        {
            if (id != null)
            {
                var config = profileService.Config;

                switch (Request.Query["type"])
                {
                    case "WhatsNew":
                        config.WhatsNew.RemoveAt(config.WhatsNew.FindIndex(i => i.Id == id));
                        break;

                    case "FriendLinks":
                        config.FriendLinks.RemoveAt(config.FriendLinks.FindIndex(i => i.Id == id));
                        break;

                    case "About":
                        config.About.RemoveAt(config.About.FindIndex(i => i.Id == id));
                        break;

                    case "Others":
                        config.Others.RemoveAt(config.Others.FindIndex(i => i.Id == id));
                        break;
                }
                profileService.Config = config;
            }

            return RedirectToAction("Index");
        }

    }
}
