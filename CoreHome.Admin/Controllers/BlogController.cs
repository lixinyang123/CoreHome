using CoreHome.Admin.ViewModels;
using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CoreHome.Admin.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public BlogController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index()
        {
            List<Article> articles = articleDbContext.Articles.ToList();
            return View(articles);
        }

        public IActionResult Upload()
        {
            return View(new ArticleViewModel());
        }

        [HttpPost]
        public IActionResult Upload(ArticleViewModel articleViewModel)
        {
            return View();
        }
    }
}