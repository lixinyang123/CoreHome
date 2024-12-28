using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Components
{
    public class CategoriesViewComponent(ArticleDbContext articleDbContext) : ViewComponent
    {
        private readonly ArticleDbContext articleDbContext = articleDbContext;

        public IViewComponentResult Invoke()
        {
            List<Category> categoriesList = articleDbContext.Categories
                .OrderByDescending(i => i.Articles.Count)
                .Take(10)
                .ToList();
            return View(categoriesList);
        }
    }
}
