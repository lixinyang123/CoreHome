﻿using CoreHome.Admin.Filter;
using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ProjectController : Controller
    {
        private readonly HomePageService homePageService;

        public ProjectController(HomePageService homePageService)
        {
            this.homePageService = homePageService;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Project";

            return View(homePageService.Config);
        }

        public IActionResult Add()
        {
            ViewBag.PageTitle = "AddProject";
            return View(new Project());
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
                return View(project);
            }
        }
    }
}