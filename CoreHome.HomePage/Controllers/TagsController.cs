using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.HomePage.Controllers
{
    public class TagsController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public TagsController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Tags";
            return View();
        }

        /// <summary>
        /// 所有标签
        /// </summary>
        /// <returns>所有标签页面</returns>
        public async Task<IActionResult> AllTags()
        {
            List<Tag> tags = await articleDbContext.Tags
                .AsNoTracking()
                .Include(i => i.ArticleTags)
                .ToListAsync();

            List<List<string>> wordClouds = new();

            tags.AsParallel().ForAll(tag =>
            {
                wordClouds.Add(new List<string>()
                {
                    {tag.TagName },
                    {(tag.ArticleTags.Count*8).ToString() }
                });
            });

            return Json(wordClouds);
        }
    }
}