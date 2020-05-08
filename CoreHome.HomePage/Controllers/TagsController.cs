using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

            List<Tag> tags = articleDbContext.Tags.Include(i => i.ArticleTags).ToList();
            ViewBag.ArticleCount = articleDbContext.Articles.Count();
            return View(tags);
        }
    }
}