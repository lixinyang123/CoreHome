using DataContext.DbOperator;
using DataContext.Models;
using Infrastructure.common;
using Infrastructure.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace coreHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly ArticleRepository articleRepository;
        private readonly int pageSize = 5;

        public BlogController(IHostingEnvironment env)
        {
            environment = env;
            articleRepository = new ArticleRepository();

            SearchEngineService.PushToBaidu(environment.WebRootPath);
        }

        public IActionResult Index(int index)
        {
            index = PageManager.GetStartIndex(index, articleRepository.Count(), pageSize);
            List<Article> articles = articleRepository.Find(index, pageSize);

            //获取页面总数
            ViewBag.LastPage = PageManager.GetLastPage(articleRepository.Count(), pageSize);

            if (articles.Count == 0)
            {
                ViewBag.Warning = "目前这里什么也没有";
            }

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];
            return View(articles);
        }

        public IActionResult Detail(int id, int index)
        {
            ViewBag.CurrentIndex = index;
            ViewBag.WebRootPath = environment.WebRootPath;
            Article article = articleRepository.Find(id);
            if (article != null)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("lastRead", id.ToString(), options);
                return View(article);
            }
            else
            {
                return Content("没有此文章");
            }
        }

    }
}