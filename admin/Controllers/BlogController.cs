using DataContext.DbOperator;
using DataContext.Models;
using Infrastructure.common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace admin.Controllers
{
    public class BlogController : VerifyController
    {
        private readonly ArticleOperator articleRepository;
        private readonly int pageSize = 20;

        public BlogController(IMemoryCache _cache) : base(_cache)
        {
            articleRepository = new ArticleOperator();
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

        public IActionResult DeleteArticle(int id)
        {
            articleRepository.Delete(id);
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
                int id = Convert.ToInt32(Request.Query["id"]);
                Article article = articleRepository.Find(id);
                return View(article);
            }
        }

    }
}