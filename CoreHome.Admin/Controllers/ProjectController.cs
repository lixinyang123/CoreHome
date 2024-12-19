using CoreHome.Admin.Filter;
using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ProjectController(HomePageService homePageService, OssService ossService) : Controller
    {
        private readonly HomePageService homePageService = homePageService;
        private readonly OssService ossService = ossService;

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

        [HttpPost]
        public IActionResult Delete(string id)
        {
            int index = homePageService.Config.FindIndex(i => i.Id == id);
            if (index >= 0)
            {
                List<Project> projects = homePageService.Config;
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
            return project == null ? RedirectToAction("Index") : View("Editor", project);
        }

        [HttpPost]
        public IActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                int index = homePageService.Config.FindIndex(i => i.Id == project.Id);
                if (index >= 0)
                {
                    List<Project> projects = homePageService.Config;
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

        [HttpPost]
        public IActionResult MoveUp(string id)
        {
            ViewBag.PageTitle = "Edit Project";
            ViewBag.Action = "Edit";
            int index = homePageService.Config.FindIndex(i => i.Id == id);
            if (index > 0)
            {
                List<Project> projects = homePageService.Config;

                (projects[index - 1], projects[index]) = (projects[index], projects[index - 1]);
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
                return Content(ossService.UploadProjCover(stream));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

    }
}
