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
        private readonly IDbOperator<Tag> tagRepository;
        private readonly int pageSize = 20;

        public BlogController(IMemoryCache _cache,
            IDbOperator<Article> articleOperator,
            IDbOperator<Comment> commentOperator,
            IDbOperator<Tag> tagOperator,
            IWebHostEnvironment env) : base(_cache, env)
        {
            articleRepository = articleOperator;
            commentRepository = commentOperator;
            tagRepository = tagOperator;
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
                //添加博客记录
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

                //添加博客标签记录
                new List<string>(Request.Form["tag"].ToString().TrimStart('#').Split("#")).ForEach(i =>
                {
                    tagRepository.Add(new Tag() { ArticleID = article.ArticleID, TagName = i });
                });

                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult DeleteArticle(string articleID)
        {
            //删除博客
            articleRepository.Delete(articleID);
            //删除此博客的所有评论
            commentRepository.Find(i => i.ArticleID == articleID, 0, commentRepository.Count()).ForEach(j => commentRepository.Delete(j.CommentID));
            //删除此博客所有标签
            tagRepository.Delete(articleID);

            return RedirectToAction("index");
        }

        public IActionResult ModifyArticle([FromForm]Article newArticle)
        {
            if (Request.Method == "POST")
            {
                articleRepository.Modify(newArticle);

                //修改博客标签记录（删除旧记录重新添加）
                tagRepository.Delete(newArticle.ArticleID);
                new List<string>(Request.Form["tag"].ToString().TrimStart('#').Split("#")).ForEach(i =>
                {
                    tagRepository.Add(new Tag() { ArticleID = newArticle.ArticleID, TagName = i });
                });

                return RedirectToAction("index");
            }
            else
            {
                string articleID = Request.Query["articleID"];
                Article article = articleRepository.Find(articleID);
                article.Tags = tagRepository.Find(i => i.ArticleID == article.ArticleID, 0, tagRepository.Count());
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