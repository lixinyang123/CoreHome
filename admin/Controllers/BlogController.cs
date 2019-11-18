using DataContext.DbOperator;
using DataContext.Models;
using Infrastructure.common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace admin.Controllers
{
    public class BlogController : AuthorizationController
    {
        private readonly IDbOperator<Article> articleRepository;
        private readonly IDbOperator<Comment> commentRepository;
        private readonly int pageSize = 20;

        public BlogController(IMemoryCache _cache,
            IDbOperator<Article> articleOperator,
            IDbOperator<Comment> commentOperator,
            IWebHostEnvironment env) : base(_cache, env)
        {
            articleRepository = articleOperator;
            commentRepository = commentOperator;
        }

        public IActionResult Index(int index)
        {
            index = PageManager.GetStartIndex(index, articleRepository.Count(), pageSize);
            List<Article> articles = articleRepository.Find(i => i.Title != null, index, pageSize);
            return View(articles);
        }

        public IActionResult UploadArticle()
        {
            if (Request.Method == "POST")
            {
                Article article = new Article()
                {
                    ArticleID = Guid.NewGuid().ToString(),
                    Title = Request.Form["title"],
                    Time = DateTime.Now.ToString("yyyy/MM/dd"),
                    Cover = Request.Form["cover"],
                    Overview = Request.Form["overview"],
                    Content = Request.Form["content"]
                };

                articleRepository.Add(article);

                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult DeleteArticle(string articleID)
        {
            articleRepository.Delete(articleID);
            return RedirectToAction("index");
        }

        public IActionResult ModifyArticle([FromForm]Article newArticle)
        {
            if (Request.Method == "POST")
            {
                articleRepository.Modify(newArticle);
                return RedirectToAction("index");
            }
            else
            {
                string articleID = Request.Query["articleID"];
                Article article = articleRepository.Find(articleID);
                return View(article);
            }
        }

        public IActionResult Comment(string articleID)
        {
            ViewBag.ArticleID = articleID;
            List<Comment> comments = commentRepository.Find(i => i.ArticleID == articleID, 0, commentRepository.Count());
            return View(comments);
        }

        public IActionResult DeleteComment(string articleID, string commentID)
        {
            commentRepository.Delete(commentID);
            return Redirect("/Admin/Blog/Comment?articleID=" + articleID);
        }

    }
}