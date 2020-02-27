using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Components
{
    public class PaginationViewComponent : ViewComponent
    {
        private readonly ArticleDbContext articleDbContext;

        public PaginationViewComponent(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
