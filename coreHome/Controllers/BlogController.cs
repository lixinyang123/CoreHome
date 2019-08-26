using coreHome.Service;
using DataContext.DbOperator;
using DataContext.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace coreHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly ArticleRepository articleRepository;
        private readonly int pageSize = 5;

        public BlogController(IHostingEnvironment env, IOptions<ConnectionStrings> seeting)
        {
            environment = env;
            articleRepository = new ArticleRepository(seeting);

            SearchEngineService.PushToBaidu(environment.WebRootPath);
        }

        public IActionResult Index(int index)
        {
            index = GetPageIndex(index);
            List<Article> articles = articleRepository.Find(index, pageSize);

            if (articles.Count == 0)
            {
                ViewBag.Warning = "目前这里什么也没有";
            }

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];
            return View(articles);
        }

        private int GetPageIndex(int index)
        {
            decimal lastPage = Math.Ceiling((decimal)articleRepository.Count() / pageSize);
            ViewBag.LastPage = lastPage;
            if (index >= lastPage)
            {
                index--;
            }

            if (index < 0)
            {
                index = 0;
            }

            return index;
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