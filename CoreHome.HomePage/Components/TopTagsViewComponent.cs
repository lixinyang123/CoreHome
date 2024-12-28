using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.HomePage.Components
{
    public class TopTagsViewComponent : ViewComponent
    {
        private readonly ArticleDbContext articleDbContext;

        public TopTagsViewComponent(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IViewComponentResult Invoke()
        {
            List<Tag> tagList = [.. articleDbContext.Tags
                .OrderByDescending(i => i.ArticleTags.Count)
                .Take(10)];
            return View(tagList);
        }
    }
}
