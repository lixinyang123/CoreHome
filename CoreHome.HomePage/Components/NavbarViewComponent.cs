using CoreHome.Data.DatabaseContext;
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
            return View();
        }
    }
}
