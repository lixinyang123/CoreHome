using CoreHome.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Components
{
    public class ProjectViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Project project)
        {
            return View(project);
        }
    }
}
