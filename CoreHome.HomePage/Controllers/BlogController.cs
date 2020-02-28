using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using CoreHome.Data.Model;
using Microsoft.Extensions.Configuration;

namespace CoreHome.HomePage.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly int pageSize;

        public BlogController(ArticleDbContext articleDbContext,IConfiguration configuration)
        {
            this.articleDbContext = articleDbContext;
            pageSize = configuration.GetValue<int>("PageSize");
        }

        public IActionResult Index(int index = 1)
        {
            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i => i.Id)
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToList();

            ViewBag.Index = index;

            return View(articles);
        }
    }
}