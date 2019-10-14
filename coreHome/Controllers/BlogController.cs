using System;
using System.Collections.Generic;
using System.Text;
using DataContext.DbOperator;
using DataContext.Models;
using Infrastructure.common;
using Infrastructure.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace coreHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly IDbOperator<Article> articleRepository;
        private readonly int pageSize = 5;

        public BlogController(IWebHostEnvironment env,IDbOperator<Article> articleOperator)
        {
            articleRepository = articleOperator;
            SearchEngineService.PushToBaidu(env.WebRootPath);
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

        public IActionResult Detail(int id)
        {
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

        public IActionResult Comment([FromForm]int id,string detail)
        {
            Article article = articleRepository.Find(id);
            article.Comments.Add(new Comment() { Time = DateTime.Now.ToString(), Detail = detail });
            articleRepository.Modify(article);
            return Content("<h1>评论成功<br/><a onclick='history.back(-1)' href='#'>返回</a></h1>","text/html", Encoding.GetEncoding("GB2312"));
        }

    }
}