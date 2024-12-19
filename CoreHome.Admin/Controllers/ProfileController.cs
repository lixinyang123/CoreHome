﻿using CoreHome.Admin.Filter;
using CoreHome.Admin.Services;
using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ProfileController(ProfileService profileService, OssService ossService, SecurityService securityService) : Controller
    {
        private readonly ProfileService profileService = profileService;
        private readonly OssService ossService = ossService;
        private readonly SecurityService securityService = securityService;

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Profile";
            return View(profileService.Config);
        }

        [HttpPost]
        public IActionResult Index(Profile profile)
        {
            ViewBag.PageTitle = "Profile";
            Profile config = profileService.Config;

            if (!ModelState.IsValid)
            {
                return View(config);
            }

            if (string.IsNullOrEmpty(profileService.Config.AdminPassword))
            {
                profile.AdminPassword = securityService.SHA256Encrypt(profile.AdminPassword);
            }

            profile.WhatsNew = config.WhatsNew;
            profile.FriendLinks = config.FriendLinks;
            profile.About = config.About;
            profile.Others = config.Others;

            profileService.Config = profile;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ResetPassword()
        {
            Profile config = profileService.Config;
            config.AdminPassword = string.Empty;
            profileService.Config = config;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UploadAvatar(IFormFile file)
        {
            using Stream stream = file.OpenReadStream();
            try
            {
                ossService.UploadAvatar(stream);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult AddFooterLink(FooterLink footerLink)
        {
            if (ModelState.IsValid)
            {
                footerLink.Id = Guid.NewGuid().ToString();
                Profile config = profileService.Config;

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

        [HttpPost]
        public IActionResult DeleteFooterLink(string id)
        {
            if (id != null)
            {
                Profile config = profileService.Config;

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
