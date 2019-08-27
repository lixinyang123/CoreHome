using DataContext.DbOperator;
using DataContext.Models;
using Infrastructure.common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace admin.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleRepository articleRepository;
        private readonly int pageSize = 20;

        public BlogController()
        {
            articleRepository = new ArticleRepository();
        }

        public IActionResult Index(int index)
        {
            index = PageManager.GetStartIndex(index, articleRepository.Count(), pageSize);
            List<Article> articles = articleRepository.Find(index, pageSize);
            return View(articles);
        }

        public IActionResult DelArticle(int id)
        {
            return Redirect("Index");
        }
    }
}