using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly ArticleDbContext articleDbContext;

        public NavbarViewComponent(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IViewComponentResult Invoke()
        {
            List<Category> categories = articleDbContext.Categories.ToList();
            return View(categories);
        }
    }
}
