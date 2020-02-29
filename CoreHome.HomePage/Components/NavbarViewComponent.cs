using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
