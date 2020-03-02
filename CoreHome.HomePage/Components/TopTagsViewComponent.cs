using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
            List<Tag> tagList = articleDbContext.Tags
                .OrderBy(i => i.ArticleTags.Count)
                .Take(10).ToList();
            return View(tagList);
        }
    }
}
