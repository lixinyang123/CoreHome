using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using CoreHome.Data.Model;

namespace CoreHome.HomePage.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private const int pageSize = 7;

        public BlogController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index(int page = 1)
        {
            List<Article> articles = articleDbContext.Articles.OrderBy(i => i.Id)
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            return View(articles);
        }
    }
}