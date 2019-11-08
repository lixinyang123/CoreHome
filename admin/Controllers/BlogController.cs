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
    public class BlogController : VerifyController
    {
        private readonly IDbOperator<Article> articleRepository;
        private readonly int pageSize = 20;

        public BlogController(IMemoryCache _cache,
            IDbOperator<Article> articleOperator,
            IWebHostEnvironment env) : base(_cache,env)
        {
            articleRepository = articleOperator;
        }

        public IActionResult Index(int index)
        {
            index = PageManager.GetStartIndex(index, articleRepository.Count(), pageSize);
            List<Article> articles = articleRepository.Find(index, pageSize);
            return View(articles);
        }

        public IActionResult UploadArticle()
        {
            if (Request.Method == "POST")
            {
                Article article = new Article()
                {
                    ArticleID = Guid.NewGuid().ToString(),
                    //标题
                    Title = Request.Form["title"],
                    //时间
                    Time = DateTime.Now.ToString("yyyy/MM/dd"),
                    //封面
                    Cover = Request.Form["cover"],
                    //概述
                    Overview = Request.Form["overview"],
                    //内容
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
            if(Request.Method=="POST")
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

    }
}