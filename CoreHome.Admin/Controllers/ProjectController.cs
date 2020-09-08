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
    public class ProjectController : Controller
    {
        private readonly HomePageService homePageService;
        private readonly OssService ossService;

        public ProjectController(HomePageService homePageService, OssService ossService)
        {
            this.homePageService = homePageService;
            this.ossService = ossService;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Project";
            return View();
        }

        public IActionResult Add()
        {
            ViewBag.PageTitle = "Add Project";
            ViewBag.Action = "Add";
            return View("Editor", new Project());
        }

        [HttpPost]
        public IActionResult Add(Project project)
        {
            if (ModelState.IsValid)
            {
                List<Project> projects = homePageService.Config;
                project.Id = Guid.NewGuid().ToString();
                projects.Add(project);
                homePageService.Config = projects;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Action = "Add";
                return View("Editor", project);
            }
        }

        public IActionResult Delete(string id)
        {
            var index = homePageService.Config.FindIndex(i => i.Id == id);
            if (index >= 0)
            {
                var projects = homePageService.Config;
                projects.RemoveAt(index);
                homePageService.Config = projects;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.PageTitle = "Edit Project";
            ViewBag.Action = "Edit";
            Project project = homePageService.Config.SingleOrDefault(i => i.Id == id);
            if (project == null)
            {
                return RedirectToAction("Index");
            }
            return View("Editor", project);
        }

        [HttpPost]
        public IActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                var index = homePageService.Config.FindIndex(i => i.Id == project.Id);
                if (index >= 0)
                {
                    var projects = homePageService.Config;
                    projects[index] = project;
                    homePageService.Config = projects;
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Action = "Edit";
                return View("Editor", project);
            }
        }

        public IActionResult MoveUp(string id)
        {
            ViewBag.PageTitle = "Edit Project";
            ViewBag.Action = "Edit";
            var index = homePageService.Config.FindIndex(i => i.Id == id);
            if (index > 0)
            {
                var projects = homePageService.Config;

                var tempProject = projects[index];
                projects[index] = projects[index - 1];
                projects[index - 1] = tempProject;

                homePageService.Config = projects;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Reset()
        {
            homePageService.ResetConfig();
            return Content("Reset Successful");
        }

        [HttpPost]
        public IActionResult UploadCover(IFormFile file)
        {
            using Stream stream = file.OpenReadStream();
            try
            {
                return Content(ossService.UploadProjCover(file.FileName, stream));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

    }
}
