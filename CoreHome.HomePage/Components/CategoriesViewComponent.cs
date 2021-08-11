using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ArticleDbContext articleDbContext;

        public CategoriesViewComponent(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IViewComponentResult Invoke()
        {
            List<Category> categoriesList = articleDbContext.Categories.Take(10).ToList();
            return View(categoriesList);
        }
    }
}
