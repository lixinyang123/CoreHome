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

        public async Task<IActionResult> AllTags()
        {
            List<Tag> tags = await articleDbContext.Tags.Include(i => i.ArticleTags).ToListAsync();

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